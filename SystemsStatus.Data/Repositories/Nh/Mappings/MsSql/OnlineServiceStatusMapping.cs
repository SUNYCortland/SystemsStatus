using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using FluentNHibernate.Mapping;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.MsSql
{
    public class OnlineServiceStatusMapping : SubclassMap<OnlineServiceStatus>
    {
        public OnlineServiceStatusMapping()
        {
            Table("ss_online_statuses");
            KeyColumn("status_id");
            Map(x => x.OnlineSince).Column("online_status_online_since");
        }
    }
}
