using System;
namespace CAF.Web.WebForm
{
    using CAF.Model;
    using FineUI;

    public partial class Module_Edit : BasePage
    {
        #region 系统生成

        protected override void Bind()
        {
            //绑定查询条件

            var item = Module.Get(this.Id);
            if (item == null)
            {
                this.btnDelete.Hidden = true;
                this.btnUpdate.Hidden = true;
            }
            else
            {
                this.btnAdd.Hidden = true;
                this.submitForm.LoadEntity(item);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("d6dd9474-72ab-4872-8e30-2e50669b0dca");
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
            var item = Model.Module.Get(this.Id);
            this.submitForm.Delete(item);
        }

        protected override void Update()
        {
            var item = Module.Get(this.Id);
            this.submitForm.Update(item);
        }

        protected override void Add()
        {
            var item = new Module { WorkflowProcess = new WorkflowProcess { Name = this.txtName.Text.Trim(), Document = "<Workflow/>" } };
            this.submitForm.Create(item);
        }

        #endregion
    }
}
