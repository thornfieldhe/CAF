using System.Web.Optimization;

namespace CAF.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.7.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/semantic").Include(
                        "~/Content/semantic/javascript/semantic.min.js"));


            bundles.Add(new StyleBundle("~/bundles/semanticcss").Include(
          "~/Content/semantic/css/semantic.min.css"));
        }
    }
}
