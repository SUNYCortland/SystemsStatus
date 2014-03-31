using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Core.Data.Repositories
{
    public interface IServiceStatusRepository : IRepository<int?, ServiceStatus>
    {
    }
}
