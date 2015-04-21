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
            this.pageId = new Guid("5387D9C6-4056-4186-9A86-169DA2D2283A");
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            this.submitForm.OnPostCreated += this.submitForm_OnPostExcute;
            this.submitForm.OnPostDelete += this.submitForm_OnPostExcute;
            this.submitForm.OnPostUpdated += this.submitForm_OnPostExcute;
            this.submitForm.OnPreCreated += this.submitForm_OnPreCreated;
            string a = "xx_ff";
            var b = a.Substring(a.IndexOf("_"), a.Length - a.IndexOf("_"));
        }

        private bool submitForm_OnPreCreated(IBusinessBase business)
        {
         
            if (!Model.User.Exists(this.txtLoginName.Text))
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
            PageHelper.BindOrganizes(new Guid(), this.dropOrganizeId, new Guid().ToString());
            PageTools.BindRadioButton(typeof(UserStatusEnum), this.radioStatus);
            PageHelper.BindRoles(this.chkUserRoles);
            var u = Model.User.Get(this.Id);
            if (u == null)
            {
                this.btnDelete.Hidden = true;
                this.btnUpdate.Hidden = true;
            }
            else
            {
                this.btnAdd.Hidden = true;
                this.txtLoginName.Enabled = false;
                this.submitForm.LoadEntity(u);
            }
            foreach (var item in this.chkUserRoles.Items.Where(item => true).Where(item => u.Roles.Count(r => r.Id == new Guid(item.Value)) > 0))
            {
                item.Selected = true;
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
            var ids = this.chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var id in ids)
            {
                item.Roles.Add(Role.Get(id));
            }
            this.submitForm.Update(item);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new User();
            var ids = this.chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var id in ids)
            {
                item.Roles.Add(Role.Get(id));
            }
            this.submitForm.Create(item);
        }
    }
}