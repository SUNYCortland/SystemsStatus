using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Repositories;

namespace SystemsStatus.Core.Services.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Authenticates the Username/Password combo
        /// </summary>
        /// <param name="Username">Username</param>
        /// <param name="PasswordHash">Hashed password</param>
        /// <returns>True/False</returns>
        public bool Authenticate(string Username, string PasswordHash)
        {
            return _userRepository.FindBy(x => x.Username.ToUpper() == Username.ToUpper()
                && x.HashedPassword == PasswordHash
                && x.Active) != null;
        }
    }
}
