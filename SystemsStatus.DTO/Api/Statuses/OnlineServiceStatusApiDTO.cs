using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api.Statuses
{
    [DataContract(Name = "Online", Namespace = "")]
    public class OnlineServiceStatusApiDTO : ServiceStatusApiDTO
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public DateTime OnlineSince { get; set; }
    }
}
