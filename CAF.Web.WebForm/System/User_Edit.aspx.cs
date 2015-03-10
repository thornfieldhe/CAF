using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Security;
    using CAF.Web.WebForm.Common;

    using FineUI;

    public partial class User_Edit : EditableBase
    {
        protected override void OnLoad(EventArgs e)
        {
            pageId = new Guid("5387D9C6-4056-4186-9A86-169DA2D2283A");
            base.OnLoad(e);
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindOrganizes(new Guid(), dropDepId,new Guid().ToString());
            PageTools.BindRadioButton(typeof(UserStatusEnum), radioStatus);
            PageHelper.BindRoles(chkUserRoles);
            var u = Model.User.Get(Id);
            if (u == null)
            {
                return;
            }
            txtLoginName.Enabled = false;
            txtId.Text = u.Id.ToString();
            PageTools.BindControls(this.submitForm, u);
            foreach (var item in chkUserRoles.Items.Where(item => true).Where(item => u.Roles.Count(r => r.Id == new Guid(item.Value)) > 0))
            {
                item.Selected = true;
            }
            btnAdd.Visible = false;
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
        }

        protected override string Update()
        {
            var user = Model.User.Get(new Guid(txtId.Text));

            PageTools.BindModel(this.Page, user);
            user.Abb = txtName.Text.GetChineseSpell();
            if (txtUserPassword.Text != "")
            {
                user.Pass = Password.DesEncrypt(txtUserPassword.Text);
            }
            var ids = chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var item in ids)
            {
                user.Roles.Add(Role.Get(item));
            }
            user.Save();
            return user.Errors.Count > 0 ? user.Errors[0] : "";
        }

        protected override string Add()
        {
            if (Model.User.Exists(txtLoginName.Text.Trim()))
            {
                return "用户已存在！";
            }
            else
            {
                Model.User user =new Model.User();
                PageTools.BindModel(this.Page, user);
                user.Abb = txtName.Text.GetChineseSpell();
                user.Pass = Password.DesEncrypt(txtUserPassword.Text);
                var ids = chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
                foreach (var item in ids)
                {
                    user.Roles.Add(Role.Get(item));
                }  
                user.Create();
                if (user.Errors.Count>0)
                {
                  return user.Errors[0];
                }
                return "";
            }
        }

        protected override string Delete()
        {
            return Model.User.Delete(Id).ToString();
        }
    }
}