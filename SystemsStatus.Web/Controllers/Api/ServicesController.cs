using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Services;
using SystemsStatus.DTO.Api;
using AutoMapper;
using SystemsStatus.Core.Data.Entities.Statuses;
using SystemsStatus.DTO.Api.Statuses;
using SystemsStatus.Web.ViewModels.Api;

namespace SystemsStatus.Web.Controllers.Api
{
    public class ServicesController : ApiController
    {
        private readonly IServiceService _serviceService;
        private readonly IDepartmentService _departmentService;
        private readonly IServiceStatusService _serviceStatusService;

        public ServicesController(IServiceService serviceService,
                                    IDepartmentService departmentService,
                                    IServiceStatusService serviceStatusService)
        {
            _serviceService = serviceService;
            _departmentService = departmentService;
            _serviceStatusService = serviceStatusService;
        }

        //
        // GET: /api/Services/All.{ext}

        [HttpGet]
        public ServicesViewModel<ServiceApiDTO> All()
        {
            var services = _serviceService.GetAllServices();

            var servicesDTO = Mapper.Map<IEnumerable<Service>, ServicesViewModel<ServiceApiDTO>>(services);

            return servicesDTO;
        }

        //
        // GET: /api/Services/Online.{ext}

        [HttpGet]
        public ServicesViewModel<ServiceApiDTO> Online()
        {
            var services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OnlineServiceStatus));

            var servicesDTO = Mapper.Map<IEnumerable<Service>, ServicesViewModel<ServiceApiDTO>>(services);

            return servicesDTO;
        }

        //
        // GET: /api/Services/Degraded.{ext}

        [HttpGet]
        public ServicesViewModel<ServiceApiDTO> Degraded()
        {
            var services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OnlineDegradedServiceStatus));

            var servicesDTO = Mapper.Map<IEnumerable<Service>, ServicesViewModel<ServiceApiDTO>>(services);

            return servicesDTO;
        }

        //
        // GET: /api/Services/Down.{ext}

        [HttpGet]
        public ServicesViewModel<ServiceApiDTO> Down()
        {
            var services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OfflineUnplannedServiceStatus));

            var servicesDTO = Mapper.Map<IEnumerable<Service>, ServicesViewModel<ServiceApiDTO>>(services);

            return servicesDTO;
        }

        //
        // GET: /api/Services/Maintenance.{ext}

        [HttpGet]
        public ServicesViewModel<ServiceApiDTO> Maintenance()
        {
            var services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OfflineMaintenanceServiceStatus));

            var servicesDTO = Mapper.Map<IEnumerable<Service>, ServicesViewModel<ServiceApiDTO>>(services);

            return servicesDTO;
        }

        //
        // GET: /api/Services/ByDepartment.{ext}/{url}
        
        [HttpGet]
        public ServicesViewModel<ServiceApiDTO> ByDepartment(string url)
        {
            var department = _departmentService.GetDepartmentByCode(url);

            if (department == null)
            {
                var error = new HttpError(String.Format("No department found with department code = '{0}'", url));

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, error));
            }

            var services = _serviceService.GetServicesByDepartmentCode(url);

            var servicesDTO = Mapper.Map<IEnumerable<Service>, ServicesViewModel<ServiceApiDTO>>(services);

            return servicesDTO;
        }
        
        //
        // GET: /api/Services/Service.{ext}/{url}

        [HttpGet]
        public ServiceApiDTO Service(string url)
        {
            var service = _serviceService.GetServiceByUrl(url);

            if (service == null)
            {
                var error = new HttpError(String.Format("No service found with URL = '{0}'", url));

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, error));
            }

            var serviceDTO = Mapper.Map<Service, ServiceApiDTO>(service);

            return serviceDTO;
        }

        //
        // GET: /api/Services/StatusHistory.{ext}/{url}

        [HttpGet]
        public StatusHistoryViewModel StatusHistory(string url, int? count = null)
        {
            var service = _serviceService.GetServiceByUrl(url);

            var serviceDTO = new StatusHistoryViewModel();

            if (service == null)
            {
                var error = new HttpError(String.Format("No service found with URL = '{0}'", url));

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, error));
            }

            var statuses = _serviceStatusService.GetAllServiceStatuses(service);
            
            if (count.HasValue)
                serviceDTO.StatusHistory = Mapper.Map<IEnumerable<ServiceStatus>, IEnumerable<ServiceStatusApiDTO>>(statuses.OrderByDescending(x => x.CreatedDate).Take(count.Value));
            else
                serviceDTO.StatusHistory = Mapper.Map<IEnumerable<ServiceStatus>, IEnumerable<ServiceStatusApiDTO>>(statuses.OrderByDescending(x => x.CreatedDate));
            
            serviceDTO.Service = Mapper.Map<Service, ServiceApiDTO>(service);

            return serviceDTO;
        }
    }
}
