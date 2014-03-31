using System.Web.Mvc;
using System;

namespace SystemsStatus.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_Departments",
                "Admin/Departments/Page/{page}",
                new { controller = "Departments", action = "Index" },
                new { page = @"^[1-9]\d*$" },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Departments_FirstPage",
                "Admin/Departments",
                new { controller = "Departments", action = "Index", page = 1 },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Services_ServiceList",
                "Admin/Services/ServiceList/{display}/Page/{page}",
                new { controller = "Services", action = "ServiceList", display = UrlParameter.Optional },
                new { page = @"^[1-9]\d*$", display = "All|Online|Maintenance|Degraded|Down" },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Services_ServiceList_FirstPage",
                "Admin/Services/ServiceList/{display}",
                new { controller = "Services", action = "ServiceList", display = "All", page = 1 },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Account_MyServices",
                "Admin/Account/MyServices/{display}/Page/{page}",
                new { controller = "Account", action = "MyServices", display = UrlParameter.Optional },
                new { page = @"^[1-9]\d*$", display = "All|Online|Maintenance|Degraded|Down" },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Account_MyServices_FirstPage",
                "Admin/Account/MyServices/{display}",
                new { controller = "Account", action = "MyServices", display = "All", page = 1 },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Account_MyWidgets",
                "Admin/Account/MyWidgets/Page/{page}",
                new { controller = "Account", action = "MyWidgets" },
                new { page = @"^[1-9]\d*$", display = "All|Online|Maintenance|Degraded|Down" },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Account_MyWidgets_FirstPage",
                "Admin/Account/MyWidgets",
                new { controller = "Account", action = "MyWidgets", page = 1 },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Users",
                "Admin/Users/Page/{page}",
                new { controller = "Users", action = "Index" },
                new { page = @"^[1-9]\d*$" },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Users_FirstPage",
                "Admin/Users",
                new { controller = "Users", action = "Index", page = 1 },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "SystemsStatus.Web.Areas.Admin.Controllers" }
            );
        }
    }
}
