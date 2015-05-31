using System;
using System.Diagnostics;

namespace CAF
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public partial class Extensions
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
        ///当前项是否在列表中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T @this, ICollection<T> list)
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
        /// 快速反转数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagetArray"></param>
        public static void Reversal<T>(this T[] tagetArray)
        {
            if (tagetArray == null)
            {
                throw new ArgumentNullException("tagetArray");
            }
            if (tagetArray.Length > 0)
            {
                for (var counter = 0; counter < (tagetArray.Length / 2); counter++)
                {
                    var tempHolder = tagetArray[counter];
                    tagetArray[counter] = tagetArray[tagetArray.Length - counter - 1];
                    tagetArray[tagetArray.Length - counter - 1] = tempHolder;
                }
            }
            else
            {
                Trace.WriteLine("Nothing to reverse");
            }
        }

        /// <summary>
        /// 交换数组中两个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagetArray"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        public static void Swap<T>(this T[] tagetArray, int indexA, int indexB)
        {
            var tempHolder = tagetArray[indexA];
            tagetArray[indexA] = tagetArray[indexB];
            tagetArray[indexB] = tempHolder;
        }

        /// <summary>
        /// 拼接集合元素
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string Splice<T>(IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            var result = new StringBuilder();
            foreach (var each in list)
                result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
            return result.ToString().TrimEnd(separator.ToCharArray());
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
