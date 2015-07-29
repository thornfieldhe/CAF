using System.Web.Mvc;
using System.Web.Routing;

namespace CAF.SPA.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                  defaults: new { controller = "Dashboards", action = "Dashboard_1", id = UrlParameter.Optional }
            );
        }
    }
}
