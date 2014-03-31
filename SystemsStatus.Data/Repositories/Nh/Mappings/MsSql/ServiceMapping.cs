using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.MsSql
{
    public class ServiceMapping : ClassMap<Service>
    {
        public ServiceMapping()
        {
            Table("ss_services");
            Id(x => x.Id).Column("service_id").GeneratedBy.Identity();
            Map(x => x.Name).Column("service_name");
            Map(x => x.Description).Column("service_description");
            Map(x => x.Public).Column("service_public").CustomType<YesNoType>();
            Map(x => x.Url).Column("service_friendly_url");
            Map(x => x.SortOrder).Column("service_sort_order");
            Map(x => x.SLA).Column("service_sla");
            Map(x => x.Cost).Column("service_cost");
            References(x => x.CreatedBy).Column("service_created_by");
            References(x => x.Parent).Column("service_parent_service_id");
            References(x => x.CurrentStatus).Column("service_current_status_id").Not.LazyLoad();
            References(x => x.Department).Column("service_department_id");
            HasMany(x => x.Children).Table("ss_services")
                .KeyColumn("service_parent_service_id")
                .OrderBy("service_name");
            HasMany(x => x.Maintenances).Table("ss_scheduled_maintenances")
                .KeyColumn("maintenance_service_id");
            HasMany(x => x.Tags).Table("ss_service_tags")
                .KeyColumn("tag_service_id")
                .Element("tag_value")
                .AsSet()
                .Cascade.AllDeleteOrphan()
                .OrderBy("tag_value");
            HasManyToMany(x => x.Owners).Table("ss_service_owners")
                .ParentKeyColumn("owner_service_id")
                .ChildKeyColumn("owner_user_id")
                .ChildOrderBy("user_username")
                .AsSet();
            HasManyToMany(x => x.Dependents).Table("ss_service_dependents")
                .ParentKeyColumn("dependent_parent_service_id")
                .ChildKeyColumn("dependent_child_service_id")
                .AsSet();
            HasManyToMany(x => x.Statuses).Table("ss_service_statuses")
                .ParentKeyColumn("service_status_service_id")
                .ChildKeyColumn("service_status_status_id")
                .ChildOrderBy("status_created_date")
                .AsSet();
        }
    }
}
