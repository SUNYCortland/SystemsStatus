using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class UsersCreateViewModel
    {
        public virtual User User { get; set; }

        public virtual string Password { get; set; }

        [Display(Name = "Repeat Password")]
        public virtual string PasswordRepeat { get; set; }

        public virtual IEnumerable<UserRole> Roles { get; set; }

        public UsersCreateViewModel()
        {
            this.Roles = new List<UserRole>();
        }
    }
}