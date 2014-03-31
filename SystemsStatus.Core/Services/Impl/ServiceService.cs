using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Core.Services.Impl
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceStatusRepository _serviceStatusRepository;
        private readonly IScheduledMaintenanceRepository _scheduledMaintenanceRepository;
        private readonly IWidgetService _widgetService;

        public ServiceService(IServiceRepository serviceRepository, 
                                IServiceStatusRepository serviceStatusRepository,
                                IScheduledMaintenanceRepository scheduledMaintenanceRepository,
                                IWidgetService widgetService)
        {
            _serviceRepository = serviceRepository;
            _serviceStatusRepository = serviceStatusRepository;
            _scheduledMaintenanceRepository = scheduledMaintenanceRepository;
            _widgetService = widgetService;
        }

        /// <summary>
        /// Returns all services.
        /// </summary>
        /// <returns>List of all services</returns>
        public IQueryable<Service> GetAllServices()
        {
            return _serviceRepository.All();
        }

        /// <summary>
        /// Returns all public services.
        /// </summary>
        /// <returns>List of all public services</returns>
        public IQueryable<Service> GetAllPublicServices()
        {
            return _serviceRepository.FilterBy(x => x.Public);
        }

        /// <summary>
        /// Returns all private services.
        /// </summary>
        /// <returns>List of all private services</returns>
        public IQueryable<Service> GetAllPrivateServices()
        {
            return _serviceRepository.FilterBy(x => !x.Public);
        }

        /// <summary>
        /// Returns all parent services.
        /// </summary>
        /// <returns>List of all parent services</returns>
        public IQueryable<Service> GetAllParentServices()
        {
            return _serviceRepository.FilterBy(x => x.Parent == null);
        }

        /// <summary>
        /// Returns all public parent services.
        /// </summary>
        /// <returns>List of all public parent services</returns>
        public IQueryable<Service> GetAllPublicParentServices()
        {
            return _serviceRepository.FilterBy(x => x.Public && x.Parent == null);
        }

        /// <summary>
        /// Returns all private parent services.
        /// </summary>
        /// <returns>List of all private parent services</returns>
        public IQueryable<Service> GetAllPrivateParentServices()
        {
            return _serviceRepository.FilterBy(x => !x.Public && x.Parent == null);
        }

        /// <summary>
        /// Returns all services with a current status of the passed type
        /// </summary>
        /// <param name="type">Type of current status</param>
        /// <returns>List of all services with a current status of the passed type</returns>
        public IQueryable<Service> GetAllServicesWithCurrentStatusType(Type type)
        {
            if (type == typeof(OnlineServiceStatus))
            {
                return _serviceRepository.All().Where(x => x.CurrentStatus.Type == "online");
            }
            else if (type == typeof(OnlineDegradedServiceStatus))
            {
                return _serviceRepository.All().Where(x => x.CurrentStatus.Type == "degraded");
            }
            else if (type == typeof(OfflineMaintenanceServiceStatus))
            {
                return _serviceRepository.All().Where(x => x.CurrentStatus.Type == "maintenance");
            }
            else if (type == typeof(OfflineUnplannedServiceStatus))
            {
                return _serviceRepository.All().Where(x => x.CurrentStatus.Type == "down");
            }

            return _serviceRepository.All();
        }

        /// <summary>
        /// Returns services by department code
        /// </summary>
        /// <param name="code">Department code of service to return</param>
        /// <returns>List of all services that belong to the specified department code</returns>
        public IQueryable<Service> GetServicesByDepartmentCode(string code)
        {
            return _serviceRepository.All().Where(x => x.Department != null && x.Department.Code.ToUpper() == code.ToUpper());
        }

        /// <summary>
        /// Returns a service by url
        /// </summary>
        /// <param name="url">Url of service to return</param>
        /// <returns>Service</returns>
        public Service GetServiceByUrl(string url)
        {
            return _serviceRepository.FindBy(x => x.Url.ToLower() == url.ToLower());
        }

        /// <summary>
        /// Returns a service
        /// </summary>
        /// <param name="id">Id of service to return</param>
        /// <returns>Service</returns>
        public Service GetServiceById(int id)
        {
            return _serviceRepository.FindBy(x => x.Id == id);
        }

        /// <summary>
        /// Inserts a service
        /// </summary>
        /// <param name="service">Service to insert</param>
        /// <returns>Service</returns>
        [UnitOfWork]
        public Service InsertService(Service service)
        {
            return AddToCurrentStatus(service, service.CurrentStatus);
        }

        /// <summary>
        /// Deletes a service and all dependencies
        /// </summary>
        /// <param name="service">Service to delete</param>
        [UnitOfWork]
        public void DeleteService(Service service)
        {
            // Delete reference in widgets
            var widgets = _widgetService.GetAllWidgetsByService(service);

            foreach (var widget in widgets)
            {
                widget.Services.Remove(service);

                _widgetService.SaveWidget(widget);
            }

            // Delete all scheduled maintenance
            foreach (var maintenance in service.Maintenances)
            {
                _scheduledMaintenanceRepository.Delete(maintenance);
            }

            // All children must move up a level
            foreach (var childService in service.Children)
            {
                childService.Parent = service.Parent;

                _serviceRepository.Update(childService);
            }

            // Delete service
            _serviceRepository.Delete(service);

            // Delete all statuses
            foreach (var status in service.Statuses)
            {
                if (status.GetType() == typeof(OfflineMaintenanceServiceStatus))
                {
                    var currentStatus = (OfflineMaintenanceServiceStatus)status;

                    _scheduledMaintenanceRepository.Delete(currentStatus.ScheduledMaintenance);
                }

                _serviceStatusRepository.Delete(status);
            }
        }

        /// <summary>
        /// Saves a service
        /// </summary>
        /// <param name="service">Service to save</param>
        /// <returns>Saved service</returns>
        [UnitOfWork]
        public Service SaveService(Service service)
        {
            return _serviceRepository.Update(service);
        }

        /// <summary>
        /// Merges a service with its entity
        /// </summary>
        /// <param name="service">Service to merge</param>
        /// <returns>Merged Service</returns>
        public Service MergeService(Service service)
        {
            return _serviceRepository.Merge(service);
        }

        /// <summary>
        /// Adds a new status as the current service status and saves the service
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        /// <returns>Service</returns>
        [UnitOfWork]
        public Service AddToCurrentStatus(Service service, ServiceStatus status)
        {
            status = _serviceStatusRepository.Insert(status);
            service.CurrentStatus = status;
            service.Statuses.Add(status);

            return _serviceRepository.SaveOrUpdate(service);
        }

        /// <summary>
        /// Adds a new status as the current service status and saves the service
        /// This status will bubble down to all child services
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        /// <returns>Service</returns>
        [UnitOfWork]
        public Service AddToCurrentStatusAndChildren(Service service, ServiceStatus status)
        {
            status = _serviceStatusRepository.Insert(status);
            service.CurrentStatus = status;
            service.Statuses.Add(status);

            ProcessAddToCurrentStatusAndChildren(service, status);

            return _serviceRepository.SaveOrUpdate(service);
        }

        /// <summary>
        /// Recursive method to add the status to all children of the specified service
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        private void ProcessAddToCurrentStatusAndChildren(Service service, ServiceStatus status)
        {
            if (service.Children.Count() == 0)
                return;

            foreach(var childService in service.Children)
            {
                childService.CurrentStatus = status;
                childService.Statuses.Add(status);
                _serviceRepository.Update(childService);

                ProcessAddToCurrentStatusAndChildren(childService, status);
            }
        }

        /// <summary>
        /// Adds a new status as the current service status and saves the service
        /// This status will bubble down to all dependent services
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        /// <returns>Service</returns>
        [UnitOfWork]
        public Service AddToCurrentStatusAndDependents(Service service, ServiceStatus status)
        {
            status = _serviceStatusRepository.Insert(status);
            service.CurrentStatus = status;
            service.Statuses.Add(status);

            ProcessAddToCurrentStatusAndDependents(service, status);

            return _serviceRepository.SaveOrUpdate(service);
        }

        /// <summary>
        /// Recursive method to add the status to all dependents of the specified service
        /// </summary>
        /// <param name="service">Service to add status to</param>
        /// <param name="status">Status to add</param>
        private void ProcessAddToCurrentStatusAndDependents(Service service, ServiceStatus status)
        {
            if (service.Dependents.Count() == 0)
                return;

            foreach (var dependentService in service.Dependents)
            {
                dependentService.CurrentStatus = status;
                dependentService.Statuses.Add(status);
                _serviceRepository.Update(dependentService);

                ProcessAddToCurrentStatusAndDependents(dependentService, status);
            }
        }
    }
}
