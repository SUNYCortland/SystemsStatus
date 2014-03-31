using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;

namespace SystemsStatus.Core.Services.Impl
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// Returns list of all user roles
        /// </summary>
        /// <returns>List of all user roles</returns>
        public IQueryable<UserRole> GetAllUserRoles()
        {
            return _userRoleRepository.All();
        }

        /// <summary>
        /// Returns a user role by id
        /// </summary>
        /// <param name="id">Id of user role</param>
        /// <returns>User Role</returns>
        public UserRole GetUserRoleById(int id)
        {
            return _userRoleRepository.FindBy(x => x.Id == id);
        }
    }
}
