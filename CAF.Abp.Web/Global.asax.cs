using System;

namespace CAF.Abp.Web
{
    using Castle.Facilities.Logging;

    using global::Abp.Dependency;
    using global::Abp.Web;

    public class MvcApplication : AbpWebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            IocManager.Instance.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            base.Application_Start(sender, e);
        }
    }
}
