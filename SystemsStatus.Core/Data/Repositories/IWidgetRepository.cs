using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Data.Repositories
{
    public interface IWidgetRepository : IRepository<int?, Widget>
    {
    }
}
