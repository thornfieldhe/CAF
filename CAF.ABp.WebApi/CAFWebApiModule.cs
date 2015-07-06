using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.ABp.WebApi
{
    using System.Reflection;

    using CAF.Abp.Application;
    using CAF.Abp.Application.Configuration;

    using global::Abp.Modules;
    using CAF.Abp.Core;

    using global::Abp.AutoMapper;

    [DependsOn(typeof(CAFCoreModule), typeof(AbpAutoMapperModule))]
    public class ModuleZeroSampleProjectApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            this.Configuration.Authorization.Providers.Add<CAFAuthorizationProvider>();
            this.Configuration.Settings.Providers.Add<MySettingProvider>();
        }
    }
}
