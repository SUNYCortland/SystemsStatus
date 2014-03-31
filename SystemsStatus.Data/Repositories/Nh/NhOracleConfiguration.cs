using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;

namespace SystemsStatus.Data.Repositories.Nh
{
    public class NhOracleConfiguration : PersistenceConfiguration<NhOracleConfiguration, FluentNHibernate.Cfg.Db.OdbcConnectionStringBuilder>
    {
        protected NhOracleConfiguration()
        {
            Driver<NHibernate.Driver.OdbcDriver>();
        }

        public static NhOracleConfiguration Dialect
        {
            get
            {
                return new NhOracleConfiguration().Dialect<NHibernate.Dialect.Oracle10gDialect>();
            }
        }
    }
}
