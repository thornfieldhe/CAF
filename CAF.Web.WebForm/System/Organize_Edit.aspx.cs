using System;
// ReSharper disable All

namespace CAF.Web.WebForm
{
    using CAF.Ext;
    using CAF.Model;
    using CAF.Web.WebForm.CAFControl;
    using CAF.Web.WebForm.Common;
    using FineUI;

    using global::System.Linq;

    public partial class Organize_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                pageId = new Guid("7405F6D8-3D7A-48E8-BC47-1169CE40AC4E");
            }
            base.OnLoad(e);
            submitForm.OnPostCreated += submitForm_OnPostExcute;
            submitForm.OnPostDelete += submitForm_OnPostExcute;
            submitForm.OnPostUpdated += submitForm_OnPostExcute;
        }

        private void submitForm_OnPostExcute(IBusinessBase business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindOrganizes(txtId.Text.ToGuid(), dropParentId, txtId.Text);
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new Organize();
            submitForm.Create(item);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var item = Organize.Get(txtId.Text.ToGuid());
            submitForm.Update(item);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var item = Organize.Get(txtId.Text.ToGuid());
            submitForm.Delete(item);
        }
    }
}