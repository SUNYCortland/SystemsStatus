using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Common.Authentication;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Common.Pagination;
using SystemsStatus.Core.Data.Entities.Statuses;
using SystemsStatus.Web.Areas.Admin.Attributes;
using SystemsStatus.Web.Areas.Admin.Authentication;
using Castle.Core.Logging;
using SystemsStatus.Web.ViewModels;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    [UserAuthorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;
        private readonly IWidgetService _widgetService;
        private readonly IDepartmentService _departmentService;
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;

        private const int DEFAULT_PAGE_SIZE = 10;

        public ILogger Logger { get; set; }

        public AccountController(IUserService userService, 
                                    IServiceService serviceService,
                                    IWidgetService widgetService,
                                    IDepartmentService departmentService,
                                    IScheduledMaintenanceService scheduledMaintenanceService)
        {
            _userService = userService;
            _serviceService = serviceService;
            _widgetService = widgetService;
            _departmentService = departmentService;
            _scheduledMaintenanceService = scheduledMaintenanceService;
        }

        //
        // GET: /Admin/Account/

        public ActionResult Index()
        {
            var user = _userService.GetUserByUsername(HttpContext.User.Identity.Name);

            if (user == null)
                throw new HttpException(404, String.Format("No user found with username = '{0}'", HttpContext.User.Identity.Name));

            return View(user);
        }

        //
        // POST: /Admin/Account/

        [HttpPost]
        public ActionResult Index(User user)
        {
            var currentUser = _userService.GetUserByUsername(HttpContext.User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Get values not submitted by form
                user.Username = currentUser.Username;
                user.Id = currentUser.Id;
                user.PasswordSalt = currentUser.PasswordSalt;
                user.HashedPassword = currentUser.HashedPassword;
                user.Role = currentUser.Role;
                user.Services = currentUser.Services;
                user.Active = currentUser.Active;

                // Merge user entity
                user = _userService.MergeUser(user);

                // Save user to database
                user = _userService.SaveUser(user);

                ViewBag.Saved = true;
            }

            return View(user);
        }

        //
        // GET: /Admin/Account/MyServices

        public ActionResult MyServices(int? page, string display, string query)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1: 0;

            var user = _userService.GetUserByUsername(((UserPrincipal)HttpContext.User).Username);
            var services = _serviceService.GetAllServices();

            if (!String.IsNullOrEmpty(display))
            {
                switch (display.ToLower())
                {
                    case "online":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OnlineServiceStatus));
                        break;

                    case "maintenance":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OfflineMaintenanceServiceStatus));
                        break;

                    case "degraded":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OnlineDegradedServiceStatus));
                        break;

                    case "down":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OfflineUnplannedServiceStatus));
                        break;
                }
            }

            if (!String.IsNullOrEmpty(query))
            {
                services = services.Where(x => (x.Name.ToLower().Contains(query.ToLower()))
                    || (x.Description.ToLower().Contains(query.ToLower()))
                    || (x.Department != null && x.Department.Name.ToLower().Contains(query.ToLower())));
            }

            services = services.Where(x => x.Owners.Contains(user)).OrderBy(x => x.Name.ToUpper());

            var servicesPaged = services.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            // The specified page is too large
            if (currentPageIndex >= servicesPaged.PageCount)
                return View(services.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(servicesPaged);
        }

        //
        // GET: Admin/Account/ChangePassword

        public ActionResult ChangePassword()
        {
            var vm = new AccountChangePasswordViewModel();

            return View(vm);
        }

        //
        // POST: Admin/Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(AccountChangePasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUsername(HttpContext.User.Identity.Name);

                if (user == null)
                    throw new HttpException(404, String.Format("No user found with username = '{0}'", HttpContext.User.Identity.Name));

                user.HashedPassword = AuthPasswordHelper.HashEncode(vm.NewPassword, user.PasswordSalt, 100);

                _userService.SaveUser(user);

                ViewBag.Saved = true;
            }

            return View(vm);
        }

        //
        // GET: Admin/Account/MyWidgets

        public ActionResult MyWidgets(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var owner = _userService.GetUserByUsername(((UserPrincipal)HttpContext.User).Username);
            var widgets = _widgetService.GetAllWidgetsByOwner(owner);

            var widgetsPaged = widgets.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            // The specified page is too large
            if (currentPageIndex >= widgetsPaged.PageCount)
                return View(widgets.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(widgetsPaged);
        }

        //
        // GET: Admin/Account/CreateWidget

        public ActionResult CreateWidget()
        {
            var vm = new AccountCreateWidgetViewModel();
            var services = _serviceService.GetAllServices()
                                .OrderBy(x => x.Name.ToUpper());

            vm.Widget = new Widget();
            vm.Services = services.ToList();
            vm.Departments = _departmentService.GetAllDepartments()
                                .OrderBy(x => x.Name.ToUpper())
                                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                                .ToList();

            return View(vm);
        }

        //
        // POST: Admin/Account/CreateWidget

        [HttpPost]
        public ActionResult CreateWidget(AccountCreateWidgetViewModel vm)
        {
            var valid = true;

            if (ModelState.IsValid)
            {
                // Only add specified services if a department is not selected
                if (!vm.DepartmentId.HasValue)
                {
                    foreach (var serviceId in vm.ServiceIds)
                    {
                        vm.Widget.Services.Add(new Service
                        {
                            Id = serviceId
                        });
                    }
                }
                else
                {
                    var department = _departmentService.GetDepartmentById(vm.DepartmentId.Value);

                    // Make sure this department exists
                    if (department != null)
                    {
                        vm.Widget.Department = department;
                    }
                    else
                    {
                        ModelState.AddModelError("Widget.Department", "You've specified an invalid department. Please select another.");
                        valid = false;
                    }
                }

                var owner = _userService.GetUserByUsername(((UserPrincipal)HttpContext.User).Username);

                vm.Widget.Owner = owner;

                if (valid)
                {
                    var widget = _widgetService.SaveWidget(vm.Widget);

                    return RedirectToAction("MyWidgets");
                }
            }

            vm.Services = _serviceService.GetAllServices()
                                .OrderBy(x => x.Name.ToUpper())
                                .ToList();

            vm.Departments = _departmentService.GetAllDepartments()
                                .OrderBy(x => x.Name.ToUpper())
                                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                                .ToList();

            return View(vm);
        }

        //
        // POST: Admin/Account/GetWidgetPreview
        [HttpPost]
        public ActionResult GetWidgetPreview(Widget widget)
        {
            var serviceIds = new List<int?>();

            // If department is not specified then just add selected services
            if (widget.Department == null)
            {
                serviceIds = widget.Services.Select(x => x.Id).ToList();

                widget.Services = _serviceService.GetAllServices()
                                        .Where(x => serviceIds.Contains(x.Id) && x.Public)
                                        .OrderBy(x => x.Name.ToUpper())
                                        .ToList();
            }
            else
            {
                // If department is specified then add all services belonging to that department
                var department = _departmentService.GetDepartmentById(widget.Department.Id.Value);

                if (department != null)
                {
                    widget.Services = _serviceService.GetServicesByDepartmentCode(department.Code)
                                        .Where(x => x.Public)
                                        .OrderBy(x => x.Name.ToUpper())
                                        .ToList();
                }
            }

            var vm = new WidgetsIndexViewModel();

            vm.Widget = widget;
            vm.UpcomingMaintenance = _scheduledMaintenanceService.GetAllUpcomingMaintenances(7)
                                            .Where(x => widget.Services.Contains(x.Service) && x.Service.Public).Any();

            return View("_WidgetPreview", vm);
        }

        //
        // GET: Admin/Account/EditWidget/{id}

        public ActionResult EditWidget(int id)
        {
            var widget = _widgetService.GetWidgetById(id);

            if (widget == null)
                throw new HttpException(404, String.Format("No widget found with id = '{0}'", id));

            var owner = _userService.GetUserByUsername(((UserPrincipal)HttpContext.User).Username);

            // Throwing HttpException does not work for 401 because MVC is retarded
            if (widget.Owner.Id != owner.Id)
            {
                Response.StatusCode = 401;
                Response.StatusDescription = String.Format("You are unauthorized to make changes to widget with id: {0}", id);
                return null;
            }

            var vm = new AccountEditWidgetViewModel();

            // Get list of all services
            vm.Services = _serviceService.GetAllServices()
                                .OrderBy(x => x.Name.ToUpper())
                                .ToList();

            // Generate the widget code
            vm.WidgetCode = _widgetService.GenerateCode(widget);

            // If this widget has a department load all services from that department
            if (widget.Department != null)
            {
                vm.DepartmentId = widget.Department.Id;

                widget.Services = _serviceService.GetServicesByDepartmentCode(widget.Department.Code).ToList();
            }
            else
            {
                // Add service ids
                foreach (var service in widget.Services)
                {
                    vm.ServiceIds.Add(service.Id.Value);
                }
            }

            // Get list of all departments
            vm.Departments = _departmentService.GetAllDepartments()
                                .OrderBy(x => x.Name.ToUpper())
                                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                                .ToList();

            vm.Widget = widget;

            return View(vm);
        }

        //
        // POST: Admin/Account/EditWidget/{id}

        [HttpPost]
        public ActionResult EditWidget(int id, AccountEditWidgetViewModel vm)
        {
            var valid = true;
            var originalWidget = _widgetService.GetWidgetById(id);

            if (originalWidget == null)
                throw new HttpException(404, String.Format("No widget found with id = '{0}'", id));

            if (ModelState.IsValid)
            {
                if (vm.DepartmentId == null)
                {
                    vm.Widget.Services.Clear();
                    foreach (var serviceId in vm.ServiceIds)
                    {
                        vm.Widget.Services.Add(new Service
                        {
                            Id = serviceId
                        });
                    }
                }
                else
                {
                    var department = _departmentService.GetDepartmentById(vm.DepartmentId.Value);

                    // Make sure this department exists
                    if (department != null)
                    {
                        vm.Widget.Department = department;
                    }
                    else
                    {
                        ModelState.AddModelError("Widget.Department", "You've specified an invalid department. Please select another.");
                        valid = false;
                    }
                }

                if (valid)
                {
                    vm.Widget.Id = originalWidget.Id;
                    vm.Widget.Guid = originalWidget.Guid;
                    vm.Widget.Owner = originalWidget.Owner;

                    // Merge and save widget
                    _widgetService.MergeWidget(vm.Widget);
                    _widgetService.SaveWidget(vm.Widget);

                    vm.Widget = _widgetService.GetWidgetById(id);

                    ViewBag.Saved = true;
                }
            }
            else
            {
                vm.Widget = originalWidget;
            }

            vm.WidgetCode = _widgetService.GenerateCode(vm.Widget);

            vm.Services = _serviceService.GetAllServices()
                                .OrderBy(x => x.Name.ToUpper())
                                .ToList();

            vm.Departments = _departmentService.GetAllDepartments()
                                .OrderBy(x => x.Name.ToUpper())
                                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                                .ToList();

            return View(vm);
        }

        //
        // GET: Admin/Account/DeleteWidget/{id}

        public ActionResult DeleteWidget(int id, int? page)
        {
            var widget = _widgetService.GetWidgetById(id);

            if (widget == null)
                throw new HttpException(404, String.Format("No widget found with id = '{0}'", id));

            var owner = _userService.GetUserByUsername(((UserPrincipal)HttpContext.User).Username);

            // We do it this way because HttpException with 401 doesnt work correctly and returns a 500 internal error
            if (widget.Owner.Id != owner.Id)
            {
                Response.StatusCode = 401;
                Response.StatusDescription = String.Format("You are unauthorized to make changes to widget with id: {0}", id);
                return null;
            }

            _widgetService.DeleteWidget(widget);

            return RedirectToAction("MyWidgets", new { page = page });
        }
    }
}
