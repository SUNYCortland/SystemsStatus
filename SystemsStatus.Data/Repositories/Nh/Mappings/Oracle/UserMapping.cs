using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SystemsStatus.Core.Data.Entities;
using NHibernate.Type;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.Oracle
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Table("ss_users");
            Id(x => x.Id).Column("user_id").GeneratedBy.Sequence("seq_ss_user_id");
            Map(x => x.FirstName).Column("user_first_name");
            Map(x => x.LastName).Column("user_last_name");
            Map(x => x.Username).Column("user_username");
            Map(x => x.HashedPassword).Column("user_hashed_password");
            Map(x => x.PasswordSalt).Column("user_password_salt");
            Map(x => x.Active).Column("user_active").CustomType<YesNoType>();
            References(x => x.Role).Column("user_role_id");
            HasManyToMany(x => x.Services).Table("ss_service_owners")
                .ParentKeyColumn("owner_user_id")
                .ChildKeyColumn("owner_service_id")
                .ChildOrderBy("service_name")
                .AsSet();
        }
    }
}
