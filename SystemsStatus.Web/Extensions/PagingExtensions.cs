using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Models;
using System.Web.Mvc.Ajax;
using System.Web.Mvc;

namespace SystemsStatus.Web.Extensions
{
    public static class PagingExtensions
    {
        #region HtmlHelper extensions

        public static Pager Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return new Pager(htmlHelper, pageSize, currentPage, totalItemCount);
        }

        public static Pager Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, AjaxOptions ajaxOptions)
        {
            return new Pager(htmlHelper, pageSize, currentPage, totalItemCount).Options(o => o.AjaxOptions(ajaxOptions));
        }

        #endregion
    }
}