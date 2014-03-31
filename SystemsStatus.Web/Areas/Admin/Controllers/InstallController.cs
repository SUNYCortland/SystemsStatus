using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Services;
using SystemsStatus.Common.Authentication;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    public class InstallController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public InstallController(IUserService userService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
        }

        //
        // GET: /Admin/Install/

        public ActionResult Index()
        {
            var user = new User();

            var users = _userService.GetAllUsers()
                            .ToList();

            if (users != null
                || users.Count() > 0)
            {
                return View("AlreadyInstalled");
            }

            return View(user);
        }

        //
        // POST: /Admin/Install

        [HttpPost]
        public ActionResult Install(User user)
        {
            var users = _userService.GetAllUsers()
                            .ToList();

            if (users != null
                || users.Count() > 0)
            {
                return View("AlreadyInstalled");
            }

            user.Role = _userRoleService.GetAllUserRoles()
                            .Where(x => x.Name == "Administrator")
                            .SingleOrDefault();

            if (ModelState.IsValid)
            {
                user.PasswordSalt = AuthPasswordHelper.GenerateSalt(100);
                user.HashedPassword = AuthPasswordHelper.HashEncode(user.HashedPassword, user.PasswordSalt, 100);
                user.Active = true;

                _userService.SaveUser(user);

                return RedirectToAction("Index", new { controller = "Auth" });
            }

            return View(user);
        }

    }
}
