using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.DTO.Api;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SystemsStatus.Web.ViewModels.Api
{
    [CollectionDataContract(Name = "Services", ItemName = "Service", Namespace = "")]
    public class ServicesViewModel<ServiceApiDTO> : List<ServiceApiDTO>
    {
    }
}