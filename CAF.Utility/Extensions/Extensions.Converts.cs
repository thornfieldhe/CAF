﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF
{
    using CAF.Utility;

    public partial class Extensions
    {
        /// <summary>
        /// 转换成int值
        /// true:1
        /// false:0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this bool obj) { return obj ? 1 : 0; }

        /// <summary>
        /// 转换成bool值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBool(this int obj) { return obj == 1; }


        /// <summary>
        /// 在未知对象类型时将对象转换成类型T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T To<T>(this object data)
        {
            if (data == null)
                return default(T);
            if (data is string && String.IsNullOrWhiteSpace(data.ToString()))
                return default(T);
            var type = OtherExtension.GetType<T>();
            try
            {
                if (type.Name.ToLower() == "guid")
                    return (T)(object)new Guid(data.ToString());
                if (data is IConvertible)
                    return (T)Convert.ChangeType(data, type);
                return (T)data;
            }
            catch
            {
                return default(T);
            }
        }


        /// <summary>
        /// 列表转换成csv对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToCSV<T>(this IEnumerable<T> list, string separator = ",")
        {
            if (list == null)
            {
                throw new ArgumentNullException();
            }
            return String.Join(separator, list);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="obj">对象</param>
        public static string ToStr(this object obj)
        {
            return obj == null ? String.Empty : obj.ToString().Trim();
        }

        #region 字符串转换

        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static int ToInt(this string @this, int defaultValue = default(int))
        {
            Int32 x;
            return Int32.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static int? ToIntOrNull(this string @this, int defaultValue = default(int))
        {
            if (@this == null)
                return null;
            Int32 x;
            return Int32.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 试图解析字符串为64位整数，如果解析失败则返回默认值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this string @this, long defaultValue = default(long))
        {
            long x;
            return Int64.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static double ToDouble(this string @this, double defaultValue = default(double))
        {
            double x;
            return Double.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为可空double
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static double? ToDoubleOrNull(this string @this, double defaultValue = default(double))
        {
            if (@this == null)
                return null;
            double x;
            return Double.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static decimal ToDecimal(this string @this, decimal defaultValue = default(decimal))
        {
            decimal x;
            return Decimal.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为可空decimal
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static decimal? ToDecimalOrNull(this string @this, decimal defaultValue = default(decimal))
        {
            if (@this == null)
                return null;
            decimal x;
            return Decimal.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换成浮点数
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string @this, float defaultValue = default(float))
        {
            float x;
            return Single.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="this">数据</param>
        public static DateTime ToDate(this string @this)
        {
            DateTime target;
            return DateTime.TryParse(@this, out target) ? target : DateTime.Now;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="this">数据</param>
        public static DateTime? ToDateOrNull(this string @this)
        {
            if (@this == null)
                return null;
            DateTime target;
            var isValid = DateTime.TryParse(@this.ToString(), out target);
            if (isValid)
                return target;
            return null;
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static Guid ToGuid(this string @this, Guid defaultValue = default(Guid))
        {
            Guid x;
            return Guid.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="this">数据</param>
        /// <param name="defaultValue"></param>
        public static Guid? ToGuidOrNull(this string @this, Guid defaultValue = default(Guid))
        {
            if (@this == null)
                return null;
            Guid x;
            return Guid.TryParse(@this, out x) ? x : defaultValue;
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="this">字符串集合</param>
        public static List<Guid> ToGuidList(this string @this)
        {
            var listGuid = new List<Guid>();
            if (String.IsNullOrWhiteSpace(@this))
                return listGuid;
            var arrayGuid = @this.Split(',');
            listGuid.AddRange(from each in arrayGuid where !String.IsNullOrWhiteSpace(each) select new Guid(each));
            return listGuid;
        }


        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <param name="this">数据</param>
        public static bool ToBool(this string @this)
        {
            if (@this == null)
                return false;
            var value = GetBool(@this);
            if (value != null)
                return value.Value;
            bool result;
            return Boolean.TryParse(@this.ToString(), out result) && result;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        private static bool? GetBool(string @this)
        {
            switch (@this.Trim().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "否":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "true":
                    return true;
                case "false":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 转换为可空布尔值
        /// </summary>
        /// <param name="this">数据</param>
        public static bool? ToBoolOrNull(this string @this)
        {
            if (@this == null)
                return null;
            var value = GetBool(@this);
            if (value != null)
                return value.Value;
            bool result;
            var isValid = Boolean.TryParse(@this, out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为目标元素集合
        /// </summary>
        /// <typeparam name="T">目标元素类型</typeparam>
        /// <param name="list">元素集合字符串，范例:83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A</param>
        public static List<T> ToList<T>(this string list)
        {
            var result = new List<T>();
            if (String.IsNullOrWhiteSpace(list))
                return result;
            var array = list.Split(',');
            result.AddRange(from each in array where !String.IsNullOrWhiteSpace(each) select To<T>(each));
            return result;
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">字符串集合</param>
        public static List<T> ToList<T>(this IList<string> obj)
        {
            return obj == null ? new List<T>() : obj.Select(t => t.To<T>()).ToList();
        }

        #endregion
    }

}
