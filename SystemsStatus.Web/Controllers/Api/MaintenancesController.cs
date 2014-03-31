using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SystemsStatus.Core.Services;
using SystemsStatus.Web.ViewModels.Api;
using SystemsStatus.DTO.Api;
using AutoMapper;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Controllers.Api
{
    public class MaintenancesController : ApiController
    {
        private readonly IScheduledMaintenanceService _scheduledMaintenanceService;

        public MaintenancesController(IScheduledMaintenanceService scheduledMaintenanceService)
        {
            _scheduledMaintenanceService = scheduledMaintenanceService;
        }

        //
        // GET: /api/Maintenances/All.{ext}

        [HttpGet]
        public MaintenancesViewModel<ScheduledMaintenanceApiDTO> All()
        {
            var scheduledMaintenances = _scheduledMaintenanceService.GetAllUpcomingMaintenances()
                                            .OrderBy(x => x.Offline);

            var scheduledMaintenancesDTO = Mapper.Map<IEnumerable<ScheduledMaintenance>, MaintenancesViewModel<ScheduledMaintenanceApiDTO>>(scheduledMaintenances);

            return scheduledMaintenancesDTO;
        }
    }
}
