using System;

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
        /// <param name="allowSpace"></param>
        public static void BindDirs(Guid id, DropDownList drop)
        {
            if (drop.Items.Count != 0)
            {
                return;
            }
            drop.Items.Clear();
            var item = new ListItem { Text = "请选择", Value = new Guid().ToString() };
            //            if (!drop.Items.Contains(item, new ListItemCompare()))
            //            {
            drop.Items.Add(item);
            //            }
            BindChildDirs(id, "", ref drop);
        }

        private static void BindChildDirs(Guid parentId, string space, ref DropDownList drop)
        {
            var dirs = Directory.GetAllByParentId(parentId);

            foreach (var item in dirs)
            {
                var i = new ListItem { Text = space + item.Name, Value = item.Id.ToString() };
                //                if (!drop.Items.Contains(i, new ListItemCompare()))
                //                {
                drop.Items.Add(i);
                //                }

                BindChildDirs(item.Id, space + "------", ref drop);
            }
        }
    }


}