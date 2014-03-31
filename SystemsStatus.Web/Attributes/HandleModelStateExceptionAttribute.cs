using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Web.Exceptions;
using System.Text;

namespace SystemsStatus.Web.Attributes
{
    /// <summary>
    /// Represents errors that occur due to invalid application model state.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HandleModelStateExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Called when an exception occurs and processes <see cref="ModelStateException"/> object.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            // handle modelStateException
            if (filterContext.Exception != null && typeof(ModelStateException).IsInstanceOfType(filterContext.Exception) && !filterContext.ExceptionHandled)
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, errors = (filterContext.Exception as ModelStateException).Errors }
                };
            }
        }
    }
}