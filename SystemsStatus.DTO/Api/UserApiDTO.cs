using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api
{
    [DataContract(Name = "User", Namespace = "")]
    public class UserApiDTO
    {
        [DataMember(IsRequired = true, Order = 0, EmitDefaultValue = false)]
        public string FirstName { get; set; }

        [DataMember(IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public string LastName { get; set; }

        [DataMember(IsRequired = true, Order = 2, EmitDefaultValue = false)]
        public string Username { get; set; }

        [DataMember(IsRequired = true, Order = 3, EmitDefaultValue = false)]
        public virtual bool Active { get; set; }

        [DataMember(IsRequired = true, Order = 4, EmitDefaultValue = false)]
        public UserRoleApiDTO Role { get; set; }
    }
}
