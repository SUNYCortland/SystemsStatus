using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class ServicesChangeStatusViewModel
    {
        public virtual ICollection<Service> Services { get; set; }
        public virtual IList<int> ServiceIds { get; set; }
        public virtual OnlineServiceStatus OnlineServiceStatus { get; set; }
        public virtual OfflineUnplannedServiceStatus OfflineUnplannedServiceStatus { get; set; }
        public virtual OfflineMaintenanceServiceStatus OfflineMaintenanceServiceStatus { get; set; }
        public virtual OnlineDegradedServiceStatus OnlineDegradedServiceStatus { get; set; }
        public virtual string StatusType { get; set; }

        public ServicesChangeStatusViewModel()
        {
            this.Services = new HashSet<Service>();
            this.ServiceIds = new List<int>();
        }
    }
}