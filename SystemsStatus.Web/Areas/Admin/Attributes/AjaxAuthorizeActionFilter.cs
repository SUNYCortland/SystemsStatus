using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemsStatus.Web.Areas.Admin.Attributes
{
    public class AjaxAuthorizeActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Write("{\"unauthorized\":true}");
                    filterContext.HttpContext.Response.ContentType = "application/json";
                    filterContext.HttpContext.Response.End();
                }
            }
        }
    }
}