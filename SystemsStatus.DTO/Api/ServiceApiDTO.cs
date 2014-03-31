using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.DTO.Api.Statuses;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api
{
    [DataContract(Name = "Service", Namespace = "")]
    public class ServiceApiDTO : EntityApiDTO<int?>
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(IsRequired = true, Order = 2, EmitDefaultValue = false)]
        public string Url { get; set; }

        [DataMember(IsRequired = false, Order = 3, EmitDefaultValue = false)]
        public DepartmentApiDTO Department { get; set; }

        [DataMember(IsRequired = false, Order = 4, EmitDefaultValue = false)]
        public string SLA { get; set; }

        [DataMember(IsRequired = false, Order = 5, EmitDefaultValue = false)]
        public string Cost { get; set; }

        [DataMember(IsRequired = true, Order = 6, EmitDefaultValue = false)]
        public ServiceStatusApiDTO CurrentStatus { get; set; }

        [DataMember(IsRequired = true, Order = 7, EmitDefaultValue = false)]
        public UserApiDTO CreatedBy { get; set; }
    }
}
