using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class ServicesEditViewModel
    {
        public virtual Service Service { get; set; }

        [Display(Name = "Owners")]
        public virtual ICollection<User> Users { get; set; }

        public virtual IList<int> OwnerIds { get; set; }

        public virtual IEnumerable<SelectListItem> Departments { get; set; }

        public virtual int? DepartmentId { get; set; }

        public ServicesEditViewModel()
        {
            this.Users = new HashSet<User>();
            this.OwnerIds = new List<int>();
            this.Departments = new List<SelectListItem>();
        }
    }
}