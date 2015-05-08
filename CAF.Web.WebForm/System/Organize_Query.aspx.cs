using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using FineUI;

    public partial class Organize_Query : BasePage
    {
        protected override void Bind()
        {
            //绑定查询条件


            base.Bind();
            this.grid_OnQuery();
            this.btnNew.OnClientClick = this.winEdit.GetShowReference("Organize_Edit.aspx", "新增");
        }

        protected void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var criteria = new ReadOnlyOrganize();
            this.grid.BindDataSource(criteria);
        }

        #region 系统事件

        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("ff28f6a4-aeca-4fa7-9565-080ded408be3");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected override void Delete()
        {
            this.grid.Delete<Organize>();
        }

        protected void gridRowCommand(object sender, GridCommandEventArgs e)
        {
            this.grid.Excute<Organize>(e);
        }

        #endregion
    }
}
