using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Areas.Admin.Authentication
{
    public interface IUserPrincipal : IPrincipal
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Username { get; set; }
        UserRole Role { get; set; }
    }
}
