using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api.Statuses
{
    [DataContract(Name = "Degraded", Namespace = "")]
    public class OnlineDegradedServiceStatusApiDTO : ServiceStatusApiDTO
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public string Cause { get; set; }

        [DataMember(IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public DateTime DegradedSince { get; set; }

        [DataMember(IsRequired = false, Order = 2, EmitDefaultValue = false)]
        public DateTime? ExpectedOnline { get; set; }

        [DataMember(IsRequired = false, Order = 3, EmitDefaultValue = false)]
        public DateTime? ActualOnline { get; set; }
    }
}
