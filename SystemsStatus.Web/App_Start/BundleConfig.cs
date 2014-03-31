using System.Web;
using System.Web.Optimization;

namespace SystemsStatus
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/jquery-ui").Include(
                "~/Scripts/jquery-ui-1.10.3.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/jquery").Include(
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/jquery.ajaxRequest.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-ajax*"));

            bundles.Add(new ScriptBundle("~/bundles/js/bootstrap.min").Include(
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/knockoutjs").Include(
                "~/Areas/Admin/Scripts/knockout-3.0.0.js"));

            bundles.Add(new StyleBundle("~/bundles/css/bootstrap.min")
                .Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/bootstrap-theme.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/css/bootstrap")
                .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/bootstrap-theme.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/css/theme")
                .Include("~/Content/theme.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/css/admin-theme")
                .Include("~/Areas/Admin/Content/theme.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/css/jquery-ui")
                .Include("~/Content/jquery-ui-1.10.3.custom.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/css/widget")
                .Include("~/Content/widget.css", new CssRewriteUrlTransform()));
        }
    }
}