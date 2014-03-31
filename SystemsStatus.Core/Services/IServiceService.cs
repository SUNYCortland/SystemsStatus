using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Core.Services
{
    public interface IServiceService
    {
        /// <summary>
        /// Returns all services.
        /// </summary>
        /// <returns>List of all services</returns>
        IQueryable<Service> GetAllServices();

        /// <summary>
        /// Returns all public services.
        /// </summary>
        /// <returns>List of all public services</returns>
        IQueryable<Service> GetAllPublicServices();

        /// <summary>
        /// Returns all private services.
        /// </summary>
        /// <returns>List of all private services</returns>
        IQueryable<Service> GetAllPrivateServices();

        /// <summary>
        /// Returns all parent services.
        /// </summary>
        /// <returns>List of all parent services</returns>
        IQueryable<Service> GetAllParentServices();

        /// <summary>
        /// Returns all public parent services.
        /// </summary>
        /// <returns>List of all public parent services</returns>
        IQueryable<Service> GetAllPublicParentServices();

        /// <summary>
        /// Returns all private parent services.
        /// </summary>
        /// <returns>List of all private parent services</returns>
        IQueryable<Service> GetAllPrivateParentServices();

        /// <summary>
        /// Returns all services with a current status of the passed type
        /// </summary>
        /// <param name="type">Type of current status</param>
        /// <returns>List of all services with a current status of the passed type</returns>
        IQueryable<Service> GetAllServicesWithCurrentStatusType(Type type);

        /// <summary>
        /// Returns services by department code
        /// </summary>
        /// <param name="code">Department code of service to return</param>
        /// <returns>List of all services that belong to the specified department code</returns>
        IQueryable<Service> GetServicesByDepartmentCode(string code);

        /// <summary>
        /// Returns a service by url
        /// </summary>
        /// <param name="url">Url of service to return</param>
        /// <returns>Service</returns>
        Service GetServiceByUrl(string url);

        /// <summary>
        /// Returns a service
        /// </summary>
        /// <param name="id">Id of service to return</param>
        /// <returns>Service</returns>
        Service GetServiceById(int id);

        /// <summary>
        /// Inserts a service
        /// </summary>
        /// <param name="service">Service to insert</param>
        /// <returns>Service</returns>
        Service InsertService(Service service);

        /// <summary>
        /// Deletes a service and all dependencies
        /// </summary>
        /// <param name="service">Service to delete</param>
        void DeleteService(Service service);

        /// <summary>
        /// Saves a service
        /// </summary>
        /// <param name="service">Service to save</param>
        /// <returns>Saved Service</returns>
        Service SaveService(Service service);

        /// <summary>
        /// Merges a service with its entity
        /// </summary>
        /// <param name="service">Service to merge</param>
        /// <returns>Merged Service</returns>
        Service MergeService(Service service);

        /// <summary>
        /// Adds a new status as the current service status and saves the service.
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        /// <returns>Service</returns>
        Service AddToCurrentStatus(Service service, ServiceStatus status);

        /// <summary>
        /// Adds a new status as the current service status and saves the service
        /// This status will bubble down to all child services
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        /// <returns>Service</returns>
        Service AddToCurrentStatusAndChildren(Service service, ServiceStatus status);

        /// <summary>
        /// Adds a new status as the current service status and saves the service
        /// This status will bubble down to all dependent services
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        /// <returns>Service</returns>
        Service AddToCurrentStatusAndDependents(Service service, ServiceStatus status);
    }
}
