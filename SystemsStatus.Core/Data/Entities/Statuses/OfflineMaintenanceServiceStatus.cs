using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Data.Entities.Statuses
{
    public class OfflineMaintenanceServiceStatus : ServiceStatus
    {
        public virtual ScheduledMaintenance ScheduledMaintenance { get; set; }
    }
}
