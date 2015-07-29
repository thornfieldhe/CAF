using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAF.SPA.Web.Areas.Manage.Controllers
{
    public class HomeController : Controller
    {
        // GET: Manage/Home
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}