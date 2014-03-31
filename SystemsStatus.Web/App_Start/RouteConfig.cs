using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SystemsStatus.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Widgets",
                "Widgets/{guid}",
                new { controller = "Widgets", action = "Index" },
                new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                "ScheduledMaintenances",
                "Maintenances/Page/{page}",
                new { controller = "Maintenances", action = "Index" },
                new { page = @"^[1-9]\d*$" },
                new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                "ScheduledMaintenances_FirstPage",
                "Maintenances",
                new { controller = "Maintenances", action = "Index", page = 1 },
                new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                "ServiceLists_AZList",
                "ServiceLists/AZList/{display}/Page/{page}",
                new { controller = "ServiceLists", action = "AZList", display = UrlParameter.Optional },
                new { page = @"^[1-9]\d*$", display = "All|Online|Maintenance|Degraded|Down" },
                new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                "ServiceLists_AZList_FirstPage",
                "ServiceLists/AZList/{display}",
                new { controller = "ServiceLists", action = "AZList", display = "All", page = 1 },
                new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                "StatusHistory",
                "Services/{serviceUrl}/StatusHistory/Page/{page}",
                new { controller = "Services", action = "StatusHistory" },
                new { page = @"^[1-9]\d*$" },
                new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                "StatusHistory_FirstPage",
                "Services/{serviceUrl}/StatusHistory",
                new { controller = "Services", action = "StatusHistory", page = 1 },
                new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Services",
                url: "Services/{serviceUrl}",
                defaults: new { controller = "Services", action = "Index" },
                namespaces: new[] { "SystemsStatus.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SystemsStatus.Web.Controllers" }
            );
        }
    }
}