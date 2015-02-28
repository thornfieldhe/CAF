using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAF.Web.WebForm.Common
{
    using FineUI;

    using Newtonsoft.Json.Linq;

    public abstract class EditebleListBase : ListBase
    {
        protected override void Bind()
        {
            base.Bind();
//            grid.AllowCellEditing = true;
//            grid.ClicksToEdit = 1;
        }

        // 删除选中行的脚本
        protected string GetDeleteScript(Grid grid)
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, grid.GetDeleteSelectedReference(), String.Empty);
        }

        protected void BuildDeleteButton(JObject defaultObj, Grid grid)
        {
            // 删除选中单元格的客户端脚本
            defaultObj.Add("Delete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",
            GetDeleteScript(grid), IconHelper.GetResolvedIconUrl(Icon.Delete)));
        }

        protected void grid_PreDataBound(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            // 设置LinkButtonField的点击客户端事件
            LinkButtonField deleteField = grid.FindColumn("Delete") as LinkButtonField;
            deleteField.OnClientClick = GetDeleteScript(grid);
        }

        protected override void BindScripts()
        {
            // 重置表格
            if (btnReset != null)
            {
                //btnReset.OnClientClick = grid.GetRejectChangesReference();
            }
        }

        protected virtual void grid_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
//                int rowID = Convert.ToInt32(grid.DataKeys[e.RowIndex][0]);
//                DeleteRowByID(rowID);
//
//                Query();
//
//                Alert.ShowInTop("删除数据成功!");
            }
        }

        protected virtual void DeleteRowByID(int rowId) { }

        #region 包含控件

        protected global::FineUI.Button btnSubmit;
        protected global::FineUI.Button btnReset;

        #endregion 包含控件
    }
}