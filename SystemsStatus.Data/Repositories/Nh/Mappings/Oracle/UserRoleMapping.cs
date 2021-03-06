﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.Oracle
{
    public class UserRoleMapping : ClassMap<UserRole>
    {
        public UserRoleMapping()
        {
            Table("ss_user_roles");
            Id(x => x.Id).Column("role_id").GeneratedBy.Sequence("seq_ss_user_role_id");
            Map(x => x.Name).Column("role_name");
            Map(x => x.Level).Column("role_level");
        }
    }
}
