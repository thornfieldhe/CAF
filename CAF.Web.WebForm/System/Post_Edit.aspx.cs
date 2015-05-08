using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;

    public partial class Post_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.pageId = new Guid("f2390f35-f438-4bb4-b753-eaa02e66b9c0");
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
            PageTools.BindDropdownList(Post.GetSimpleRoleList(), this.dropPosts, Guid.Empty.ToString());

        }

        protected override void Delete()
        {
            var item = Post.Get(this.dropPosts.SelectedValue.ToGuid());
            this.submitForm.Delete(item);
        }

        protected override void Update()
        {
            var item = Post.Get(this.dropPosts.SelectedValue.ToGuid());
            this.submitForm.Update(item);
        }

        protected override void Add()
        {
            var item = new Post();
            this.submitForm.Create(item);
        }


        protected void dropPostsId_SelectedIndexChanged(object sender, EventArgs e)
        {
            var post = Post.Get(this.dropPosts.SelectedValue.ToGuid());
            if (post != null)
            {
                this.submitForm.LoadEntity(post);
            }
        }
    }
}