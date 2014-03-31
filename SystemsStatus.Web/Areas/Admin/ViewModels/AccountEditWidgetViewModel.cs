using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class AccountEditWidgetViewModel
    {
        public virtual IList<int> ServiceIds { get; set; }
        public virtual IList<Service> Services { get; set; }
        public virtual IList<SelectListItem> Departments { get; set; }
        public virtual int? DepartmentId { get; set; }
        public virtual Widget Widget { get; set; }

        [Display(Name = "Code")]
        public virtual string WidgetCode { get; set; }

        public AccountEditWidgetViewModel()
        {
            this.ServiceIds = new List<int>();
            this.Services = new List<Service>();
            this.Departments = new List<SelectListItem>();
        }
    }
}