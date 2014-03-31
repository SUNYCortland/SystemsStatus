using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SystemsStatus.Core.Data.Entities;
using NHibernate.Type;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.MsSql
{
    public class WidgetMapping : ClassMap<Widget>
    {
        public WidgetMapping()
        {
            Table("ss_widgets");
            Id(x => x.Id).Column("widget_id").GeneratedBy.Identity();
            Map(x => x.Guid).Column("widget_guid").CustomType<GuidType>();
            Map(x => x.Height).Column("widget_height");
            Map(x => x.Name).Column("widget_name");
            References(x => x.Owner).Column("widget_user_id");
            References(x => x.Department).Column("widget_department_id");
            HasManyToMany(x => x.Services).Table("ss_widget_services")
                .ParentKeyColumn("widget_id")
                .ChildKeyColumn("widget_service_id")
                .ChildOrderBy("service_sort_order")
                .AsSet();
        }
    }
}
