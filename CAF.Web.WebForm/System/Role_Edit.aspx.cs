using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;

    public partial class Role_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.pageId = new Guid("FD4D499B-3667-4AA1-8120-89FEEF28AB59");
            }
            base.OnLoad(e);
            this.submitForm.OnPostCreated += this.submitForm_OnPostExcute;
            this.submitForm.OnPostDelete += this.submitForm_OnPostExcute;
            this.submitForm.OnPostUpdated += this.submitForm_OnPostExcute;
        }

        private void submitForm_OnPostExcute(IBusinessBase business)
        {
            this.Initialization();
        }

        protected override void Bind()
        {
            base.Bind();
            PageTools.BindDropdownList(Role.GetSimpleRoleList(), this.dropRoles, Guid.Empty.ToString());

        }

        protected override void Delete()
        {
            var item = Role.Get(this.dropRoles.SelectedValue.ToGuid());
            this.submitForm.Delete(item);
        }

        protected override void Update()
        {
            var item = Role.Get(this.dropRoles.SelectedValue.ToGuid());
            this.submitForm.Update(item);
        }

        protected override void Add()
        {
            var item = new Role();
            this.submitForm.Create(item);
        }

        protected void dropRolesId_SelectedIndexChanged(object sender, EventArgs e)
        {
            var role = Role.Get(this.dropRoles.SelectedValue.ToGuid());
            if (role != null)
            {
                this.submitForm.LoadEntity(role);
            }
        }
    }
}