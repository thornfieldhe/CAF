using System.Web.Mvc;

namespace CAF.Web.Controllers
{

    using CAF_Model;

    using Microsoft.Ajax.Utilities;
    using System;
    using System.Net;

    public class ManageController : BaseController
    {
        public ActionResult Index()
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
            string whereStr = " 1=1 ";

            whereStr += organizeId.IfNotNull(o => "And OrganizeId=@OrganizeId");
            whereStr += ruleId.IfNotNull(o => "And RuleId=@RuleId");
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