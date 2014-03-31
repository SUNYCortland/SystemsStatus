using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using NHibernate;

namespace SystemsStatus.Data.Repositories.Nh
{
    public class NhDepartmentRepository : NhRepositoryBase<int?, Department>, IDepartmentRepository
    {
        public NhDepartmentRepository(ISession session)
            : base(session)
        {

        }
    }
}
