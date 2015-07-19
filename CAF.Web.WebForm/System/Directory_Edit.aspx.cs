using System;
namespace CAF.Web.WebForm
{
    using CAF.Web.WebForm.Common;
    using FineUI;

    public partial class Directory_Edit : BasePage
    {
        protected override void Bind()
        {
            //绑定查询条件
            PageHelper.BindDirectories(this.txtId.Text.ToGuid(), this.dropParentId, new Guid().ToString());
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

        #region 系统生成
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("f66d4ee2-8c93-47bd-83bf-550cab2025da");
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

        protected override void Delete()
        {
            var item = Directory.Get(this.txtId.Text.ToGuid());
            this.submitForm.Delete(item); base.Delete();
        }

        protected override void Update()
        {
            var item = Directory.Get(this.txtId.Text.ToGuid());
            this.submitForm.Update(item);
        }

        protected override void Add()
        {
            var item = new Directory();
            this.submitForm.Create(item); base.Add();
        }

        #endregion
    }
}
