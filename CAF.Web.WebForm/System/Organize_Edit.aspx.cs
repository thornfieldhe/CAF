using System;
// ReSharper disable All

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;
    using FineUI;


    public partial class Organize_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                pageId = new Guid("7405F6D8-3D7A-48E8-BC47-1169CE40AC4E");
            }
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
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
            PageHelper.BindOrganizes(this.Id, dropParentId, this.ID.ToString());
            var item = Organize.Get(this.Id);
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

        protected override void Add()
        {
            var item = new Organize();
            submitForm.Create(item);
        }

        protected override void Update()
        {
            var item = Organize.Get(this.Id);
            submitForm.Update(item);
        }

        protected override void Delete()
        {
            var item = Organize.Get(this.Id);
            submitForm.Delete(item);
        }

    }
}