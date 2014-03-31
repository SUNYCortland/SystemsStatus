using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace SystemsStatus.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}.{ext}",
                defaults: new { ext = "xml|json" }
            );

            config.Routes.MapHttpRoute(
                name: "ActionApiServiceUrl",
                routeTemplate: "api/{controller}/{action}.{ext}/{url}",
                defaults: new { url = RouteParameter.Optional, ext = "xml|json" }
            );

            var jsonFormatter = config.Formatters.JsonFormatter;
            var xmlFormatter = config.Formatters.XmlFormatter;

            jsonFormatter.AddUriPathExtensionMapping("json", "application/json");
            xmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");
        }
    }
}
