using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Services.Impl
{
    public class ServiceStatusService : IServiceStatusService
    {
        private readonly IServiceStatusRepository _serviceStatusRepository;

        public ServiceStatusService(IServiceStatusRepository serviceStatusRepository)
        {
            _serviceStatusRepository = serviceStatusRepository;
        }

        /// <summary>
        /// Inserts a service status
        /// </summary>
        /// <returns>Service status</returns>
        [UnitOfWork]
        public ServiceStatus InsertStatus(ServiceStatus status)
        {
            return _serviceStatusRepository.Insert(status);
        }

        /// <summary>
        /// Saves a service status
        /// </summary>
        /// <returns>Service status</returns>
        [UnitOfWork]
        public ServiceStatus SaveStatus(ServiceStatus status)
        {
            return _serviceStatusRepository.SaveOrUpdate(status);
        }

        /// <summary>
        /// Merges a service status
        /// </summary>
        /// <param name="status">Status to merge</param>
        /// <returns>Service status</returns>
        public ServiceStatus MergeStatus(ServiceStatus status)
        {
            return _serviceStatusRepository.Merge(status);
        }

        /// <summary>
        /// Returns all service statuses.
        /// </summary>
        /// <returns>List of all service statuses</returns>
        public IQueryable<ServiceStatus> GetAllStatuses()
        {
            return _serviceStatusRepository.All();
        }

        /// <summary>
        /// Returns all service statuses of a project.
        /// </summary>
        /// <param name="service">Service of which to return statuses</param>
        /// <returns>List of all service statuses of a service</returns>
        public IQueryable<ServiceStatus> GetAllServiceStatuses(Service service)
        {
            return _serviceStatusRepository.All().Where(x => x.Services.Where(y => y.Id == service.Id).Count() > 0);
        }

        /// <summary>
        /// Returns all service statuses of a project.
        /// </summary>
        /// <param name="id">Id of service of which to return statuses</param>
        /// <returns>List of all service statuses of a service</returns>
        public IQueryable<ServiceStatus> GetAllServiceStatuses(int id)
        {
            return _serviceStatusRepository.All().Where(x => x.Services.Where(y => y.Id == id).Count() > 0);
        }

        /// <summary>
        /// Returns a service status by id
        /// </summary>
        /// <returns>Service status</returns>
        public ServiceStatus GetStatusById(int id)
        {
            return _serviceStatusRepository.FindBy(x => x.Id == id);
        }
    }
}
