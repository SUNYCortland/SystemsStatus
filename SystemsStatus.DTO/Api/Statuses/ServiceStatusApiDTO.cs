using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api.Statuses
{
    [DataContract(Name = "Status", Namespace = "")]
    [KnownType(typeof(OnlineServiceStatusApiDTO))]
    [KnownType(typeof(OnlineDegradedServiceStatusApiDTO))]
    [KnownType(typeof(OfflineMaintenanceServiceStatusApiDTO))]
    [KnownType(typeof(OfflineUnplannedServiceStatusApiDTO))]
    public class ServiceStatusApiDTO
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(IsRequired = true, Order = 2, EmitDefaultValue = false)]
        public DateTime CreatedDate { get; set; }
    }
}
