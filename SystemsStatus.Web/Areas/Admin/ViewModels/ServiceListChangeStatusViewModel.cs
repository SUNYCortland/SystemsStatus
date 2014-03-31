using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities.Statuses;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class ServiceListChangeStatusViewModel
    {
        public virtual Service Service { get; set; }
        public virtual OnlineServiceStatus OnlineServiceStatus { get; set; }
        public virtual OfflineUnplannedServiceStatus OfflineUnplannedServiceStatus { get; set; }
        public virtual OfflineMaintenanceServiceStatus OfflineMaintenanceServiceStatus { get; set; }
        public virtual OnlineDegradedServiceStatus OnlineDegradedServiceStatus { get; set; }
        public virtual string StatusType { get; set; }
    }
}