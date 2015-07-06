
namespace CAF.Abp.Core
{
    using global::Abp.Modules;
    using System.Reflection;

    public class CAFCoreModule : AbpModule
    {
        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
