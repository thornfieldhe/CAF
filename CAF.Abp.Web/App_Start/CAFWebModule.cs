using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAF.Abp.Web.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using CAF.Abp.Application;

    using global::Abp.Modules;
    using CAF.Abp.EF;

    [DependsOn(typeof(CAFDataModule), typeof(CAFApplicationModule), typeof(CAFWebApiModule))]
    public class CAFWebApiModule : AbpModule
    {
        public override void PreInitialize()
        {
            //            //Add/remove languages for your application
            //            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england", true));
            //            Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flag-tr"));
            //
            //            //Add/remove localization sources here
            //            Configuration.Localization.Sources.Add(
            //                new XmlLocalizationSource(
            //                    ModuleZeroSampleProjectConsts.LocalizationSourceName,
            //                    HttpContext.Current.Server.MapPath("~/Localization/ModuleZeroSampleProject")
            //                    )
            //                );
            //
            //            //Configure navigation/menu
            //            Configuration.Navigation.Providers.Add<ModuleZeroSampleProjectNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}