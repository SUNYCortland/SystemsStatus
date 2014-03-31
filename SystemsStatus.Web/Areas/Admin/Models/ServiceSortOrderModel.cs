using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemsStatus.Web.Areas.Admin.Models
{
    public class ServiceSortOrderModel
    {
        public int id { get; set; }
        public IList<ServiceSortOrderModel> children { get; set; }

        public ServiceSortOrderModel()
        {
            this.children = new List<ServiceSortOrderModel>();
        }
    }
}