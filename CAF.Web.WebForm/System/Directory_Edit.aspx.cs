using System;
namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;

    using FineUI;

    public partial class Directory_Edit : BasePage
    {

        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.pageId = new Guid("f66d4ee2-8c93-47bd-83bf-550cab2025da");
            }
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            this.submitForm.OnPostCreated += this.submitForm_OnPostExcute;
            this.submitForm.OnPostDelete += this.submitForm_OnPostExcute;
            this.submitForm.OnPostUpdated += this.submitForm_OnPostExcute;
        }

        private void submitForm_OnPostExcute(IBusinessBase business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void Bind()
        {
            PageHelper.BindDirectories(this.txtId.Text.ToGuid(), this.dropParentId, selectItem: Guid.Empty.ToString());


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
    }
}