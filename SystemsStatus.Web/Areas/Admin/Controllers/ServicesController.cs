using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Web.Attributes;
using SystemsStatus.Web.Areas.Admin.Attributes;
using SystemsStatus.Web.Exceptions;
using SystemsStatus.Core.Data.Entities.Statuses;
using Castle.Core.Logging;
using SystemsStatus.Web.Areas.Admin.Authentication;
using SystemsStatus.Common.Pagination;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IUserService _userServices;
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;
        private readonly IServiceStatusService _serviceStatusService;
        private readonly IDepartmentService _departmentService;

        private const int DEFAULT_PAGE_SIZE = 10;

        public ILogger Logger { get; set; }

        public ServicesController(IServiceService serviceService, 
                                IUserService userService, 
                                IScheduledMaintenanceService scheduledMaintenanceService, 
                                IServiceStatusService serviceStatusService,
                                IDepartmentService departmentService)
        {
            _serviceService = serviceService;
            _userServices = userService;
            _scheduledMaintenanceService = scheduledMaintenanceService;
            _serviceStatusService = serviceStatusService;
            _departmentService = departmentService;
        }
        
        //
        // GET: /Admin/Services

        [UserAuthorize]
        public ActionResult Index()
        {
            return RedirectToAction("ServiceList");
        }

        //
        // GET: /Admin/Services/ServiceList

        [UserAuthorize]
        public ActionResult ServiceList(int? page, string display, string query)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1: 0;
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
                    || (x.Department != null && x.Department.Name.ToLower().Contains(query.ToLower()))
                    || (x.Tags.Any(y => y.ToUpper().Contains(query.ToUpper()))));
            }

            services = services.OrderBy(x => x.Name.ToUpper());

            var servicesPaged = services.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            // The specified page is too large
            if (currentPageIndex >= servicesPaged.PageCount)
                return View(services.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(servicesPaged);
        }

        //
        // GET: /Admin/Services/Hierarchy

        [UserAuthorize(Roles = "Administrator, Service Owner")]
        public ActionResult Hierarchy()
        {
            var services = _serviceService.GetAllParentServices().OrderBy(x => x.SortOrder).ToList();

            return View(services);
        }

        //
        // POST: /Admin/Services/Hierarchy

        [UserAuthorize(Roles = "Administrator, Service Owner")]
        [HttpPost]
        [HandleModelStateException]
        public ActionResult Hierarchy(ServicesHierarchyViewModel viewModel)
        {
            Service childService = _serviceService.GetServiceById(viewModel.Id);
            Service parentService = null;

            if (!ModelState.IsValid)
                throw new ModelStateException(ModelState);

            // Save hierarchy
            if(viewModel.ParentId != null)
                parentService = _serviceService.GetServiceById(viewModel.ParentId.Value);

            if (childService != null)
            {
                childService.Parent = parentService;

                _serviceService.SaveService(childService);
            }

            // Update sort order
            int sortOrder = 0;

            foreach (var sortOrderService in viewModel.SortOrder)
            {
                var service = _serviceService.GetServiceById(sortOrderService.id);

                if (service == null)
                {
                    ModelState.AddModelError("Service.Id", "Invalid request. If this problem persists please contact a systems administrator.");
                    throw new ModelStateException(ModelState);
                }

                service.SortOrder = sortOrder;

                _serviceService.SaveService(service);

                sortOrder++;

                foreach (var sortOrderChildService in sortOrderService.children)
                {
                    var child = _serviceService.GetServiceById(sortOrderChildService.id);

                    if (child == null)
                    {
                        ModelState.AddModelError("Service.Id", "Invalid request. If this problem persists please contact a systems administrator.");
                        throw new ModelStateException(ModelState);
                    }

                    child.SortOrder = sortOrder;

                    _serviceService.SaveService(child);

                    sortOrder++;
                }
            }

            return Json(new { success = true });
        }

        //
        // GET: /Admin/Services/Create

        [UserAuthorize(Roles = "Administrator, Service Owner" )]
        public ActionResult Create()
        {
            var vm = new ServicesCreateViewModel();

            vm.Services = _serviceService.GetAllParentServices()
                            .OrderBy(x => x.Name.ToUpper())
                            .Select(x => 
                                new SelectListItem
                                {
                                  Value = x.Id.Value.ToString(),
                                  Text = x.Name
                                })
                            .ToList();

            vm.Departments = _departmentService.GetAllDepartments()
                            .OrderBy(x => x.Name.ToUpper())
                            .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.Value.ToString(),
                                    Text = x.Name
                                })
                            .ToList();

            return View(vm);
        }

        //
        // POST: /Admin/Services/Create

        [UserAuthorize(Roles = "Administrator, Service Owner")]
        [HttpPost]
        public ActionResult Create(ServicesCreateViewModel vm)
        {
            vm.Services = _serviceService.GetAllParentServices()
                            .OrderBy(x => x.Name.ToUpper())
                            .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.Value.ToString(),
                                    Text = x.Name
                                })
                            .ToList();

            if (ModelState.IsValid)
            {
                var currentUser = _userServices.GetUserByUsername(HttpContext.User.Identity.Name);

                // Add the current user as the owner
                vm.Service.Owners.Add(currentUser);
                vm.Service.CreatedBy = currentUser;
                vm.Service.SortOrder = 999;

                if (vm.ParentServiceId.HasValue)
                    vm.Service.Parent = _serviceService.GetServiceById(vm.ParentServiceId.Value);

                if (vm.DepartmentId.HasValue)
                    vm.Service.Department = _departmentService.GetDepartmentById(vm.DepartmentId.Value);

                // Generate friendly URL
                if (String.IsNullOrWhiteSpace(vm.Service.Url))
                {
                    vm.Service.Url = SystemsStatus.Common.Helpers.UrlHelper.GenerateFriendlyUrl(vm.Service.Name);
                }
                else
                {
                    vm.Service.Url = SystemsStatus.Common.Helpers.UrlHelper.GenerateFriendlyUrl(vm.Service.Url);
                }

                // Make sure this URL isn't already taken
                if (_serviceService.GetServiceByUrl(vm.Service.Url) != null)
                {
                    ModelState.AddModelError("Service.Url", "This URL is already being used by another service. Please choose another.");
                    return View(vm);
                }

                // Should always be online
                if (vm.StatusType == "online")
                {
                    vm.OnlineServiceStatus.CreatedBy = currentUser;
                    vm.OnlineServiceStatus.CreatedDate = DateTime.Now;
                    vm.OnlineServiceStatus.Type = "online";

                    _serviceService.AddToCurrentStatus(vm.Service, vm.OnlineServiceStatus);
                }
                else
                {
                    ModelState.AddModelError("StatusType", "Invalid status type. All services must start out with an online status.");
                    return View(vm);
                }

                return RedirectToAction("ServiceList");
            }
            else
            {
                return View(vm);
            }
        }

        //
        // POST: /Admin/Services/GetServiceStatusForm

        [HttpPost]
        [UserAuthorize]
        [HandleModelStateException]
        public ActionResult GetServiceStatusForm(string ServiceStatusType)
        {
            var vm = new ServicesCreateViewModel();

            switch (ServiceStatusType)
            {
                case "online":
                    return View("_Create_OnlineServiceStatus", vm);
                    /*
                case "offline":
                    return View("_Create_OfflineUnplannedServiceStatus", vm);

                case "offlineMaintenance":
                    return View("_Create_OfflineMaintenanceServiceStatus", vm);*/

                default:
                    ModelState.AddModelError("serviceStatusType", "Invalid service status type.");
                    throw new ModelStateException(ModelState);
            }
        }

        //
        // POST: /Admin/Services/GetAddStatusForm

        [HttpPost]
        [UserAuthorize]
        [HandleModelStateException]
        public ActionResult GetAddStatusForm(string ServiceStatusType)
        {
            var vm = new ServicesChangeStatusViewModel();

            switch (ServiceStatusType)
            {
                case "online":
                    return View("_ChangeStatus_OnlineServiceStatus", vm);

                case "offline":
                    return View("_ChangeStatus_OfflineUnplannedServiceStatus", vm);

                case "offlineMaintenance":
                    return View("_ChangeStatus_OfflineMaintenanceServiceStatus", vm);

                case "onlineDegraded":
                    return View("_ChangeStatus_OnlineDegradedServiceStatus", vm);

                default:
                    ModelState.AddModelError("serviceStatusType", "Invalid service status type.");
                    throw new ModelStateException(ModelState);
            }
        }

        //
        // GET: /Admin/Services/Edit/{id}

        [UserAuthorize(Roles = "Administrator, Service Owner")]
        public ActionResult Edit(int id)
        {
            var service = _serviceService.GetServiceById(id);

            if (service == null)
                throw new HttpException(404, String.Format("No service found with id = '{0}'", id));

            if (!service.Owners.Any(x => x.Username == HttpContext.User.Identity.Name)
                && (User as UserPrincipal).IsInRole("Service Owner"))
                throw new HttpException(403, "You do not have permission to edit this service");

            var vm = new ServicesEditViewModel();
            vm.Service = service;

            var users = _userServices.GetAllUsers().OrderBy(x => x.LastName.ToUpper()).ToList();
            vm.Users = users;

            foreach (var owner in service.Owners)
            {
                vm.OwnerIds.Add(owner.Id.Value);
            }

            vm.Departments = _departmentService.GetAllDepartments()
                            .OrderBy(x => x.Name.ToUpper())
                            .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.Value.ToString(),
                                    Text = x.Name
                                })
                            .ToList();

            if (service.Department != null)
                vm.DepartmentId = service.Department.Id;

            return View(vm);
        }

        //
        // POST: /Admin/Services/Edit/{id}

        [UserAuthorize(Roles = "Administrator, Service Owner")]
        [HttpPost]
        public ActionResult Edit(int id, ServicesEditViewModel vm)
        {
            var originalService = _serviceService.GetServiceById(id);

            if (originalService == null)
                throw new HttpException(404, String.Format("No service found with id = '{0}'", id));

            if (!originalService.Owners.Any(x => x.Username == HttpContext.User.Identity.Name)
                && (User as UserPrincipal).IsInRole("Service Owner"))
                throw new HttpException(403, "You do not have permission to edit this service");

            var users = _userServices.GetAllUsers().OrderBy(x => x.LastName.ToUpper()).ToList();
            vm.Users = users;

            vm.Departments = _departmentService.GetAllDepartments()
                            .OrderBy(x => x.Name.ToUpper())
                            .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.Value.ToString(),
                                    Text = x.Name
                                })
                            .ToList();

            if (ModelState.IsValid)
            {
                // Generate friendly URL
                if (String.IsNullOrWhiteSpace(vm.Service.Url))
                {
                    vm.Service.Url = SystemsStatus.Common.Helpers.UrlHelper.GenerateFriendlyUrl(vm.Service.Name);
                }
                else
                {
                    vm.Service.Url = SystemsStatus.Common.Helpers.UrlHelper.GenerateFriendlyUrl(vm.Service.Url);
                }

                // Check for uniqueness of URL
                if (_serviceService.GetAllServices().Where(x => x.Url.ToLower() == vm.Service.Url.ToLower() && x.Id != id).Count() > 0)
                {
                    ModelState.AddModelError("Service.Url", "This URL is already being used by another service. Please choose another.");
                    return View(vm);
                }

                if (vm.DepartmentId.HasValue)
                    vm.Service.Department = _departmentService.GetDepartmentById(vm.DepartmentId.Value);

                // Bring parameters over that were not send by form
                vm.Service.Id = id;
                vm.Service.Maintenances = originalService.Maintenances;
                vm.Service.Parent = originalService.Parent;
                vm.Service.CurrentStatus = originalService.CurrentStatus;
                vm.Service.Statuses = originalService.Statuses;
                vm.Service.Children = originalService.Children;
                vm.Service.Dependents = originalService.Dependents;
                vm.Service.CreatedBy = _userServices.GetUserByUsername(HttpContext.User.Identity.Name);
                
                // Create the owners list
                vm.Service.Owners.Clear();
                foreach (int ownerId in vm.OwnerIds)
                {
                    vm.Service.Owners.Add(new User
                    {
                        Id = ownerId
                    });
                }

                // Merge the entity
                var serviceEntity = _serviceService.MergeService(vm.Service);

                // Save entity
                vm.Service = _serviceService.SaveService(serviceEntity);

                ViewBag.Saved = true;
            }

            return View(vm);
        }

        //
        // GET: /Admin/Services/ChangeStatus

        [UserAuthorize]
        public ActionResult ChangeStatus()
        {
            var vm = new ServicesChangeStatusViewModel();

            var services = _serviceService.GetAllServices()
                .OrderBy(x => x.Name.ToUpper())
                .ToList();

            vm.Services = services;

            return View(vm);
        }

        //
        // POST: /Admin/Services/ChangeStatus

        [UserAuthorize]
        [HttpPost]
        public ActionResult ChangeStatus(ServicesChangeStatusViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var status = new ServiceStatus();
                var user = _userServices.GetUserByUsername(HttpContext.User.Identity.Name);

                if (vm.StatusType == "offlineMaintenance")
                {
                    // Add current user
                    vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance.ScheduledBy = user;
                    vm.OfflineMaintenanceServiceStatus.CreatedBy = user;
                    vm.OfflineMaintenanceServiceStatus.CreatedDate = DateTime.Now;
                    vm.OfflineMaintenanceServiceStatus.Type = "maintenance";

                    // Save the scheduled maintenance
                    var maintenance = _scheduledMaintenanceService.InsertScheduledMaintenance(vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance);

                    // Add the scheduled maintenance to the service status
                    vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance = maintenance;

                    // Save the status
                    status = (OfflineMaintenanceServiceStatus)_serviceStatusService.SaveStatus(vm.OfflineMaintenanceServiceStatus);
                }
                else if (vm.StatusType == "offline")
                {
                    vm.OfflineUnplannedServiceStatus.CreatedBy = user;
                    vm.OfflineUnplannedServiceStatus.CreatedDate = DateTime.Now;
                    vm.OfflineUnplannedServiceStatus.Type = "down";

                    // Save the status
                    status = (OfflineUnplannedServiceStatus)_serviceStatusService.SaveStatus(vm.OfflineUnplannedServiceStatus);
                }
                else if (vm.StatusType == "online")
                {
                    vm.OnlineServiceStatus.CreatedBy = user;
                    vm.OnlineServiceStatus.CreatedDate = DateTime.Now;
                    vm.OnlineServiceStatus.Type = "online";

                    // Save the status
                    status = (OnlineServiceStatus)_serviceStatusService.SaveStatus(vm.OnlineServiceStatus);
                }
                else if (vm.StatusType == "onlineDegraded")
                {
                    vm.OnlineDegradedServiceStatus.CreatedBy = user;
                    vm.OnlineDegradedServiceStatus.CreatedDate = DateTime.Now;
                    vm.OnlineDegradedServiceStatus.Type = "degraded";

                    // Save the status
                    status = (OnlineDegradedServiceStatus)_serviceStatusService.SaveStatus(vm.OnlineDegradedServiceStatus);
                }

                // Add status to all selected services and save the service
                foreach (int serviceId in vm.ServiceIds)
                {
                    var service = _serviceService.GetServiceById(serviceId);

                    // Update actual online time when going from offlineUnplanned -> online
                    if (service.CurrentStatus.GetType() == typeof(OfflineUnplannedServiceStatus)
                        && vm.StatusType == "online")
                    {
                        var currentStatus = (OfflineUnplannedServiceStatus)service.CurrentStatus;

                        currentStatus.ActualOnline = vm.OnlineServiceStatus.OnlineSince;

                        _serviceStatusService.SaveStatus(currentStatus);
                    }

                    // Update actual online time when going from offlineUnplanned -> offlineMaintenance
                    if (service.CurrentStatus.GetType() == typeof(OfflineUnplannedServiceStatus)
                        && vm.StatusType == "offlineMaintenance")
                    {
                        var currentStatus = (OfflineUnplannedServiceStatus)service.CurrentStatus;

                        currentStatus.ActualOnline = vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance.Offline;

                        _serviceStatusService.SaveStatus(currentStatus);
                    }

                    // Update actual online time when going from offlineUnplanned -> onlineDegraded
                    if (service.CurrentStatus.GetType() == typeof(OfflineUnplannedServiceStatus)
                        && vm.StatusType == "onlineDegraded")
                    {
                        var currentStatus = (OfflineUnplannedServiceStatus)service.CurrentStatus;

                        currentStatus.ActualOnline = vm.OnlineDegradedServiceStatus.DegradedSince;

                        _serviceStatusService.SaveStatus(currentStatus);
                    }

                    // Update actual online time when going from offlineMaintenance -> online
                    if (service.CurrentStatus.GetType() == typeof(OfflineMaintenanceServiceStatus)
                        && vm.StatusType == "online")
                    {
                        var currentStatus = (OfflineMaintenanceServiceStatus)service.CurrentStatus;

                        currentStatus.ScheduledMaintenance.Online = vm.OnlineServiceStatus.OnlineSince;

                        _scheduledMaintenanceService.SaveScheduledMaintenance(currentStatus.ScheduledMaintenance);
                    }

                    // Update actual online time when going from offlineMaintenance -> offlineUnplanned
                    if (service.CurrentStatus.GetType() == typeof(OfflineMaintenanceServiceStatus)
                        && vm.StatusType == "offline")
                    {
                        var currentStatus = (OfflineMaintenanceServiceStatus)service.CurrentStatus;

                        currentStatus.ScheduledMaintenance.Online = vm.OfflineUnplannedServiceStatus.OfflineSince;

                        _scheduledMaintenanceService.SaveScheduledMaintenance(currentStatus.ScheduledMaintenance);
                    }

                    // Update actual online time when going from offlineMaintenance -> onlineDegraded
                    if (service.CurrentStatus.GetType() == typeof(OfflineMaintenanceServiceStatus)
                        && vm.StatusType == "onlineDegraded")
                    {
                        var currentStatus = (OfflineMaintenanceServiceStatus)service.CurrentStatus;

                        currentStatus.ScheduledMaintenance.Online = vm.OnlineDegradedServiceStatus.DegradedSince;

                        _scheduledMaintenanceService.SaveScheduledMaintenance(currentStatus.ScheduledMaintenance);
                    }

                    // Update actual online time when going from onlineDegraded -> online
                    if (service.CurrentStatus.GetType() == typeof(OnlineDegradedServiceStatus)
                        && vm.StatusType == "online")
                    {
                        var currentStatus = (OnlineDegradedServiceStatus)service.CurrentStatus;

                        currentStatus.ActualOnline = vm.OnlineServiceStatus.OnlineSince;

                        _serviceStatusService.SaveStatus(currentStatus);
                    }

                    // Update actual online time when going from onlineDegraded -> offlineMaintenance
                    if (service.CurrentStatus.GetType() == typeof(OnlineDegradedServiceStatus)
                        && vm.StatusType == "offlineMaintenance")
                    {
                        var currentStatus = (OnlineDegradedServiceStatus)service.CurrentStatus;

                        currentStatus.ActualOnline = vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance.Offline;

                        _serviceStatusService.SaveStatus(currentStatus);
                    }

                    // Update actual online time when going from onlineDegraded -> offlineUnplanned
                    if (service.CurrentStatus.GetType() == typeof(OnlineDegradedServiceStatus)
                        && vm.StatusType == "offline")
                    {
                        var currentStatus = (OnlineDegradedServiceStatus)service.CurrentStatus;

                        currentStatus.ActualOnline = vm.OfflineUnplannedServiceStatus.OfflineSince;

                        _serviceStatusService.SaveStatus(currentStatus);
                    }

                    service.CurrentStatus = status;
                    service.Statuses.Add(status);

                    _serviceService.SaveService(service);
                }

                return RedirectToAction("ServiceList");
            }

            var services = _serviceService.GetAllServices()
                .OrderBy(x => x.Name.ToUpper())
                .ToList();

            vm.Services = services;

            return View(vm);
        }

        //
        // GET: /Admin/Services/ScheduleMaintenance

        [UserAuthorize]
        public ActionResult ScheduleMaintenance()
        {
            var vm = new ServicesScheduleMaintenanceViewModel();

            vm.Services = _serviceService.GetAllServices().OrderBy(x => x.Name.ToUpper()).ToList();

            return View(vm);
        }

        //
        // POST: /Admin/Services/ScheduleMaintenance

        [UserAuthorize]
        [HttpPost]
        public ActionResult ScheduleMaintenance(ServicesScheduleMaintenanceViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = _userServices.GetUserByUsername(HttpContext.User.Identity.Name);

                foreach (int serviceId in vm.ServiceIds)
                {
                    var scheduledMaintenance = new ScheduledMaintenance();

                    scheduledMaintenance.Description = vm.ScheduledMaintenance.Description;
                    scheduledMaintenance.ExpectedOnline = vm.ScheduledMaintenance.ExpectedOnline;
                    scheduledMaintenance.Name = vm.ScheduledMaintenance.Name;
                    scheduledMaintenance.Offline = vm.ScheduledMaintenance.Offline;
                    scheduledMaintenance.Online = vm.ScheduledMaintenance.Online;
                    scheduledMaintenance.ScheduledBy = user;
                    scheduledMaintenance.Service = new Service
                    {
                        Id = serviceId
                    };

                    // Save scheduled maintenance
                    _scheduledMaintenanceService.InsertScheduledMaintenance(scheduledMaintenance);
                }

                return RedirectToAction("Index", new { controller = "Maintenances" });
            }

            vm.Services = _serviceService.GetAllServices().OrderBy(x => x.Name.ToUpper()).ToList();

            return View(vm);
        }

        //
        // GET: /Admin/Services/Delete/{id}

        [UserAuthorize(Roles = "Administrator, Service Owner")]
        public ActionResult Delete(int id, int? page, string display, string from)
        {
            var service = _serviceService.GetServiceById(id);

            if(service == null)
                throw new HttpException(404, String.Format("No service found with id = '{0}'", id));

            _serviceService.DeleteService(service);

            if (String.IsNullOrWhiteSpace(from))
                return RedirectToAction("ServiceList", new { page = page, display = display });
            else
                return RedirectToAction("MyServices", new { controller = "Account", page = page, display = display });
        }

        //
        // GET: /Admin/Services/GetServicesSelectList

        [UserAuthorize]
        [HandleModelStateException]
        public ActionResult GetServicesSelectList(string query, string filter)
        {
            var services = _serviceService.GetAllServices();

            if (!String.IsNullOrEmpty(filter))
            {
                switch (filter.ToLower())
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
                services = services.Where(x => x.Name.ToLower().Contains(query.ToLower()));
            }

            services = services.OrderBy(x => x.Name.ToUpper());

            return Json(services.Select(x => new { value = x.Id, text = x.Name }), JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Admin/Services/GetChangeStatusForm

        [UserAuthorize]
        [HandleModelStateException]
        [HttpPost]
        public ActionResult GetChangeStatusForm(int id)
        {
            var service = _serviceService.GetServiceById(id);

            if (service == null)
            {
                ModelState.AddModelError("Service.Id", "Invalid request. If this problem persists please contact a systems administrator.");
                throw new ModelStateException(ModelState);
            }

            var vm = new ServiceListChangeStatusViewModel();
            vm.Service = service;

            return PartialView("_ServiceList_ChangeStatusForm", vm);
        }

        //
        // POST: /Admin/Services/ChangeServiceStatus

        [UserAuthorize]
        [HandleModelStateException]
        [HttpPost]
        public ActionResult ChangeServiceStatus(int id, ServiceListChangeStatusViewModel vm)
        {
            var service = _serviceService.GetServiceById(id);

            if (service == null)
            {
                ModelState.AddModelError("Service.Id", "Invalid request. If this problem persists please contact a systems administrator.");
                throw new ModelStateException(ModelState);
            }

            if (ModelState.IsValid)
            {
                var status = new ServiceStatus();
                var user = _userServices.GetUserByUsername(HttpContext.User.Identity.Name);

                if (vm.StatusType == "offlineMaintenance")
                {
                    // Add current user
                    vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance.ScheduledBy = user;
                    vm.OfflineMaintenanceServiceStatus.CreatedBy = user;
                    vm.OfflineMaintenanceServiceStatus.CreatedDate = DateTime.Now;
                    vm.OfflineMaintenanceServiceStatus.Type = "maintenance";

                    // Save the scheduled maintenance
                    var maintenance = _scheduledMaintenanceService.InsertScheduledMaintenance(vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance);

                    // Add the scheduled maintenance to the service status
                    vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance = maintenance;

                    // Save the status
                    status = (OfflineMaintenanceServiceStatus)_serviceStatusService.SaveStatus(vm.OfflineMaintenanceServiceStatus);
                }
                else if (vm.StatusType == "offline")
                {
                    vm.OfflineUnplannedServiceStatus.CreatedBy = user;
                    vm.OfflineUnplannedServiceStatus.CreatedDate = DateTime.Now;
                    vm.OfflineUnplannedServiceStatus.Type = "down";

                    // Save the status
                    status = (OfflineUnplannedServiceStatus)_serviceStatusService.SaveStatus(vm.OfflineUnplannedServiceStatus);
                }
                else if (vm.StatusType == "online")
                {
                    vm.OnlineServiceStatus.CreatedBy = user;
                    vm.OnlineServiceStatus.CreatedDate = DateTime.Now;
                    vm.OnlineServiceStatus.Type = "online";

                    // Save the status
                    status = (OnlineServiceStatus)_serviceStatusService.SaveStatus(vm.OnlineServiceStatus);
                }
                else if (vm.StatusType == "onlineDegraded")
                {
                    vm.OnlineDegradedServiceStatus.CreatedBy = user;
                    vm.OnlineDegradedServiceStatus.CreatedDate = DateTime.Now;
                    vm.OnlineDegradedServiceStatus.Type = "degraded";

                    // Save the status
                    status = (OnlineDegradedServiceStatus)_serviceStatusService.SaveStatus(vm.OnlineDegradedServiceStatus);
                }

                // Update actual online time when going from offlineUnplanned -> online
                if (service.CurrentStatus.GetType() == typeof(OfflineUnplannedServiceStatus)
                    && vm.StatusType == "online")
                {
                    var currentStatus = (OfflineUnplannedServiceStatus)service.CurrentStatus;

                    currentStatus.ActualOnline = vm.OnlineServiceStatus.OnlineSince;

                    _serviceStatusService.SaveStatus(currentStatus);
                }

                // Update actual online time when going from offlineUnplanned -> offlineMaintenance
                if (service.CurrentStatus.GetType() == typeof(OfflineUnplannedServiceStatus)
                    && vm.StatusType == "offlineMaintenance")
                {
                    var currentStatus = (OfflineUnplannedServiceStatus)service.CurrentStatus;

                    currentStatus.ActualOnline = vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance.Offline;

                    _serviceStatusService.SaveStatus(currentStatus);
                }

                // Update actual online time when going from offlineUnplanned -> onlineDegraded
                if (service.CurrentStatus.GetType() == typeof(OfflineUnplannedServiceStatus)
                    && vm.StatusType == "onlineDegraded")
                {
                    var currentStatus = (OfflineUnplannedServiceStatus)service.CurrentStatus;

                    currentStatus.ActualOnline = vm.OnlineDegradedServiceStatus.DegradedSince;

                    _serviceStatusService.SaveStatus(currentStatus);
                }

                // Update actual online time when going from offlineMaintenance -> online
                if (service.CurrentStatus.GetType() == typeof(OfflineMaintenanceServiceStatus)
                    && vm.StatusType == "online")
                {
                    var currentStatus = (OfflineMaintenanceServiceStatus)service.CurrentStatus;

                    currentStatus.ScheduledMaintenance.Online = vm.OnlineServiceStatus.OnlineSince;

                    _scheduledMaintenanceService.SaveScheduledMaintenance(currentStatus.ScheduledMaintenance);
                }

                // Update actual online time when going from offlineMaintenance -> offlineUnplanned
                if (service.CurrentStatus.GetType() == typeof(OfflineMaintenanceServiceStatus)
                    && vm.StatusType == "offline")
                {
                    var currentStatus = (OfflineMaintenanceServiceStatus)service.CurrentStatus;

                    currentStatus.ScheduledMaintenance.Online = vm.OfflineUnplannedServiceStatus.OfflineSince;

                    _scheduledMaintenanceService.SaveScheduledMaintenance(currentStatus.ScheduledMaintenance);
                }

                // Update actual online time when going from offlineMaintenance -> onlineDegraded
                if (service.CurrentStatus.GetType() == typeof(OfflineMaintenanceServiceStatus)
                    && vm.StatusType == "onlineDegraded")
                {
                    var currentStatus = (OfflineMaintenanceServiceStatus)service.CurrentStatus;

                    currentStatus.ScheduledMaintenance.Online = vm.OnlineDegradedServiceStatus.DegradedSince;

                    _scheduledMaintenanceService.SaveScheduledMaintenance(currentStatus.ScheduledMaintenance);
                }

                // Update actual online time when going from onlineDegraded -> online
                if (service.CurrentStatus.GetType() == typeof(OnlineDegradedServiceStatus)
                    && vm.StatusType == "online")
                {
                    var currentStatus = (OnlineDegradedServiceStatus)service.CurrentStatus;

                    currentStatus.ActualOnline = vm.OnlineServiceStatus.OnlineSince;

                    _serviceStatusService.SaveStatus(currentStatus);
                }

                // Update actual online time when going from onlineDegraded -> offlineMaintenance
                if (service.CurrentStatus.GetType() == typeof(OnlineDegradedServiceStatus)
                    && vm.StatusType == "offlineMaintenance")
                {
                    var currentStatus = (OnlineDegradedServiceStatus)service.CurrentStatus;

                    currentStatus.ActualOnline = vm.OfflineMaintenanceServiceStatus.ScheduledMaintenance.Offline;

                    _serviceStatusService.SaveStatus(currentStatus);
                }

                // Update actual online time when going from onlineDegraded -> offlineUnplanned
                if (service.CurrentStatus.GetType() == typeof(OnlineDegradedServiceStatus)
                    && vm.StatusType == "offline")
                {
                    var currentStatus = (OnlineDegradedServiceStatus)service.CurrentStatus;

                    currentStatus.ActualOnline = vm.OfflineUnplannedServiceStatus.OfflineSince;

                    _serviceStatusService.SaveStatus(currentStatus);
                }

                service.CurrentStatus = status;
                service.Statuses.Add(status);

                _serviceService.SaveService(service);

                ViewBag.Saved = true;

                return Json(new { saved = true });
            }

            vm.Service = service;

            return PartialView("_ServiceList_ChangeStatusForm", vm);
        }
    }
}
