using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using FluentNHibernate.Mapping;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.MsSql
{
    public class OnlineDegradedServiceStatusMapping : SubclassMap<OnlineDegradedServiceStatus>
    {
        public OnlineDegradedServiceStatusMapping()
        {
            Table("ss_degraded_statuses");
            KeyColumn("status_id");
            Map(x => x.Cause).Column("degraded_status_cause");
            Map(x => x.ExpectedOnline).Column("degraded_status_exp_online").Nullable();
            Map(x => x.ActualOnline).Column("degraded_status_act_online").Nullable();
            Map(x => x.DegradedSince).Column("degraded_status_degraded_since");
        }
    }
}
