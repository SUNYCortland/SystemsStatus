using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Common.Pagination;
using SystemsStatus.Core.Data.Entities.Statuses;

namespace SystemsStatus.Web.ViewModels
{
    public class ServicesStatusHistoryViewModel
    {
        public virtual Service Service { get; set; }
        public virtual IPagedList<ServiceStatus> Statuses { get; set; }
    }
}