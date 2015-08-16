//
//namespace CAF.Tests.Domains.Validations
//{
//
//    using Autofac;
//
//    using CAF.Validations;
//
//
//
//    /// <summary>
//    /// 依赖注入配置
//    /// </summary>
//    public class IocConfig : CAF.DI.ConfigBase
//    {
//        /// <summary>
//        /// 加载配置
//        /// </summary>
//        /// <param name="builder">容器生成器</param>
//        protected override void Load(ContainerBuilder builder)
//        {
//            base.Load(builder);
//            builder.RegisterType<Validation>().As<IValidation>();
//            builder.RegisterType<ValidationHandler>().As<IValidationHandler>();
//        }
//    }
//}
