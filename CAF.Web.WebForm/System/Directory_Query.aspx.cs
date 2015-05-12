using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;

    using FineUI;

    public partial class Directory_Query : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("514d089c-c034-4ae7-8e6f-9156ba000566");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected override void Bind()
        {
            base.Bind();
            this.grid_OnQuery();
            this.btnNew.OnClientClick = this.winEdit.GetShowReference("Directory_Edit.aspx", "新增");
        }

        protected override void Delete() { this.grid.Delete<Directory>(); }


        protected void gridRowCommand(object sender, GridCommandEventArgs e)
        {
            this.grid.Excute<Directory>(e);
        }

        private void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            this.grid.BindDataSource<ReadOnlyDirectory>(null);
        }
    }
}