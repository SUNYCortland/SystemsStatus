using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Services
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Returns all departments.
        /// </summary>
        /// <returns>IQueryable of all departments</returns>
        IQueryable<Department> GetAllDepartments();

        /// <summary>
        /// Returns a department
        /// </summary>
        /// <param name="id">Id of department to return</param>
        /// <returns>Department</returns>
        Department GetDepartmentById(int id);

        /// <summary>
        /// Returns a department
        /// </summary>
        /// <param name="code">Code of department to return</param>
        /// <returns>Department</returns>
        Department GetDepartmentByCode(string code);

        /// <summary>
        /// Inserts a department
        /// </summary>
        /// <param name="department">Department to insert</param>
        /// <returns>Department</returns>
        Department InsertDepartment(Department department);

        /// <summary>
        /// Deletes a department and removes service dependencies
        /// </summary>
        /// <param name="department">Department to delete</param>
        void DeleteDepartment(Department department);

        /// <summary>
        /// Saves a department
        /// </summary>
        /// <param name="department">Department to save</param>
        /// <returns>Saved Department</returns>
        Department SaveDepartment(Department department);

        /// <summary>
        /// Merges a department with its entity
        /// </summary>
        /// <param name="department">Department to merge</param>
        /// <returns>Merged Department</returns>
        Department MergeDepartment(Department department);
    }
}
