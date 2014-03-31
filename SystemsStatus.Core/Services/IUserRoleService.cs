using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Services
{
    public interface IUserRoleService
    {
        /// <summary>
        /// Returns list of all user roles
        /// </summary>
        /// <returns>List of all user roles</returns>
        IQueryable<UserRole> GetAllUserRoles();

        /// <summary>
        /// Returns a user role by id
        /// </summary>
        /// <param name="id">Id of user role</param>
        /// <returns>User Role</returns>
        UserRole GetUserRoleById(int id);
    }
}
