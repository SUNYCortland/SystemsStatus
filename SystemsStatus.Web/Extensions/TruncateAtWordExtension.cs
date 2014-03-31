using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemsStatus.Web.Extensions
{
    public static class TruncateAtWordExtension
    {
        public static string TruncateAtWord(this HtmlHelper helper, string value, int length)
        {
            if (value == null || value.Length < length)
                return value;
            int iNextSpace = value.LastIndexOf(" ", length);
            return string.Format("{0}...", value.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }
    }
}