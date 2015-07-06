using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CAF.Web.WebForm.Startup))]
namespace CAF.Web.WebForm
{
    using Autofac;

    using CAF.DI;
    using CAF.Validations;

    public partial class Startup {
        public void Configuration(IAppBuilder app) {
//            ConfigureAuth(app);
            Ioc.Register(new IocConfig());
        }
    }

    /// <summary>
    /// 依赖注入配置
    /// </summary>
    public class IocConfig : CAF.DI.ConfigBase
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="builder">容器生成器</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<Validation>().As<IValidation>();
            builder.RegisterType<ValidationHandler>().As<IValidationHandler>();
        }
    }
}
