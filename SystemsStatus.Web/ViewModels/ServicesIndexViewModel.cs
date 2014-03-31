using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Web.ViewModels
{
    public class ServicesIndexViewModel
    {
        public virtual Service Service { get; set; }
        public virtual IQueryable<ServiceStatus> Statuses { get; set; }
    }
}