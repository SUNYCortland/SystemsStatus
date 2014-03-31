using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Type;
using SystemsStatus.Data.Repositories.Nh.Mappings.Types;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.Oracle
{
    public class WidgetMapping : ClassMap<Widget>
    {
        public WidgetMapping()
        {
            Table("ss_widgets");
            Id(x => x.Id).Column("widget_id").GeneratedBy.Sequence("seq_ss_widget_id");
            Map(x => x.Guid).Column("widget_guid").Generated.Insert().CustomType<OracleGuidType>();
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
