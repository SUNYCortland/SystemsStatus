using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class AccountChangePasswordViewModel
    {
        [Display(Name = "Current Password")]
        public virtual string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        public virtual string NewPassword { get; set; }

        [Display(Name = "Repeat Password")]
        public virtual string NewPasswordRepeat { get; set; }
    }
}