using System;
using System.Collections.Generic;
using System.Reflection;

namespace CAF.Core
{
    /// <summary>
    /// 帮助客户内心和客户程序获取其Attribute定义中需要的抽象类型实例
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        /// 获取某个类型包括指定属性的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IList<T> GetCustomAttributes<T>(Type type) where T : Attribute
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            T[] attributes = (T[])(type.GetCustomAttributes(typeof(T), false));
            return (attributes.Length == 0) ? null : new List<T>(attributes);
        }

        /// <summary>
        /// 获取某个类型包括制定属性的所有方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IList<MethodInfo> GetMethodsWithCustomAttribute<T>(Type type) where T : Attribute
        {
            if (type==null)
            {
                throw new ArgumentNullException("type");  
            }
            MethodInfo[] methods = type.GetMethods();
            if ((methods==null)||(methods.Length==0))
            {
                return null;
            }
            IList<MethodInfo> result = new List<MethodInfo>();
            foreach (MethodInfo method in methods)
            {
                if (method.IsDefined(typeof(T),false))
                {
                    result.Add(method);
                }
            }
            return result.Count == 0 ? null : result;
        }

        /// <summary>
        /// 获取某个方法指定类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static IList<T> GetMethodCustomAttributes<T>(MethodInfo method) where T : Attribute
        {
            if (method==null)
            {
                throw new ArgumentNullException("method");
            }
            T[] attributes = (T[])(method.GetCustomAttributes(typeof(T), false));
            return (attributes.Length == 0) ? null : new List<T>(attributes);
        }

        /// <summary>
        /// 获取某个方法制定类型的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static T GetMethodCustomAttribute<T>(MethodInfo method) where T : Attribute
        {
            IList<T> attributes = GetMethodCustomAttributes<T>(method);
            return (attributes == null) ? null : attributes[0];
        }

        /// <summary>
        /// 根据枚举实例的值获取该枚举的显示名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <returns>枚举显示名称</returns>
        public static string GetAttributeName<T>(string value)
        {
            try
            {
                T cStatus = (T)Enum.Parse(typeof(T), value);
                Type enumType = typeof(T);

                DisplayNameAttribute att = enumType.GetField(cStatus.ToString()).GetCustomAttributes(typeof(DisplayNameAttribute), true)[0] as DisplayNameAttribute;
                return att.DisplayName;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
