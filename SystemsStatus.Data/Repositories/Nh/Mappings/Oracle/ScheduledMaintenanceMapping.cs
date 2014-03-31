using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentNHibernate.Mapping;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.Oracle
{
    public class ScheduledMaintenanceMapping : ClassMap<ScheduledMaintenance>
    {
        public ScheduledMaintenanceMapping()
        {
            Table("ss_scheduled_maintenances");
            Id(x => x.Id).Column("maintenance_id").GeneratedBy.Sequence("seq_ss_scheduled_maint_id");
            Map(x => x.Name).Column("maintenance_name");
            Map(x => x.Description).Column("maintenance_description");
            Map(x => x.Offline).Column("maintenance_offline");
            Map(x => x.ExpectedOnline).Column("maintenance_expected_online");
            Map(x => x.Online).Column("maintenance_online").Nullable();
            References(x => x.Service).Column("maintenance_service_id");
            References(x => x.ScheduledBy).Column("maintenance_scheduled_by");
        }
    }
}
