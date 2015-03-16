using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;

    public partial class Role_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                pageId = new Guid("FD4D499B-3667-4AA1-8120-89FEEF28AB59");
            }
            base.OnLoad(e);
            submitForm.OnPostCreated += submitForm_OnPostExcute;
            submitForm.OnPostDelete += submitForm_OnPostExcute;
            submitForm.OnPostUpdated += submitForm_OnPostExcute;
        }

        private void submitForm_OnPostExcute(IBusinessBase business)
        {
            Initialization();
        }

        protected override void Bind()
        {
            base.Bind();
            PageTools.BindDropdownList(Role.GetSimpleRoleList(), dropRoles, Guid.Empty.ToString());

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var item = Role.Get(dropRoles.SelectedValue.ToGuid());
            submitForm.Delete(item);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var item = Role.Get(dropRoles.SelectedValue.ToGuid());
            submitForm.Update(item);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new Role();
            submitForm.Create(item);
        }

        protected void dropRolesId_SelectedIndexChanged(object sender, EventArgs e)
        {
            var role = Role.Get(dropRoles.SelectedValue.ToGuid());
            if (role != null)
            {
                submitForm.LoadEntity(role);
            }
        }
    }
}