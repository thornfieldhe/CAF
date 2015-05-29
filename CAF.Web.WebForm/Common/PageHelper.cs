using System;
using System.Linq;
namespace CAF.Web.WebForm.Common
{
    using CAF.Model;
    using FineUI;

    using global::System.Collections.Generic;

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
                var items = Directory.GetOtherDirectories(id).Select(d => new ListItem { Text = d.Name, Value = d.Level, SimulateTreeLevel = d.Sort }).ToList();
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
            }
            else
            {
                var items = Organize.GetOtherOrganizes(id).Select(d => new ListItem { Text = d.Name, Value = d.Level, SimulateTreeLevel = d.Sort }).ToList();
                PageTools.BindDropdownList(items, drop, selectItem, "");
            }
        }


        /// <summary>
        /// 绑定岗位用户下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="drop"></param>
        /// <param name="selectItem"></param>
        public static void BindPostUsers(Guid id, DropDownList drop, string selectItem)
        {
            var users = new List<User>();
            var items = id!=Guid.Empty ? Post.Get(id).Users.Distinct().Select(u => new ListItem { Text = u.Name, Value = u.Id.ToString() }).ToList() 
                             : Post.GetAll().SelectMany(p => p.Users).Distinct().Select(u => new ListItem { Text = u.Name, Value = u.Id.ToString() }).ToList();
            PageTools.BindDropdownList(items, drop, selectItem);
        }

        /// <summary>
        /// 绑定岗位下拉列表
        /// </summary>
        /// <param name="drop"></param>
        /// <param name="selectItem"></param>
        public static void BindPosts(DropDownList drop, string selectItem)
        {
            var items = Post.GetAll().Select(u => new ListItem { Text = u.Name, Value = u.Id.ToString() }).ToList();
            PageTools.BindDropdownList(items, drop, selectItem);
        }

        /// <summary>
        /// 绑定角色复选框
        /// </summary>
        /// <param name="chk"></param>
        public static void BindRoles(CheckBoxList chk)
        {
            chk.DataSource = Role.GetAll().Select(r => new
            {
                Text = r.Name,
                Value = r.Id
            });
            chk.DataTextField = "Text";
            chk.DataValueField = "Value";
            chk.DataBind();
        }

        /// <summary>
        /// 绑定岗位复选框
        /// </summary>
        /// <param name="chk"></param>
        public static void BindPosts(CheckBoxList chk)
        {
            chk.DataSource = Post.GetAll().Select(r => new
            {
                Text = r.Name,
                Value = r.Id
            });
            chk.DataTextField = "Text";
            chk.DataValueField = "Value";
            chk.DataBind();
        }
        /// <summary>
        /// 绑定角色单选框
        /// </summary>
        /// <param name="radio"></param>
        /// <param name="enumItem"></param>
        public static void BindRoles(RadioButtonList radio, Type enumItem)
        {
            radio.DataSource = EnumContent.Get(enumItem).Select(r => new
            {
                Text = r.Description,
                Value = r.Value.ToString()
            });
            radio.DataTextField = "Text";
            radio.DataValueField = "Value";
            radio.DataBind();
        }

        /// <summary>
        /// 绑定角色下拉列表
        /// </summary>
        /// <param name="drop"></param>
        /// <param name="selectItem"></param>
        public static void BindRoles(DropDownList drop, string selectItem)
        {
            var items = Role.GetAll().Select(d => new ListItem { Text = d.Name, Value = d.Id.ToString() }).ToList();
            PageTools.BindDropdownList(items, drop, selectItem);
        }

    }
}