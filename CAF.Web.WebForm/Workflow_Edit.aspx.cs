using System;

namespace CAF.Web.WebForm.System
{
    public partial class Workflow_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("95020d1a-7e63-4a14-a019-ad6c3554ad80");
            base.OnLoad(e);
        }
    }
}