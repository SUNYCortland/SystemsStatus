using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Entities.Statuses;
using SystemsStatus.DTO.Api.Statuses;
using SystemsStatus.DTO.Api.Profiles;

namespace SystemsStatus.DTO.Api
{
    public static class AutoMapperApiConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new ApiProfile());
            });
        }
    }
}
