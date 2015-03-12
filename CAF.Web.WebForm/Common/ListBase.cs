using System;

namespace CAF.Web.WebForm
{

    using CAF.Model;

    using FineUI;


    public class ListBase : BasePage
    {


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
            if (deleteField != null)
            {
                deleteField.OnClientClick = GetDeleteScript(grid.GetDeleteSelectedReference());
            }
        }

    }
}