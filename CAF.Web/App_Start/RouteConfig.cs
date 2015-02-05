using System.Web.Mvc;
using System.Web.Routing;

namespace CAF.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Angular",
                url: "App/{angular}",
                defaults: new { controller = "Manage", action = "Dashboard", angular = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { action = "Dashboard", id = UrlParameter.Optional }
            );

        }
    }
}

