using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;

namespace SystemsStatus.Data.Repositories.Nh
{
    public class NhMsSql2012Configuration : PersistenceConfiguration<NhMsSql2012Configuration, FluentNHibernate.Cfg.Db.MsSqlConnectionStringBuilder>
    {
        protected NhMsSql2012Configuration()
        {
            Driver<NHibernate.Driver.OleDbDriver>();
        }

        public static NhMsSql2012Configuration Dialect
        {
            get
            {
                return new NhMsSql2012Configuration().Dialect<NHibernate.Dialect.MsSql2012Dialect>();
            }
        }
    }
}
