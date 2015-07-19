using System;
namespace CAF.Web.WebForm
{
    using FineUI;

    public partial class ErrorLog_Details : BasePage
    {
        protected override void Bind()
        {
            var item = ErrorLog.Get(this.Id);
            if (item != null)
            {
                this.submitForm.LoadEntity(item);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.pageId = new Guid("acaf1349-b79b-489a-a32c-25c16e6d8093");
            }
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
        }

    }
}
