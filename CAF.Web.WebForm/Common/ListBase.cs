using System;

namespace CAF.Web.WebForm
{

    using CAF.Model;

    using FineUI;


    public class ListBase : BasePage
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


        protected virtual void ExcuteQuery()  {  }

        protected override void Query()
        {
            ExcuteQuery();
            BindDataSource();
        }

        protected override void Bind()
        {
            base.Bind();
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
            ExcuteQuery();
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