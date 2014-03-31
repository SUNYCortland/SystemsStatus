using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.MsSql
{
    public class OfflineMaintenanceServiceStatusMapping : SubclassMap<OfflineMaintenanceServiceStatus>
    {
        public OfflineMaintenanceServiceStatusMapping()
        {
            Table("ss_maint_statuses");
            KeyColumn("status_id");
            References(x => x.ScheduledMaintenance).Column("maint_status_maintenance_id");
        }
    }
}
