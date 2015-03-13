using System;
using System.Linq;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;

    using FineUI;

    public partial class User_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            pageId = new Guid("5387D9C6-4056-4186-9A86-169DA2D2283A");
            base.OnLoad(e);
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            submitForm.OnPostCreated += submitForm_OnPostExcute;
            submitForm.OnPostDelete += submitForm_OnPostExcute;
            submitForm.OnPostUpdated += submitForm_OnPostExcute;
            submitForm.OnPreCreated += submitForm_OnPreCreated;
        }

        private bool submitForm_OnPreCreated(IBusinessBase business)
        {
            if (!Model.User.Exists(txtLoginName.Text))
            {
                return true;
            }
            Alert.ShowInTop("用户已存在");
            return false;
        }

        private void submitForm_OnPostExcute(IBusinessBase business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindOrganizes(new Guid(), dropOrganizeId, new Guid().ToString());
            PageTools.BindRadioButton(typeof(UserStatusEnum), radioStatus);
            PageHelper.BindRoles(chkUserRoles);
            var u = Model.User.Get(Id);
            if (u == null)
            {
                btnDelete.Hidden = true;
                btnUpdate.Hidden = true;
            }
            else
            {
                btnAdd.Hidden = true;
                txtLoginName.Enabled = false;
                submitForm.LoadEntity(u);
            }
            foreach (var item in chkUserRoles.Items.Where(item => true).Where(item => u.Roles.Count(r => r.Id == new Guid(item.Value)) > 0))
            {
                item.Selected = true;
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var item = Model.User.Get(txtId.Text.ToGuid());
            submitForm.Delete(item);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var item = Model.User.Get(txtId.Text.ToGuid());
            var ids = chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var id in ids)
            {
                item.Roles.Add(Role.Get(id));
            }
            submitForm.Update(item);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new User();
            var ids = chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var id in ids)
            {
                item.Roles.Add(Role.Get(id));
            }
            submitForm.Create(item);
        }
    }
}