using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Entities.Statuses;
using System.Web.Mvc;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class ServicesCreateViewModel
    {
        public virtual Service Service { get; set; }
        public virtual OnlineServiceStatus OnlineServiceStatus { get; set; }
        public virtual IEnumerable<SelectListItem> Services { get; set; }
        public virtual IEnumerable<SelectListItem> Departments { get; set; }
        public virtual string StatusType { get; set; }
        public virtual int? ParentServiceId { get; set; }
        public virtual int? DepartmentId { get; set; }

        public ServicesCreateViewModel()
        {
            this.Services = new List<SelectListItem>();
            this.Departments = new List<SelectListItem>();
        }
    }
}