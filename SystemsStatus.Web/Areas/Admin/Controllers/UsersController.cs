using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Common.Authentication;
using SystemsStatus.Web.Areas.Admin.Attributes;
using SystemsStatus.Common.Pagination;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    [UserAuthorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;
        private readonly IUserRoleService _userRoleService;

        private const int DEFAULT_PAGE_SIZE = 10;

        public UsersController(IUserService userService, IServiceService serviceService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _serviceService = serviceService;
            _userRoleService = userRoleService;
        }

        //
        // GET: /Admin/Users/

        public ActionResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var users = _userService.GetAllUsers()
                            .OrderBy(x => x.LastName.ToUpper());

            return View(users.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE));
        }

        //
        // GET: /Admin/Users/Create

        public ActionResult Create()
        {
            var vm = new UsersCreateViewModel();
            vm.User = new User();
            vm.Roles = _userRoleService.GetAllUserRoles().OrderBy(x => x.Name.ToUpper()).ToList();

            return View(vm);
        }

        //
        // POST: /Admin/Users/Create

        [HttpPost]
        public ActionResult Create(UsersCreateViewModel vm)
        {
            vm.Roles = _userRoleService.GetAllUserRoles().OrderBy(x => x.Name.ToUpper()).ToList();

            if (ModelState.IsValid)
            {
                vm.User.PasswordSalt = AuthPasswordHelper.GenerateSalt(100);
                vm.User.HashedPassword = AuthPasswordHelper.HashEncode(vm.Password, vm.User.PasswordSalt, 100);
                vm.User.Active = true;

                _userService.InsertUser(vm.User);

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        //
        // GET: /Admin/Users/Edit/{id}

        public ActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);
            
            if (user == null)
                throw new HttpException(404, String.Format("No user found with id = '{0}'", id));

            var vm = new UsersEditViewModel();
            vm.User = user;
            vm.Services = _serviceService.GetAllServices().OrderBy(x => x.Name.ToUpper()).ToList();
            vm.Roles = _userRoleService.GetAllUserRoles().OrderBy(x => x.Name.ToUpper()).ToList();

            foreach (var service in user.Services)
            {
                vm.ServiceIds.Add(service.Id.Value);
            }

            return View(vm);
        }

        //
        // POST: /Admin/Users/Edit/{id}

        [HttpPost]
        public ActionResult Edit(int id, UsersEditViewModel vm)
        {
            var entity = _userService.GetUserById(id);

            if (entity == null)
                throw new HttpException(404, String.Format("No user found with id = '{0}'", id));

            vm.Services = _serviceService.GetAllServices().OrderBy(x => x.Name.ToUpper()).ToList();
            vm.Roles = _userRoleService.GetAllUserRoles().OrderBy(x => x.Name.ToUpper()).ToList();

            if (ModelState.IsValid)
            {
                if (_userService.GetAllUsers().Where(x => x.Username.ToLower() == vm.User.Username.ToLower() && x.Id != id).Count() > 0)
                {
                    ModelState.AddModelError("User.Username", "A user with this username already exists. Please choose another.");
                    return View(vm);
                }

                // Bring over values not submitted by form
                vm.User.Id = id;
                vm.User.HashedPassword = entity.HashedPassword;
                vm.User.PasswordSalt = entity.PasswordSalt;

                foreach (int serviceId in vm.ServiceIds)
                {
                    vm.User.Services.Add(new Service
                    {
                        Id = serviceId
                    });
                }

                // Merge the entity
                vm.User = _userService.MergeUser(vm.User);

                // Save the user
                vm.User = _userService.SaveUser(vm.User);

                ViewBag.Saved = true;

                return View(vm);
            }
            

            return View(vm);
        }
    }
}
