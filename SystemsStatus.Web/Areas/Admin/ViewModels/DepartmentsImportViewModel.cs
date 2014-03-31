using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SystemsStatus.Web.Areas.Admin.ViewModels
{
    public class DepartmentsImportViewModel
    {
        public virtual HttpPostedFileBase DepartmentsFile { get; set; }

        [Display(Name = "Departments XML")]
        [AllowHtml]
        public virtual string DepartmentsXml { get; set; }
    }
}