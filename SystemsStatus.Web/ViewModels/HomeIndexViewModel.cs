using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public virtual IEnumerable<ScheduledMaintenance> Maintenances { get; set; }
        public virtual IEnumerable<Service> Services { get; set; }

        public HomeIndexViewModel()
        {
            this.Maintenances = new List<ScheduledMaintenance>();
            this.Services = new List<Service>();
        }
    }
}