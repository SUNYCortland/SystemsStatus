using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class HomeIndexViewModel
    {
        public virtual IEnumerable<Service> Services { get; set; }
        public virtual IEnumerable<ScheduledMaintenance> Maintenances { get; set; }

        public HomeIndexViewModel()
        {
            this.Services = new List<Service>();
            this.Maintenances = new List<ScheduledMaintenance>();
        }
    }
}