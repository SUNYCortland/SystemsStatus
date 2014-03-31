using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Web.Areas.Admin.Attributes;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    [UserAuthorize]
    public class MaintenancesController : Controller
    {
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;

        public MaintenancesController(IScheduledMaintenanceService scheduledMaintenanceService)
        {
            _scheduledMaintenanceService = scheduledMaintenanceService;
        }

        //
        // GET: /Admin/Maintenances/

        public ActionResult Index()
        {
            var maintenances = _scheduledMaintenanceService.GetAllUpcomingMaintenances()
                .OrderBy(x => x.Offline)
                .ToList();

            return View(maintenances);
        }

        //
        // GET: /Admin/Maintenances/Edit/{id}

        public ActionResult Edit(int id)
        {
            var maintenance = _scheduledMaintenanceService.GetScheduledMaintenanceById(id);

            if (maintenance == null)
                throw new HttpException(404, String.Format("No maintenance found with id = '{0}'", id));

            return View(maintenance);
        }

        //
        // POST: /Admin/Maintenances/Edit/{id}

        [HttpPost]
        public ActionResult Edit(int id, ScheduledMaintenance maintenance)
        {
            var originalMaintenance = _scheduledMaintenanceService.GetScheduledMaintenanceById(id);

            if (originalMaintenance == null)
                throw new HttpException(404, String.Format("No maintenance found with id = '{0}'", id));

            if (ModelState.IsValid)
            {
                // Get values not posted by form
                maintenance.Id = id;
                maintenance.ScheduledBy = originalMaintenance.ScheduledBy;
                maintenance.Service = originalMaintenance.Service;

                // Merge maintenance entity
                maintenance = _scheduledMaintenanceService.MergeScheduledMaintenance(maintenance);

                // Save maintenance
                maintenance = _scheduledMaintenanceService.SaveScheduledMaintenance(maintenance);

                ViewBag.Saved = true;
            }

            return View(maintenance);
        }

        //
        // GET: /Admin/Maintenances/Delete/{id}

        public ActionResult Delete(int id)
        {
            var maintenance = _scheduledMaintenanceService.GetScheduledMaintenanceById(id);

            if (maintenance == null)
                throw new HttpException(404, String.Format("No maintenance found with id = '{0}'", id));

            _scheduledMaintenanceService.DeleteScheduledMaintenance(maintenance);

            return RedirectToAction("Index");
        }
    }
}
