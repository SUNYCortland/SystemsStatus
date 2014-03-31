using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api
{
    [DataContract(Name = "ScheduledMaintenance", Namespace = "")]
    public class ScheduledMaintenanceApiDTO
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(IsRequired = true, Order = 2, EmitDefaultValue = false)]
        public DateTime Offline { get; set; }

        [DataMember(IsRequired = false, Order = 3, EmitDefaultValue = false)]
        public DateTime? ExpectedOnline { get; set; }

        [DataMember(IsRequired = false, Order = 4, EmitDefaultValue = false, Name = "ActualOnline")]
        public DateTime? Online { get; set; }

        [DataMember(IsRequired = false, Order = 5, EmitDefaultValue = false)]
        public ServiceApiDTO Service { get; set; }

        [DataMember(IsRequired = true, Order = 6, EmitDefaultValue = false)]
        public UserApiDTO ScheduledBy { get; set; }
    }
}
