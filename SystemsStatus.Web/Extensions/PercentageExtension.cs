using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemsStatus.Web.Extensions
{
    public static class PercentageExtension
    {
        public static string ToPercentage(this HtmlHelper helper, double value)
        {
            return String.Format("{0:0.00%}", value);
        }

        public static string ToPercentage(double value)
        {
            return String.Format("{0:0.00%}", value);
        }
    }
}