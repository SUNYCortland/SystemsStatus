using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Areas.Admin.Authentication
{
    public class UserPrincipalPoco
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}