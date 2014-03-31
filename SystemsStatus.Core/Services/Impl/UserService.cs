using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data;

namespace SystemsStatus.Core.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Checks if this username exists
        /// </summary>
        /// <param name="Username">Username</param>
        /// <param name="Active">Whether or not the specified user is active</param>
        /// <returns>True/False</returns>
        public bool UserExists(string Username, bool Active)
        {
            if (Active)
                return _userRepository.FindBy(x => x.Username.ToUpper() == Username.ToUpper() && x.Active) != null;

            return _userRepository.FindBy(x => x.Username.ToUpper() == Username.ToUpper()) != null;
        }

        /// <summary>
        /// Checks if this User exists
        /// </summary>
        /// <param name="User">User</param>
        /// <param name="Active">Whether or not the specified user is active</param>
        /// <returns>True/False</returns>
        public bool UserExists(User User, bool Active)
        {
            if (Active)
                return _userRepository.FindBy(x => x.Username.ToUpper() == User.Username.ToUpper() && x.Active) != null;

            return _userRepository.FindBy(x => x.Username.ToUpper() == User.Username.ToUpper()) != null;
        }

        /// <summary>
        /// Returns a user by username
        /// </summary>
        /// <param name="Username">Username</param>
        /// <returns>User</returns>
        public User GetUserByUsername(string Username)
        {
            return _userRepository.FindBy(x => x.Username.ToUpper() == Username.ToUpper());
        }

        /// <summary>
        /// Returns a user by id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>User</returns>
        public User GetUserById(int Id)
        {
            return _userRepository.FindBy(x => x.Id == Id);
        }

        /// <summary>
        /// Returns a list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        public IQueryable<User> GetAllUsers()
        {
            return _userRepository.All();
        }

        /// <summary>
        /// Inserts User entity
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        [UnitOfWork]
        public User InsertUser(User user)
        {
            return _userRepository.Insert(user);
        }

        /// <summary>
        /// Saves User entity
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        [UnitOfWork]
        public User SaveUser(User user)
        {
            return _userRepository.SaveOrUpdate(user);
        }

        /// <summary>
        /// Merges User entity
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        [UnitOfWork]
        public User MergeUser(User user)
        {
            return _userRepository.Merge(user);
        }

        /// <summary>
        /// Sets User entity property of Active to False.
        /// </summary>
        /// <param name="User">User</param>
        [UnitOfWork]
        public void DeleteUser(User user)
        {
            user.Active = false;

            _userRepository.SaveOrUpdate(user);
        }
    }
}
