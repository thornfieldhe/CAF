using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAF.Web.WebForm
{
    using System.Text.RegularExpressions;

    using CAF.Model;

    using FineUI;

    using Newtonsoft.Json.Linq;

    public abstract class ListBase : BasePage
    {


        protected void grid_PageIndexChange(object sender, FineUI.GridPageEventArgs e)
        {
            ((Grid)sender).PageIndex = e.NewPageIndex;
            Query();
        }

        protected void grid_Sort(object sender, FineUI.GridSortEventArgs e)
        {
            Query();
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        protected virtual void BindDataSource() { }

        protected virtual void BindQueryCondition() { }

        protected virtual void ExcuteQuery()
        {

        }

        protected override void Query()
        {
            BindDataSource();
            BindQueryCondition();
            ExcuteQuery();
        }

        protected override void Bind()
        {
            base.Bind();
        }

        /// <summary>
        /// 获取指定列的汇总数据
        /// </summary>
        /// <param name="strFileds"></param>
        /// <param name="strWhereExt"></param>
        protected void OutputSummaryData(string strFileds, string strWhereExt = " And 1=1")
        {

        }

        // 删除选中行的脚本
        protected string GetDeleteScript(string okMessage)
        {
            return Confirm.GetShowReference(Resource.System_Message_ConfirmDelete, Resource.System_Action_Delete,
                MessageBoxIcon.Question, okMessage, String.Empty);
        }

        protected virtual void Grid_PreDataBound(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            // 设置LinkButtonField的点击客户端事件
            var deleteField = grid.FindColumn("Delete") as LinkButtonField;
            deleteField.OnClientClick = GetDeleteScript(grid.GetDeleteSelectedReference());
        }

        protected virtual void btnExport_Click(object sender, EventArgs e)
        {
            BindDataSource();
            BindQueryCondition();
            ExportExcel();
        }

        protected virtual void ExportExcel()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}