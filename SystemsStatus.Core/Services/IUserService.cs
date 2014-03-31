using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Checks if this username exists
        /// </summary>
        /// <param name="Username">Username</param>
        /// <param name="Active">Whether or not the specified user is active</param>
        /// <returns>True/False</returns>
        bool UserExists(string Username, bool Active);

        /// <summary>
        /// Checks if this User exists
        /// </summary>
        /// <param name="User">User</param>
        /// <param name="Active">Whether or not the specified user is active</param>
        /// <returns>True/False</returns>
        bool UserExists(User User, bool Active);

        /// <summary>
        /// Returns a user by username
        /// </summary>
        /// <param name="Username">Username</param>
        /// <returns>User</returns>
        User GetUserByUsername(string Username);

        /// <summary>
        /// Returns a user by id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>User</returns>
        User GetUserById(int Id);

        /// <summary>
        /// Returns a list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        IQueryable<User> GetAllUsers();

        /// <summary>
        /// Inserts User entity
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        User InsertUser(User user);

        /// <summary>
        /// Saves User entity
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        User SaveUser(User user);

        /// <summary>
        /// Merges User entity
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        User MergeUser(User user);

        /// <summary>
        /// Sets User entity property of Active to False.
        /// </summary>
        /// <param name="User">User</param>
        void DeleteUser(User user);
    }
}
