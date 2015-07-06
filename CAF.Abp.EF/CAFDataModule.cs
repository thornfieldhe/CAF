using CAF.Abp.Core;
using Abp.Modules;
using Abp.Zero.EntityFramework;
namespace CAF.Abp.EF
{
    using System.Reflection;

    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(CAFCoreModule))]
    public class CAFDataModule:AbpModule
    {
        public override void PreInitialize() { this.Configuration.DefaultNameOrConnectionString = "Default"; }

        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            
        }
    }
}
