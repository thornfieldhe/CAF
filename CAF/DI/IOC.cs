//using System;
//using System.Collections.Generic;
//using System.Linq;
//
//namespace CAF.DI
//{
//    using Autofac;
//    using Autofac.Core;
//    using CAF.Utility;
//    using System.Reflection;
//    using System.Text.RegularExpressions;
//
//
//    /// <summary>
//    /// 容器
//    /// </summary>
//    public static class Ioc
//    {
//        /// <summary>
//        /// 需要跳过的程序集列表
//        /// </summary>
//        private const string AssemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^NSubstitute|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Telerik|^Iesi|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";
//
//        /// <summary>
//        /// 创建实例
//        /// </summary>
//        /// <typeparam name="T">实例类型</typeparam>
//        public static T Create<T>()
//        {
//            return Container.Create<T>();
//        }
//
//        /// <summary>
//        /// 创建实例
//        /// </summary>
//        /// <param name="type">对象类型</param>
//        public static object Create(Type type)
//        {
//            return Container.Create(type);
//        }
//
//        /// <summary>
//        /// 为测试环境注册依赖
//        /// </summary>
//        /// <param name="modules">依赖配置</param>
//        public static void Register(params IModule[] modules)
//        {
//            var path = Sys.GetPhysicalPath(AppDomain.CurrentDomain.BaseDirectory);
//            var assemblies = Reflection.GetAssemblies(path);
//            Container.Init(builder => RegisterTypes(assemblies, builder), modules);
//        }
//
//        /// <summary>
//        /// 注册程序集列表中所有实现了IDependency的类型
//        /// </summary>
//        private static void RegisterTypes(IEnumerable<Assembly> assemblies, ContainerBuilder builder)
//        {
//            var typeBase = typeof(IDependency);
//            builder.RegisterAssemblyTypes(FilterSystemAssembly(assemblies))
//                .Where(t => typeBase.IsAssignableFrom(t) && t != typeBase && !t.IsAbstract)
//                .AsImplementedInterfaces().InstancePerLifetimeScope();
//        }
//
//        /// <summary>
//        /// 过滤系统程序集
//        /// </summary>
//        private static Assembly[] FilterSystemAssembly(IEnumerable<Assembly> assemblies)
//        {
//            return assemblies
//                .Where(assembly => !Regex.IsMatch(assembly.FullName, AssemblySkipLoadingPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
//                .ToArray();
//        }
//
//        /// <summary>
//        /// 为Mvc注册依赖
//        /// </summary>
//        /// <param name="mvcAssembly">mvc项目所在的程序集</param>
//        /// <param name="modules">依赖配置</param>
//        public static void RegisterMvc(Assembly mvcAssembly, params IModule[] modules)
//        {
//            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            Container.RegisterMvc(mvcAssembly, builder => RegisterTypes(assemblies, builder), modules);
//        }
//    }
//}
