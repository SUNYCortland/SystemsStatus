using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data;

namespace SystemsStatus.Dependency.UoW
{
    /// <summary>
    /// This static class will help determine when we need to intercept a method
    /// and create a transaction to the database
    /// </summary>
    public static class UnitOfWorkHelper
    {
        /// <summary>
        /// Checks if a method is a repository method
        /// </summary>
        /// <param name="methodInfo">MethodInfo of the method</param>
        /// <returns>True/False</returns>
        public static bool IsRepositoryMethod(MethodInfo methodInfo)
        {
            return IsRepositoryClass(methodInfo.DeclaringType);
        }

        /// <summary>
        /// Checks if a class is a repository class
        /// </summary>
        /// <param name="type">Type of the class</param>
        /// <returns>True/False</returns>
        public static bool IsRepositoryClass(Type type)
        {
            return typeof(IRepository).IsAssignableFrom(type);
        }

        /// <summary>
        /// Checks if a method is marked with the UnitOfWork attribute
        /// </summary>
        /// <param name="methodInfo">MethodInfo of the method</param>
        /// <returns>True/False</returns>
        public static bool HasUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }
    }
}
