using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class ServicesScheduleMaintenanceViewModel
    {
        public virtual ScheduledMaintenance ScheduledMaintenance { get; set; }
        public virtual IList<int> ServiceIds { get; set; }
        public virtual ICollection<Service> Services { get; set; }

        public ServicesScheduleMaintenanceViewModel()
        {
            this.ServiceIds = new List<int>();
            this.Services = new HashSet<Service>();
        }
    }
}