using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAF.Web.Controllers
{
    using System.Web.Mvc;

    public class OrganizesController:BaseController
    {
        public ActionResult Dashboard()
        {
            return RedirectToAction("Dashboard", "Manage");
        }
    }
}