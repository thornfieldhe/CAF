using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CAF.SPA.Web.Startup))]
namespace CAF.SPA.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
