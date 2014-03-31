using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentNHibernate.Mapping;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.Oracle
{
    public class DepartmentMapping : ClassMap<Department>
    {
        public DepartmentMapping()
        {
            Table("ss_departments");
            Id(x => x.Id).Column("department_id").GeneratedBy.Sequence("seq_ss_department_id");
            Map(x => x.Name).Column("department_name");
            Map(x => x.Code).Column("department_code");
            HasMany(x => x.Services)
                .Table("ss_services")
                .KeyColumn("service_department_id")
                .AsSet();
        }
    }
}
