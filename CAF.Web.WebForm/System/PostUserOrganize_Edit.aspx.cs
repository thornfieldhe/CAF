using System;
namespace CAF.Web.WebForm
{
    using CAF.Web.WebForm.Common;
    using FineUI;

    public partial class PostUserOrganize_Edit : BasePage
    {
        protected void dropRolesId_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageHelper.BindPostUsers(this.dropPostId.SelectedValue.ToGuid(), this.dropUserId, selectItem: new Guid().ToString());
        }


        #region 系统生成

        protected override void Bind()
        {
            //绑定查询条件
            PageHelper.BindPosts(this.dropPostId, selectItem: new Guid().ToString());
            PageHelper.BindPostUsers(Guid.Empty, this.dropUserId, selectItem: new Guid().ToString());
            PageHelper.BindOrganizes(Guid.Empty, this.dropOrganizeId, selectItem: new Guid().ToString());

            var item = PostUserOrganize.Get(this.Id);
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

        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("8f9165fa-cdcd-46ee-a2f3-879bea35e614");
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            this.submitForm.OnPostCreated += this.submitForm_OnPostExcute;
            this.submitForm.OnPostDelete += this.submitForm_OnPostExcute;
            this.submitForm.OnPostUpdated += this.submitForm_OnPostExcute;
        }

        protected void submitForm_OnPostExcute(IBusinessBase business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void Delete()
        {
            var item = PostUserOrganize.Get(this.Id);
            this.submitForm.Delete(item);
        }

        protected override void Update()
        {
            var item = PostUserOrganize.Get(this.Id);
            this.submitForm.Update(item);
        }

        protected override void Add()
        {
            var item = new PostUserOrganize();
            this.submitForm.Create(item);
        }

        #endregion
    }
}
