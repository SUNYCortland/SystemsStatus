using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api
{
    [DataContract(Name = "Role", Namespace = "")]
    public class UserRoleApiDTO
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}
