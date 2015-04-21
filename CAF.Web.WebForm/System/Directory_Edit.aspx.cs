using System;
namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;
    using FineUI;

    public partial class Directory_Edit : BasePage
    {
        protected override void Bind()
        {
            //绑定查询条件
            PageHelper.BindDirectories(this.txtId.Text.ToGuid(), this.dropParentId, selectItem: new Guid().ToString());
            var item = Directory.Get(this.Id);
            if (item == null)
            {
                this.btnDelete.Hidden = true;
                this.btnUpdate.Hidden = true;
            }
            else
            {
                this.btnAdd.Hidden = true;
                this.txtId.Readonly = true;
                this.submitForm.LoadEntity(item);
            }
        }

        #region 系统事件
        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.pageId = new Guid("5043a6f2-f7dc-4492-a611-154e2b0d5810");
            }
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            this.submitForm.OnPostCreated += this.submitForm_OnPostExcute;
            this.submitForm.OnPostDelete += this.submitForm_OnPostExcute;
            this.submitForm.OnPostUpdated += this.submitForm_OnPostExcute;
        }

        protected void submitForm_OnPostExcute(IBusinessBase business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var item = Directory.Get(this.txtId.Text.ToGuid());
            this.submitForm.Delete(item);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var item = Directory.Get(this.txtId.Text.ToGuid());
            this.submitForm.Update(item);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new Directory();
            this.submitForm.Create(item);
        }
        #endregion
    }
}
