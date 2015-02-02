using System.Web.Optimization;

namespace CAF.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/semantic/css")
                .Include("~/metronic/global/plugins/font-awesome/css/font-awesome.min.css",
                "~/metronic/global/plugins/simple-line-icons/simple-line-icons.min.css",
                "~/metronic/global/plugins/simple-line-icons/simple-line-icons.min.css",
                "~/metronic/global/plugins/bootstrap/css/bootstrap.min.css",
                "~/metronic/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
                "~/metronic/global/css/plugins.css",
                "~/metronic/global/css/components.css",
                "~/metronic/admin/layout/css/layout.css",
                "~/metronic/admin/layout/css/themes/default.css"));

            bundles.Add(new ScriptBundle("~/bundles/semantic/js")
                .Include("~/metronic/global/plugins/jquery-1.11.0.min.js",
                "~/metronic/global/plugins/jquery-migrate-1.2.1.min.js",
                "~/metronic/global/plugins/bootstrap/js/bootstrap.min.js",
                "~/metronic/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                "~/metronic/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/metronic/global/scripts/metronic.js",
                "~/metronic/admin/layout/scripts/layout.js",
                "~/metronic/admin/layout/scripts/quick-sidebar.js"));

            bundles.Add(new ScriptBundle("~/Scripts/angular")
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/angular-resource.js")
                .Include("~/Scripts/angular-messages.js")
                .Include("~/Scripts/restangular.js"));

            bundles.Add(new ScriptBundle("~/Scripts/app")
                .Include("~/Scripts/app/*.js")
                 .Include("~/Scripts/app/controllers/*.js")
                  .Include("~/Scripts/app/services/*.js"));
        }
    }
}
