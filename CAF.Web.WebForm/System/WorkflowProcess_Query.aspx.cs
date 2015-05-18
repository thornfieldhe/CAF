using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using FineUI;

    public partial class WorkflowProcess_Query : BasePage
    {

        #region 系统生成

        protected override void Bind()
        {
            //绑定查询条件

            base.Bind();
            this.grid_OnQuery();
        }

        protected void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            const string where = " 1=1";
            this.grid.BindDataSource<WorkflowProcess>(null,where:where);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("3edd4d93-1720-4810-9a0b-76e3a804b459");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected override void Delete()
        {
            this.grid.Delete<WorkflowProcess>();
        }

        protected void gridRowCommand(object sender, GridCommandEventArgs e)
        {
            this.grid.Excute<WorkflowProcess>(e);
        }

        #endregion
    }
}
