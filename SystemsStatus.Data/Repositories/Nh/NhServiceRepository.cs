using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using NHibernate;
using NHibernate.Criterion;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Data.Repositories.Nh
{
    public class NhServiceRepository : NhRepositoryBase<int?, Service>, IServiceRepository
    {
        public NhServiceRepository(ISession session)
            : base(session)
        {

        }
    }
}
