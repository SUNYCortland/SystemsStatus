using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class UsersEditViewModel
    {
        public virtual User User { get; set; }

        [Display(Name = "Services Owned")]
        public virtual IList<int> ServiceIds { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual IEnumerable<UserRole> Roles { get; set; }

        public UsersEditViewModel()
        {
            this.ServiceIds = new List<int>();
            this.Services = new HashSet<Service>();
            this.Roles = new List<UserRole>();
        }
    }
}