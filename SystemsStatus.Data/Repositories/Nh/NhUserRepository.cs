using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using NHibernate;

namespace SystemsStatus.Data.Repositories.Nh
{
    public class NhUserRepository : NhRepositoryBase<int?, User>, IUserRepository
    {
        public NhUserRepository(ISession _session)
            : base(_session)
        {

        }
    }
}
