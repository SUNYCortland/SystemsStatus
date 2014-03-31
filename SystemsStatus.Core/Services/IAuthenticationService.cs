using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates the Username/Password combo
        /// </summary>
        /// <param name="Username">Username</param>
        /// <param name="PasswordHash">Hashed password</param>
        /// <returns>True/False</returns>
        bool Authenticate(string Username, string PasswordHash);
    }
}
