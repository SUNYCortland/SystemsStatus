using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using SystemsStatus.Core.Services;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Entities.Statuses;
using FluentValidation;
using SystemsStatus.Web.ViewModels;

namespace SystemsStatus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;

        public ILogger Logger { get; set; }

        public HomeController(IServiceService serviceService, IScheduledMaintenanceService scheduledMaintenanceService)
        {
            _serviceService = serviceService;
            _scheduledMaintenanceService = scheduledMaintenanceService;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var services = _serviceService.GetAllPublicParentServices()
                        .OrderBy(x => x.SortOrder)
                        .ToList();

            var maintenances = _scheduledMaintenanceService.GetAllUpcomingMaintenances()
                        .OrderBy(x => x.Offline)
                        .Take(3)
                        .ToList();

            var vm = new HomeIndexViewModel();
            vm.Services = services;
            vm.Maintenances = maintenances;

            return View(vm);
        }
    }
}
