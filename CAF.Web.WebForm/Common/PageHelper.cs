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
        /// <param name="selectItem"></param>
        /// <param name="useLevel"></param>
        public static void BindDirectories(Guid id, DropDownList drop, string selectItem, bool useLevel = false)
        {
            if (!useLevel)
            {
                var items = Directory.GetOtherDirectories(id).Select(d => new ListItem { Text = d.Name, Value = d.Id.ToString(), SimulateTreeLevel = d.Sort }).ToList();
                PageTools.BindDropdownList(items, drop, selectItem);
            }
            else
            {
                var items = Directory.GetOtherDirectories(id).Select(d => new ListItem { Text = d.Name, Value = d.Level,SimulateTreeLevel = d.Sort}).ToList();
                PageTools.BindDropdownList(items, drop, selectItem, "");
            }

        }

        /// <summary>
        /// 绑定部门下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="drop"></param>
        /// <param name="selectItem"></param>
        /// <param name="useLevel"></param>
        public static void BindOrganizes(Guid id, DropDownList drop, string selectItem, bool useLevel = false)
        {
            if (!useLevel)
            {
                var items = Organize.GetOtherOrganizes(id).Select(d => new ListItem { Text = d.Name, Value = d.Id.ToString(), SimulateTreeLevel = d.Sort }).ToList();
                PageTools.BindDropdownList(items, drop, selectItem);
            }else
            {
                var items = Organize.GetOtherOrganizes(id).Select(d => new ListItem { Text = d.Name, Value = d.Level, SimulateTreeLevel = d.Sort }).ToList();
                PageTools.BindDropdownList(items, drop, selectItem, "");
            }
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