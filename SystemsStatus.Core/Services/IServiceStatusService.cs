using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Services
{
    public interface IServiceStatusService
    {
        /// <summary>
        /// Returns a service status by id
        /// </summary>
        /// <param name="id">Id of service status</param>
        /// <returns>Service status</returns>
        ServiceStatus GetStatusById(int id);

        /// <summary>
        /// Returns all service statuses.
        /// </summary>
        /// <returns>List of all service statuses</returns>
        IQueryable<ServiceStatus> GetAllStatuses();

        /// <summary>
        /// Returns all service statuses of a project.
        /// </summary>
        /// <param name="service">Service of which to return statuses</param>
        /// <returns>List of all service statuses of a service</returns>
        IQueryable<ServiceStatus> GetAllServiceStatuses(Service service);

        /// <summary>
        /// Returns all service statuses of a project.
        /// </summary>
        /// <param name="id">Id of service of which to return statuses</param>
        /// <returns>List of all service statuses of a service</returns>
        IQueryable<ServiceStatus> GetAllServiceStatuses(int id);

        /// <summary>
        /// Inserts a service status
        /// </summary>
        /// <param name="status">Status to insert</param>
        /// <returns>Service status</returns>
        ServiceStatus InsertStatus(ServiceStatus status);

        /// <summary>
        /// Saves a service status
        /// </summary>
        /// <param name="status">Status to save</param>
        /// <returns>Service status</returns>
        ServiceStatus SaveStatus(ServiceStatus status);

        /// <summary>
        /// Merges a service status
        /// </summary>
        /// <param name="status">Status to merge</param>
        /// <returns>Service status</returns>
        ServiceStatus MergeStatus(ServiceStatus status);
    }
}
