using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.DTO.Api.Statuses;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.DTO.Api.Profiles
{
    public class ApiProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Service, ServiceApiDTO>();
            Mapper.CreateMap<ServiceStatus, ServiceStatusApiDTO>()
                .Include<OnlineServiceStatus, OnlineServiceStatusApiDTO>()
                .Include<OnlineDegradedServiceStatus, OnlineDegradedServiceStatusApiDTO>()
                .Include<OfflineMaintenanceServiceStatus, OfflineMaintenanceServiceStatusApiDTO>()
                .Include<OfflineUnplannedServiceStatus, OfflineUnplannedServiceStatusApiDTO>();
            Mapper.CreateMap<OnlineServiceStatus, OnlineServiceStatusApiDTO>();
            Mapper.CreateMap<OnlineDegradedServiceStatus, OnlineDegradedServiceStatusApiDTO>();
            Mapper.CreateMap<OfflineMaintenanceServiceStatus, OfflineMaintenanceServiceStatusApiDTO>();
            Mapper.CreateMap<OfflineUnplannedServiceStatus, OfflineUnplannedServiceStatusApiDTO>();
            Mapper.CreateMap<Department, DepartmentApiDTO>();
            Mapper.CreateMap<User, UserApiDTO>();
            Mapper.CreateMap<UserRole, UserRoleApiDTO>();
            Mapper.CreateMap<ScheduledMaintenance, ScheduledMaintenanceApiDTO>();
        }
    }
}
