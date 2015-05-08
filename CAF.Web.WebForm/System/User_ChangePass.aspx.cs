using System;

namespace CAF.Web.WebForm.System
{
    using CAF.Security;

    using FineUI;


    public partial class User_ChangePass : BasePage
    {
        #region 系统生成

        protected override void Bind()
        {
            var item = Model.User.Get(this.Id);
            this.submitForm.LoadEntity(item);

        }


        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.pageId = new Guid("f3787cb4-c4c2-47e4-8e9c-ae625dbfa821");
            }
            base.OnLoad(e);
            this.btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
            this.submitForm.OnPostUpdated += this.submitForm_OnPostExcute;
            this.submitForm.OnPreUpdated += this.submitForm_OnPreUpdated;
        }

        bool submitForm_OnPreUpdated(IBusinessBase business)
        {
            var user = business as Model.User;
            if (this.txtOldPassword.Text.Trim() == string.Empty)
            {
                Alert.ShowInTop("原密码不允许为空！");
                return false;
            }
            else if (Password.DesEncrypt(this.txtOldPassword.Text.Trim()) != user.Pass)
            {
                Alert.ShowInTop("原密码错误！");
                return false;
            }
            if (this.txtNewPass.Text.Trim() == this.txtConfirmPassword.Text.Trim())
            {
                return true;
            }
            Alert.ShowInTop("密码两次输入不一致！");
            return false;
        }

        protected void submitForm_OnPostExcute(IBusinessBase business)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void Update()
        {
            var item = Model.User.Get(this.Id);
            this.submitForm.Update(item);
        }

        #endregion
    }
}