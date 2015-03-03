using System;
using System.Linq;
namespace CAF.Web.WebForm.Common
{
    using CAF.Model;
    using FineUI;



    public class PageHelper
    {
        /// <summary>
        /// 绑定目录下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="drop"></param>
        public static void BindDirectories(Guid id, DropDownList drop)
        {
            var items = Directory.GetOtherDIrectories(id).Select(d => new ListItem { Text = d.Name, Value = d.Id.ToString() }).ToList();
            PageTools.BindDropdownList(items, drop);
        }

        /// <summary>
        /// 绑定部门下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="drop"></param>
        public static void BindOrganizes(Guid id, DropDownList drop)
        {
            var items = Organize.GetChildrenOrganizes(id).Select(d => new ListItem { Text = d.Name, Value = d.Id.ToString() }).ToList();
            PageTools.BindDropdownList(items, drop);
        }

        /// <summary>
        /// 绑定角色复选框
        /// </summary>
        /// <param name="chk"></param>
        public static void BindRoles(CheckBoxList chk)
        {
            chk.DataSource = Role.GetAll().Select(r => new
            {
                Text = r.Name +
                    (r.Status == (int)IsSystemRoleEnum.System ? "<span style='color: #FF0000'>[系统]</span>" : ""),
                Value = r.Id
            });
            chk.DataTextField = "Text";
            chk.DataValueField = "Value";
            chk.DataBind();
        }
    }
}