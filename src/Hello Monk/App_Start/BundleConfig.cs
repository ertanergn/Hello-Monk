using System.Web.Optimization;

namespace Monk.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/JQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/Bootstrap/bootstrap.*"));

            bundles.Add(new ScriptBundle("~/bundles/Utilities").Include(
                        "~/Scripts/smoothscroll.js",
                        "~/Scripts/custom.js",
                        "~/Scripts/jqBootstrapValidation.js",
                        "~/Scripts/monk.js"));

            bundles.Add(new StyleBundle("~/Content/Css").Include("~/Content/Css/style.css",
                                                                 "~/Content/Css/bootstrap.*",
                                                                 "~/Content/Css/font-awesome.*"));
        }
    }
}