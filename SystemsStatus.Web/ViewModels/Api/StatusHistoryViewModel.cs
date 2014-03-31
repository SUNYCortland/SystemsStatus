using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.DTO.Api.Statuses;
using System.Runtime.Serialization;
using SystemsStatus.DTO.Api;

namespace SystemsStatus.Web.ViewModels.Api
{
    [DataContract(Name = "StatusHistory", Namespace = "")]
    public class StatusHistoryViewModel
    {
        [DataMember(Name = "Service", IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public ServiceApiDTO Service { get; set; }

        [DataMember(Name = "Statuses", IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public IEnumerable<ServiceStatusApiDTO> StatusHistory { get; set; }

        public StatusHistoryViewModel()
        {
            this.StatusHistory = new List<ServiceStatusApiDTO>();
        }
    }
}