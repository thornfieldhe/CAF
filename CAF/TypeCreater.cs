using Microsoft.Practices.Unity;
using System;

namespace CAF
{

    /// <summary>
    /// 实例创建类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeCreater
    {
        /// <summary>
        /// 反射创建无参数实例
        /// </summary>
        /// <returns></returns>
        public static T BuildUp<T>()
        {
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 反射创建创建带参数实例
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T BuildUp<T>(object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }

        /// <summary>
        /// 根据类型名称，参数创建实例
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="args">参数列表</param>
        /// <returns></returns>
        public static T BuildUp<T>(string typeName, object[] args)
        {
            return (T)Activator.CreateInstance(Type.GetType(typeName), args);
        }

        /// <summary>
        /// 依赖注入方式创建实例
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public static T IocBuildUp<T>(string alias=null)
        {
            IUnityContainer container = new UnityContainer();
            var section = SingletonBase<CAFConfiguration>.Instance.Unity;
            section.Configure(container);
                return string.IsNullOrWhiteSpace(alias) 
                    ? container.Resolve<T>() : container.Resolve<T>(alias);
        }


        public static IUnityContainer GetContainer()
        {
            try
            {
                IUnityContainer container = new UnityContainer();
                var section = SingletonBase<CAFConfiguration>.Instance.Unity;
                section.Configure(container);

                return container;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}