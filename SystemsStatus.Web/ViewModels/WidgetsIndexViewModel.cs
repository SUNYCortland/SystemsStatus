using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.ViewModels
{
    public class WidgetsIndexViewModel
    {
        public virtual Widget Widget { get; set; }
        public virtual bool UpcomingMaintenance { get; set; }
    }
}