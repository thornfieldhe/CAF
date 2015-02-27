using System.Web.Mvc;

namespace CAF.Web.Controllers
{

    using CAF.Ext;
    using CAF_Model;
    using System;
    using System.Net;

    public class ManageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Organizes()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View("Users");
        }

        #region UserManage
        [HttpPost]
        public JsonResult GetUser(Guid userId)
        {
            return this.Json(Model.User.Get(userId));
        }

        [HttpPost]
        public JsonResult GetUsers(string name, Guid? organizeId, Guid? ruleId)
        {
            var whereStr = " 1=1 ";

            organizeId.IfNotNull(o => whereStr += "And OrganizeId=@OrganizeId");
            ruleId.IfNotNull(o => whereStr = "And RuleId=@RuleId");
            name.IfIsNotNullOrEmpty(a => whereStr += " And Name Like %@Name%");

            return this.Json(ReadOnlyUserList.Instance.Query("Name", 20, new { Name = name, OrganizeId = organizeId, RuleId = ruleId }, whereStr));
        }

        public ActionResult SaveUser(Model.User user)
        {
            user.Save();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        #endregion
    }
}