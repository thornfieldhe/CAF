using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CAF.Web.WebForm.Startup))]
namespace CAF.Web.WebForm
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
//            ConfigureAuth(app);
        }
    }
}
