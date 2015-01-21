
namespace CAF.Web.Controllers
{
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        /// <summary>
        /// 路由前进行权限验证
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"];
            var action = filterContext.RouteData.Values["action"];
            base.OnActionExecuting(filterContext);
        }
    }
}