using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Core.Services;
using SystemsStatus.Web.Areas.Admin.Attributes;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    [UserAuthorize]
    public class HomeController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;

        public HomeController(IServiceService serviceService, IScheduledMaintenanceService scheduledMaintenanceService)
        {
            _serviceService = serviceService;
            _scheduledMaintenanceService = scheduledMaintenanceService;
        }

        //
        // GET: /Admin/Home/
        
        public ActionResult Index()
        {
            var vm = new HomeIndexViewModel();

            vm.Services = _serviceService.GetAllServices().OrderBy(x => x.Name.ToUpper());

            vm.Maintenances = _scheduledMaintenanceService.GetAllUpcomingMaintenances()
                            .OrderByDescending(x => x.Offline)
                            .Take(5);

            return View(vm);
        }

    }
}
