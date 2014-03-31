using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Web.ViewModels;

namespace SystemsStatus.Web.Controllers
{
    public class WidgetsController : Controller
    {
        private readonly IWidgetService _widgetService;
        private readonly IServiceService _serviceService;
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;

        public WidgetsController(IWidgetService widgetService,
                                    IServiceService serviceService,
                                    IScheduledMaintenanceService scheduledMaintenanceService)
        {
            _widgetService = widgetService;
            _serviceService = serviceService;
            _scheduledMaintenanceService = scheduledMaintenanceService;
        }

        //
        // GET: /Widgets/

        public ActionResult Index(string guid)
        {
            var guidObject = new Guid(guid);

            var widget = _widgetService.GetWidgetByGuid(guidObject);

            if (widget == null)
                throw new HttpException(404, String.Format("No widget found with id = '{0}'", guid));

            var vm = new WidgetsIndexViewModel();

            if (widget.Department != null)
            {
                widget.Services = _serviceService.GetServicesByDepartmentCode(widget.Department.Code)
                                        .Where(x => x.Public).ToList();
            }
            else
            {
                widget.Services = widget.Services
                                        .Where(x => x.Public).ToList();
            }

            vm.Widget = widget;
            vm.UpcomingMaintenance = _scheduledMaintenanceService.GetAllUpcomingMaintenances(7)
                                            .Where(x => widget.Services.Contains(x.Service) && x.Service.Public).Any();

            return View(vm);
        }

    }
}
