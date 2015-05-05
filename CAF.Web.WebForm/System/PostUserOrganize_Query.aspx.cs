using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using FineUI;

    public partial class PostUserOrganize_Query : BasePage
    {

        #region 系统生成

        protected override void Bind()
        {
            //绑定查询条件

            base.Bind();
            this.grid_OnQuery();
            this.btnNew.OnClientClick = this.winEdit.GetShowReference("PostUserOrganize_Edit.aspx", "新增");
        }

        protected void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var criteria = new ReadOnlyPostUserOrganize();
            this.grid.BindDataSource(criteria);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("113c95be-7c7f-4e7c-8f68-dc03c4833fd2");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected void btnDeleteRows_Click(object sender, EventArgs e)
        {
            this.grid.Delete<PostUserOrganize>();
        }

        protected void gridRowCommand(object sender, GridCommandEventArgs e)
        {
            this.grid.Excute<PostUserOrganize>(e);
        }

        #endregion
    }
}
