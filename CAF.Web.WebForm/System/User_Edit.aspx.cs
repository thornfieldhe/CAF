using System;
using System.Linq;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Security;
    using CAF.Web.WebForm.Common;

    using FineUI;

    public partial class User_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("5387D9C6-4056-4186-9A86-169DA2D2283A");
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            this.submitForm.OnPostCreated += this.submitForm_OnPostExcute;
            this.submitForm.OnPostDelete += this.submitForm_OnPostExcute;
            this.submitForm.OnPostUpdated += this.submitForm_OnPostExcute;
            this.submitForm.OnPreCreated += this.submitForm_OnPreCreated;
            this.submitForm.OnPreUpdated += this.submitForm_OnPreUpdated;
        }

        private bool submitForm_OnPreCreated(IBusinessBase business)
        {
            var item = business as Model.User;
            if (item == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.txtPass.Text.Trim()))
            {
                Alert.ShowInTop("密码不允许为空！");
                this.txtPass.Focus();
                return false;
            }
            if (this.txtPass.Text.Trim() != this.txtConfirmPass.Text.Trim())
            {
                Alert.ShowInTop("密码两次输入不一致！");
                return false;
            }
            if (Model.User.Exists(this.txtLoginName.Text))
            {
                Alert.ShowInTop("用户已存在");
                return false;
            }
            item.Pass = Password.DesEncrypt(item.Pass);
            var ids = this.chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var id in ids)
            {
                item.Roles.Add(Role.Get(id));
            }
            return true;
        }

        private bool submitForm_OnPreUpdated(IBusinessBase business)
        {

            var item = business as Model.User;
            if (item == null)
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(this.txtPass.Text.Trim()))
            {
                if (this.txtPass.Text.Trim() != this.txtConfirmPass.Text.Trim())
                {
                    Alert.ShowInTop("密码两次输入不一致！");
                    return false;
                }
                else
                {
                    item.Pass = Password.DesEncrypt(item.Pass);
                }
            }
            else
            {
                item.SkipProperties("Pass");
            }

            var ids = this.chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var id in ids)
            {
                item.Roles.Add(Role.Get(id));
            }
            return true;
        }

        private void submitForm_OnPostExcute(IBusinessBase business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindOrganizes(new Guid(), this.dropOrganizeId, new Guid().ToString(), true);
            PageTools.BindRadioButton(typeof(UserStatusEnum), this.radioStatus);
            PageHelper.BindRoles(this.chkUserRoles);
            var item = Model.User.Get(this.Id);
            if (item == null)
            {
                this.btnDelete.Hidden = true;
                this.btnUpdate.Hidden = true;
            }
            else
            {
                this.btnAdd.Hidden = true;
                this.txtLoginName.Readonly = true;
                this.submitForm.LoadEntity(item);
                foreach (var role in this.chkUserRoles.Items.Where(ur => item.Roles.Count(r => r.Id == new Guid(ur.Value)) > 0))
                {
                    role.Selected = true;
                }
                this.txtPass.Text = "";
                this.txtConfirmPass.Text = "";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var item = Model.User.Get(this.Id);
            this.submitForm.Delete(item);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var item = Model.User.Get(this.Id);
            this.submitForm.Update(item);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new User();
            this.submitForm.Create(item);
        }
    }
}