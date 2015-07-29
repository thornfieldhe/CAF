using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAF.SPA.Web.Areas.Manage.Controllers
{

    using CAF.SPA.Web.Common;
    using CAF.SPA.Web.Models;

    using global::System.Threading.Tasks;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    public class UserController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;



        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this._signInManager ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                this._signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                this._userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Manage
        public ActionResult Login()
        {
            return this.View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                var error = this.ModelState.Values.SelectMany(r => r.Errors).First();
                return this.Json(new ActionResultData<string>(10, error.ErrorMessage));
            }


            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await this.SignInManager.PasswordSignInAsync(model.PhoneNumber, model.Password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.Json(new ActionResultData<string>(returnUrl ?? "/Manage/Home/Dashboard"));
                default:
                    return this.Json(new ActionResultData<string>(100, "用户名或密码错误"));
            }
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            this.AuthenticationManager.SignOut();
            return this.RedirectToAction("Login", "User", "Manage");
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.PhoneNumber };
                var result = await this.UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);
                    return this.Json(new ActionResultData<string>("/Manage/Home/Dashboard"));
                }
            }
            var error = this.ModelState.Values.SelectMany(r => r.Errors).First();
            return this.Json(new ActionResultData<string>(10, error.ErrorMessage));
        }
    }
}