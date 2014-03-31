using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Common.Pagination;
using Castle.Core.Logging;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Controllers
{
    public class MaintenancesController : Controller
    {
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;

        private const int DEFAULT_PAGE_SIZE = 25;

        public ILogger Logger { get; set; }

        public MaintenancesController(IScheduledMaintenanceService scheduledMaintenanceService)
        {
            _scheduledMaintenanceService = scheduledMaintenanceService;
        }

        //
        // GET: /Maintenances/

        public ActionResult Index(int? page)
        {
            try
            {
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

                var scheduledMaintenances = _scheduledMaintenanceService.GetAllUpcomingMaintenances()
                                            .OrderBy(x => x.Offline);

                var scheduledMaintenancesPaged = scheduledMaintenances.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

                // The specified page is too large
                if (currentPageIndex >= scheduledMaintenancesPaged.PageCount)
                    return View(scheduledMaintenances.ToPagedList(0, DEFAULT_PAGE_SIZE));

                return View(scheduledMaintenancesPaged);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
            }

            return RedirectToAction("InternalServerError", new { controller = "Errors" });
        }

    }
}
