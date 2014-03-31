using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.Models;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class ServicesHierarchyViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public IList<ServiceSortOrderModel> SortOrder { get; set; }

        public ServicesHierarchyViewModel()
        {
            this.SortOrder = new List<ServiceSortOrderModel>();
        }
    }
}