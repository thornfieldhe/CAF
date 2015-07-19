using System;
namespace CAF.Web.WebForm
{
    using FineUI;

    public partial class WorkflowProcess_Edit : BasePage
    {
        #region 系统生成

        protected override void Bind()
        {
            //绑定查询条件

            var item = WorkflowProcess.Get(this.Id);
            if (item == null)
            {
                this.btnDelete.Hidden = true;
                this.btnUpdate.Hidden = true;
            }
            else
            {
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
            var item = WorkflowProcess.Get(this.Id);
            this.submitForm.Delete(item);
        }

        protected override void Update()
        {
            var item = WorkflowProcess.Get(this.Id);
            this.submitForm.Update(item);
        }

        protected override void Add()
        {
            var item = new WorkflowProcess();
            this.submitForm.Create(item);
        }

        #endregion
    }
}
