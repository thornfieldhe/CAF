using System;
using System.Linq;

namespace CAF.Web.WebForm
{
    using CAF.Models;
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

        private bool submitForm_OnPreCreated(IDbAction business)
        {
            var item = business as Models.User;
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
            if (Models.User.Exist(r => r.Name.Contains(this.txtLoginName.Text.Trim()) || r.Abb.Contains(this.txtLoginName.Text.Trim())))
            {
                Alert.ShowInTop("用户已存在");
                return false;
            }
            var ids = this.chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            foreach (var id in ids)
            {
                item.Roles.Add(Role.Get(id));
            }
            return true;
        }

        private bool submitForm_OnPreUpdated(IDbAction business)
        {

            var item = business as Models.User;
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
            }

            var ids = this.chkUserRoles.SelectedValueArray.Select(d => new Guid(d)).ToList();
            var userRoles = item.Roles.Select(r => r.Id).ToList();
            userRoles.Except(ids).ToList().ForEach(i => item.Roles.Remove(item.Roles.First(r => r.Id == i)));
            ids.Except(userRoles).ToList().ForEach(i => item.Roles.Add(Role.Get(i)));

            var ids2 = this.chkUserPosts.SelectedValueArray.Select(d => new Guid(d)).ToList();
            var userPosts = item.Posts.Select(r => r.Id).ToList();
            userPosts.Except(ids2).ToList().ForEach(i => item.Posts.Remove(item.Posts.First(r => r.Id == i)));
            ids2.Except(userPosts).ToList().ForEach(i => item.Posts.Add(Post.Get(i)));


            return true;
        }

        private void submitForm_OnPostExcute(IDbAction business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindOrganizes(new Guid(), this.dropOrganizeId, new Guid().ToString(), false);
            PageTools.BindRadioButton<UserStatusEnum>(this.radioStatus);
            PageHelper.BindRoles(this.chkUserRoles);
            PageHelper.BindPosts(this.chkUserPosts);
            var item = Models.User.Get(this.Id);
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
                foreach (var post in this.chkUserPosts.Items.Where(ur => item.Posts.Count(r => r.Id == new Guid(ur.Value)) > 0))
                {
                    post.Selected = true;
                }
                this.txtPass.Text = "";
                this.txtConfirmPass.Text = "";
            }
        }

        protected override void Delete()
        {
            var item = Models.User.Get(this.Id);
            this.submitForm.Delete(item);
        }

        protected override void Update()
        {
            var item = Models.User.Get(this.Id);
            this.submitForm.Update(item);
        }

        protected override void Add()
        {
            var item = new User();
            this.submitForm.Create(item);
        }

    }
}