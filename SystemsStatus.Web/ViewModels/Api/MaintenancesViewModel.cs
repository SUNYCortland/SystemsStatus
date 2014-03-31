using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SystemsStatus.Web.ViewModels.Api
{
    [CollectionDataContract(Name = "Maintenances", ItemName = "Maintenance", Namespace = "")]
    public class MaintenancesViewModel<ScheduledMaintenanceApiDTO> : List<ScheduledMaintenanceApiDTO>
    {

    }
}