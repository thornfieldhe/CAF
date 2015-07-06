using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;


namespace CAF.Abp.Application
{
    using CAF.Abp.Application;
    using CAF.Abp.Application.Configuration;
    using CAF.Abp.Core;

    [DependsOn(typeof(CAFCoreModule), typeof(AbpAutoMapperModule))]
    public class CAFApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            this.Configuration.Authorization.Providers.Add<CAFAuthorizationProvider>();
            this.Configuration.Settings.Providers.Add<MySettingProvider>();
        }
    }
}
