using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.Oracle
{
    public class OfflineUnplannedServiceStatusMapping : SubclassMap<OfflineUnplannedServiceStatus>
    {
        public OfflineUnplannedServiceStatusMapping()
        {
            Table("ss_unplanned_statuses");
            KeyColumn("status_id");
            Map(x => x.OfflineSince).Column("unplanned_status_offline_since");
            Map(x => x.ExpectedOnline).Column("unplanned_status_exp_online").Nullable();
            Map(x => x.ActualOnline).Column("unplanned_status_act_online").Nullable();
            Map(x => x.OfflineCause).Column("unplanned_status_offline_cause");
        }
    }
}
