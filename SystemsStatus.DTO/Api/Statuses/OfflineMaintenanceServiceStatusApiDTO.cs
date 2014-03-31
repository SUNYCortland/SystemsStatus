using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api.Statuses
{
    [DataContract(Name = "Maintenance", Namespace = "")]
    public class OfflineMaintenanceServiceStatusApiDTO : ServiceStatusApiDTO
    {
        [DataMember]
        public ScheduledMaintenanceApiDTO ScheduledMaintenance { get; set; }
    }
}
