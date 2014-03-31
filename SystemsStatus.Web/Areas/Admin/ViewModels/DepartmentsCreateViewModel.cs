using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class DepartmentsCreateViewModel
    {
        public virtual Department Department { get; set; }
    }
}