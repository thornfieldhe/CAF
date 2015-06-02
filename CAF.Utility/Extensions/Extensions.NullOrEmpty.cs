using System;

namespace CAF
{
    public partial class Extensions
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T @this) { return @this == null; }

        /// <summary>
        /// 是否不为空
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T @this) { return @this != null; }

        /// <summary>
        /// 检测空值,为null则抛出ArgumentNullException异常
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="parameterName">参数名</param>
        public static void CheckNull(this object obj, string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// String是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// GUID是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid? value)
        {
            return value == null || IsEmpty(value.Value);
        }

        /// <summary>
        /// GUID是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        /// <summary>
        /// 不为空执行委托方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="func"></param>
        public static void IfNotNull<T>(this T @this, Action<T> func) { if (@this != null) { func(@this); } }

        /// <summary>
        /// 为空执行委托方法
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void IfNull<T>(this T @this, Action action) { if (@this == null) { action(); } }

        /// <summary>
        /// 为真执行委托方法
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static bool IfTrue(this bool @this, Action action)
        {
            if (@this)
                action();
            return @this;
        }

        /// <summary>
        /// 如果为假，执行委托方法
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static bool IfFalse(this bool @this, Action action)
        {
            if (!@this)
                action();
            return @this;
        }

        /// <summary>
        /// 如果为真，返回泛型默认值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static TResult WhenTrue<TResult>(this bool @this, TResult content)
        {
            return @this ? content : default(TResult);
        }
        /// <summary>
        ///如果结果为假，执行指定委托方法
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static TResult WhenFalse<TResult>(this bool @this, Func<TResult> exp)
        {
            return !@this ? exp() : default(TResult);
        }
        /// <summary>
        /// 如果为假，返回泛型默认值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static TResult WhenFalse<TResult>(this bool @this, TResult content)
        {
            return !@this ? content : default(TResult);
        }

        /// <summary>
        /// 判断是否为泛型类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool Is<T>(this T @this)
        {
            return @this is T;
        }
        /// <summary>
        /// 对象安全转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T As<T>(this T @this) where T : class
        {
            return @this as T;
        }
        /// <summary>
        /// 锁定对象后执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static T Lock<T>(this T @this, Action<T> action)
        {
            var lockObject = new object();
            lock (lockObject)
            {
                action(@this);
            }
            return @this;
        }
        /// <summary>
        /// 当前值是否在范围中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static bool Between<T>(this T @this, T lower, T upper) where T : IComparable<T> {
            return @this.CompareTo(lower) >= 0 && @this.CompareTo(upper) < 0;
        }

        /// <summary>
        /// 使用lambda表达式更新对象属性.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void Set<T>(this T @this, Action<T> action) { action(@this); }
        /// <summary>
        /// 安全读取对象属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TResult SafeValue<T, TResult>(this T @this, Func<T, TResult> action)
        {
            if (@this == null)
            {
                return default(TResult);
            }
            else
            {
                try
                {
                    return action(@this);
                }
                catch
                {
                    return default(TResult);
                }
            }
        }

        /// <summary>
        /// 安全创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T SafeValue<T>(this T? @this) where T : struct
        {
            return @this ?? default(T);
        }

        /// <summary>
        /// 安全创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T SafeValue<T>(this T @this) where T : class,new()
        {
            return new T();
        }

        /// <summary>
        /// 通过表达式返回对象，对象如果为空则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="this"></param>
        /// <param name="exp"></param>
        /// <param name="elseValue"></param>
        /// <returns></returns>
        public static TReturn NullOr<T, TReturn>(this T @this, Func<T, TReturn> exp, TReturn elseValue = default(TReturn)) where T : class
        {
            return @this != null ? exp(@this) : elseValue;
        }
    }
}
