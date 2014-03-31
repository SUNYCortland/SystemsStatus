using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data;

namespace SystemsStatus.Core.Services.Impl
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IServiceRepository _serviceRepository;

        public DepartmentService(IDepartmentRepository departmentRepository, IServiceRepository serviceRepository)
        {
            _departmentRepository = departmentRepository;
            _serviceRepository = serviceRepository;
        }

        /// <summary>
        /// Returns all departments.
        /// </summary>
        /// <returns>IQueryable of all departments</returns>
        public IQueryable<Department> GetAllDepartments()
        {
            return _departmentRepository.All();
        }

        /// <summary>
        /// Returns a department
        /// </summary>
        /// <param name="id">Id of department to return</param>
        /// <returns>Department</returns>
        public Department GetDepartmentById(int id)
        {
            return _departmentRepository.FindBy(id);
        }

        /// <summary>
        /// Returns a department
        /// </summary>
        /// <param name="code">Code of department to return</param>
        /// <returns>Department</returns>
        public Department GetDepartmentByCode(string code)
        {
            return _departmentRepository.All().Where(x => x.Code.ToUpper() == code.ToUpper()).SingleOrDefault();
        }

        /// <summary>
        /// Inserts a department
        /// </summary>
        /// <param name="department">Department to insert</param>
        /// <returns>Department</returns>
        public Department InsertDepartment(Department department)
        {
            return _departmentRepository.Insert(department);
        }

        /// <summary>
        /// Deletes a department and removes service dependencies
        /// </summary>
        /// <param name="department">Department to delete</param>
        [UnitOfWork]
        public void DeleteDepartment(Department department)
        {
            var services = _serviceRepository.All().Where(x => x.Department.Id == department.Id);

            // Remove reference to this department for all services
            foreach (var service in services)
            {
                service.Department = null;

                _serviceRepository.SaveOrUpdate(service);
            }

            _departmentRepository.Delete(department);
        }

        /// <summary>
        /// Saves a department
        /// </summary>
        /// <param name="department">Department to save</param>
        /// <returns>Saved Department</returns>
        public Department SaveDepartment(Department department)
        {
            return _departmentRepository.SaveOrUpdate(department);
        }

        /// <summary>
        /// Merges a department with its entity
        /// </summary>
        /// <param name="department">Department to merge</param>
        /// <returns>Merged Department</returns>
        public Department MergeDepartment(Department department)
        {
            return _departmentRepository.Merge(department);
        }
    }
}
