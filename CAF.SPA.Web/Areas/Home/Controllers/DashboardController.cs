using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAF.SPA.Web.Areas.Home.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Home/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}