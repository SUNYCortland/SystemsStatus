using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}