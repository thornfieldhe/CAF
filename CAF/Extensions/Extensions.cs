using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF
{
    public static partial class Extensions
    {
        /// <summary>
        /// 遍历执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            if (@this == null)
            {
                return;
            }
            foreach (var item in @this)
            {
                action(item);
            }
        }

        /// <summary>
        /// 通过执行动作中的每个项的枚举提供项目的当前索引上执行指定的列表一个foreach循环。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T, int> action)
        {
            if (@this == null)
            {
                return;
            }
            var index = 0;
            foreach (var item in @this)
            {
                action(item, index);
                index += 1;
            }
        }
        /// <summary>
        /// 遍历执行，同foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void ForEvery<T>(this IEnumerable<T> @this, Action<T> action)
        {
            if (@this == null)
            {
                return;
            }
            foreach (var item in @this)
            {
                action(item);
            }
        }

        /// <summary>
        /// 通过执行动作中的每个项的枚举提供项目的当前索引上执行指定的列表一个foreach循环。同foreach 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void ForEvery<T>(this IEnumerable<T> @this, Action<T, int> action)
        {
            if (@this == null)
            {
                return;
            }
            var index = 0;
            foreach (var item in @this)
            {
                action(item, index);
                index += 1;
            }
        }
        /// <summary>
        /// 随机取出列表中的一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T Random<T>(this IEnumerable<T> @this)
        {
            if (@this == null)
            {
                return default(T);
            }
            var index = new Random().Next(0, @this.Count());
            return @this.ElementAt(index);
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T @this)
        {
            return @this == null;
        }
        /// <summary>
        /// 是否不为空
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T @this)
        {
            return @this != null;
        }

        /// <summary>
        /// 不为空执行委托方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="this"></param>
        /// <param name="func"></param>
        public static void IfNotNull<T, K>(this T @this, Func<T, K> func)
        {
            if (@this != null) { func.Invoke(@this); }
        }
        /// <summary>
        /// 为空执行委托方法
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        public static void IfNull(this object @this, Action action)
        {
            if (@this == null) { action.Invoke(); }
        }

        /// <summary>
        ///当前项是否在列表中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T @this, params T[] list)
        {
            return list.Contains(@this);
        }
        /// <summary>
        /// 当前项是否不在列表中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T @this, params T[] list)
        {
            return !list.Contains(@this);
        }
        /// <summary>
        /// 列表是否为空（不为空，且包含1个以上元素）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return (@this == null || !@this.Any());
        }
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
        /// 如果结果为真，执行指定委托方法
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static TResult IfTrue<TResult>(this bool @this, Func<TResult> exp)
        {
            return @this ? exp() : default(TResult);
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
        public static bool Between<T>(this T @this, T lower, T upper) where T : IComparable<T>
        {
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
        /// 安全创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TResult Safe<T, TResult>(this T @this, Func<T, TResult> action)
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
        /// 获取列表的最小值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T MinBy<T, TMember>(this IEnumerable<T> source, Func<T, TMember> predicate)
        {
            return MinBy(source, predicate, Comparer<TMember>.Default);
        }
        /// <summary>
        /// 获取列表的最小值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T MinBy<T, TMember>(this IEnumerable<T> source, Func<T, TMember> predicate, IComparer<TMember> comparer)
        {
            var min = source.FirstOrDefault();

            var minValue = predicate(min);
            foreach (var item in source.Where(item => comparer.Compare(predicate(item), minValue) < 0))
            {
                min = item;
                minValue = predicate(min);
            }
            return min;
        }
        /// <summary>
        /// 获取列表的最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T MaxBy<T, TMember>(this IEnumerable<T> source, Func<T, TMember> predicate)
        {
            return MaxBy(source, predicate, Comparer<TMember>.Default);
        }
        /// <summary>
        /// 获取列表的最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static T MaxBy<T, TMember>(this IEnumerable<T> source, Func<T, TMember> predicate, IComparer<TMember> comparer)
        {
            var max = source.FirstOrDefault();

            var maxValue = predicate(max);
            foreach (var item in source.Where(item => comparer.Compare(predicate(item), maxValue) > 0))
            {
                max = item;
                maxValue = predicate(max);
            }
            return max;
        }
        /// <summary>
        /// 列表随机排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            var dice = new Random();
            var buffer = list.ToList();

            for (var i = 0; i < buffer.Count; i++)
            {
                var dicePick = dice.Next(i, buffer.Count);
                yield return buffer[dicePick];
                buffer[dicePick] = buffer[i];
            }
        }

        /// <summary>
        /// 如果列表为null，返回一个不包含元素的空白列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> @this)
        {
            return @this ?? Enumerable.Empty<T>();
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
        /// <summary>
        /// 将指定内标所有的元素拼接为字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> collection, string separator = " ")
        {
            return ToString(collection, t => t.ToString(), separator);
        }
        /// <summary>
        ///将指定内标所有的元素拼接为字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="exp"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> collection, Func<T, string> exp, string separator = " ")
        {
            var sBuilder = new StringBuilder();
            foreach (var item in collection)
            {
                sBuilder.Append(exp(item));
                sBuilder.Append(separator);
            }
            return sBuilder.ToString(0, Math.Max(0, sBuilder.Length - separator.Length));
        }
    }
}
