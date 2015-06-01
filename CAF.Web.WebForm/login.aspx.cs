using FineUI;
using System;
using System.Web;
using System.Web.UI;

namespace CAF.Web
{
    using CAF.Model;
    using CAF.Utility;
    using CAF.Web.WebForm.Common;
    using CAF.Webs;

    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                return;
            }
            this.IsAuthenticated();
            this.LoadData();
        }

        private void LoadData()
        {
            this.InitCaptchaCode();
        }

        private void IsAuthenticated()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.btnLogin.OnClientClick = Confirm.GetShowReference(
                    string.Format("用户[{0}]已在本机登录，是否强制登录？", CAF.Model.User.Get(this.User.Identity.Name).Name), String.Empty, MessageBoxIcon.Question, this.btnLogin.GetPostBackEventReference(), String.Empty);
                this.btnLogin.EnablePostBack = false;
            }
            else
            {
                this.btnLogin.EnablePostBack = true;
            }
        }

        /// <summary>
        /// 初始化验证码
        /// </summary>
        private void InitCaptchaCode()
        {
            // 创建一个 6 位的随机数并保存在 Session 对象中
            this.Session["CaptchaImageText"] = PageTools.GenerateRandomCode();
            this.imgCaptcha.ImageUrl = "~/captcha/captcha.ashx?w=150&h=30&t=" + DateTime.Now.Ticks;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.InitCaptchaCode();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.txtCaptcha.Text != this.Session["CaptchaImageText"].ToString())
            {
                Alert.ShowInTop("验证码错误！");
                this.InitCaptchaCode();
                this.txtPassword.Text = "";
                return;
            }
            var principal = new CAFPrincipal();
            if (principal.Login(this.txtUserName.Text.Trim(), Encrypt.DesEncrypt(this.txtPassword.Text.Trim())))
            {
                HttpContext.Current.User = principal;
                System.Web.Security.FormsAuthentication.SetAuthCookie(this.txtUserName.Text, true);
                var referrer = this.Request["referrer"];
                HttpContext.Current.Session["Principal"] = HttpContext.Current.User;
                this.Session["User"] = CAF.Model.User.Get(this.User.Identity.Name);
                var log = new LoginLog { UserName = this.User.Identity.Name, Ip = Net.GetClientIP() };

                log.Create();
                this.Response.Redirect(referrer ?? @"Dashboard.aspx", false);
            }
            else
            {
                Alert.ShowInTop("用户名或密码错误！", MessageBoxIcon.Error);
                this.txtPassword.Text = "";
                this.InitCaptchaCode();
            }
        }

    }
}
