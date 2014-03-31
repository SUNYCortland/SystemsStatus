using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using System.Security.Principal;

namespace SystemsStatus.Web.Areas.Admin.Authentication
{
    public class UserPrincipal : IUserPrincipal
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }

        public IIdentity Identity { get; private set; }

        public UserPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public bool IsInRole(string role)
        {
            return Role.Name.ToLower() == role.ToLower();
        }

        public bool IsInRole(int level)
        {
            return Role.Level == level;
        }
    }
}