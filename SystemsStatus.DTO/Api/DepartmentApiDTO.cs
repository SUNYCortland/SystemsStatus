using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api
{
    [DataContract(Name = "Department", Namespace = "")]
    public class DepartmentApiDTO
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public string Code { get; set; }

        [DataMember(IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}
