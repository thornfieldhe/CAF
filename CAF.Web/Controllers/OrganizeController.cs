
namespace CAF.Web.Controllers
{
    using CAF.Model;
    using System.Web.Mvc;


    public class OrganizeController : BaseController
    {
        public JsonResult Get()
        {
            var organizes = Organize.GetAll();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult Put()
        {
            var organizes = Organize.GetAll();
            return Json("");
        }
    }
}
