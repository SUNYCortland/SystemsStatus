using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.MsSql
{
    public class ServiceStatusMapping : ClassMap<ServiceStatus>
    {
        public ServiceStatusMapping()
        {
            Table("ss_statuses");
            Id(x => x.Id).Column("status_id").GeneratedBy.Identity();
            Map(x => x.Name).Column("status_name");
            Map(x => x.CreatedDate).Column("status_created_date");
            Map(x => x.Type).Column("status_type");
            References(x => x.CreatedBy).Column("status_created_by");
            HasManyToMany(x => x.Services)
                .Table("ss_service_statuses")
                .ParentKeyColumn("service_status_status_id")
                .ChildKeyColumn("service_status_service_id")
                .ChildOrderBy("service_name")
                .AsSet();
        }
    }
}
