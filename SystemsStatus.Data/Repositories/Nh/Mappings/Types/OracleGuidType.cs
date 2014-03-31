using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.UserTypes;
using NHibernate.SqlTypes;
using System.Data;

namespace SystemsStatus.Data.Repositories.Nh.Mappings.Types
{
    [Serializable] //to allow NH configuration serialization
    public class OracleGuidType : IUserType
    {
        SqlType[] sqlTypes;

        public OracleGuidType()
        {
            sqlTypes = new[] { SqlTypeFactory.GetSqlType(DbType.String, 0, 0) };
        }

        public SqlType[] SqlTypes
        {
            get { return sqlTypes; }
        }

        public Type ReturnedType
        {
            get { return typeof(Guid); }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            if (rs[names[0]] == DBNull.Value)
            {
                return Guid.Empty;
            }

            return new Guid(rs[names[0]].ToString());
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var param = (IDataParameter)cmd.Parameters[index];
            param.DbType = sqlTypes[0].DbType;
            var guid = (Guid)value;

            if (guid != Guid.Empty)
            {
                param.Value = guid.ToString("N").ToUpper();
            }
            else
            {
                param.Value = DBNull.Value;
            }
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            return x != null && x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }
    }
}
