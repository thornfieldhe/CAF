using System;

namespace CAF.Web.WebForm.Common
{
    using CAF.Model;
    using FineUI;
    using System.Linq;

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
    }
}