using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAF.Utility
{
    using System.Linq;
    /// <summary>
    /// 富枚举内容
    /// </summary>
    public class EnumContent
    {
        public string Description { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }

        public static List<EnumContent> Get(Type enumType)
        {
            var items = (from Enum s in Enum.GetValues(enumType) select new EnumContent { Description = GetDescription(s), Text = s.ToString() }).ToList();

            var i = 0;
            foreach (int s in Enum.GetValues(enumType))
            {
                items[i].Value = s;
                i++;
            }
            return items;
        }

        public static string GetDescription(Enum obj)
        {
            var objName = obj.ToString();
            var t = obj.GetType();
            var fi = t.GetField(objName);
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return arrDesc[0].Description;
        }

        public static string GetDescription<T>(long value) where T : struct, IConvertible
        {
            var obj = GetEnumFromFlagsEnum<T>(value);
            var objName = obj.ToString();
            var t = obj.GetType();
            var fi = t.GetField(objName);
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return arrDesc[0].Description;
        }

        /// <summary>
        /// 将数值转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumFromFlagsEnum<T>(long value) where T : struct, IConvertible
        {
            var values = (T[])System.Enum.GetValues(typeof(T));

            foreach (var itemValue in values.Where(itemValue => Convert.ToInt64(itemValue) == value))
            {
                return itemValue;
            }
            return default(T);
        }

        /// <summary>
        ///将字符串文本转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignorecase"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value, bool ignorecase = false) where T : struct, IConvertible
        {
            if (value == null)
            {
                return default(T);
            }

            value = value.Trim();

            if (value.Length == 0)
            {
                return default(T);
            }

            Type t = typeof(T);

            if (!t.IsEnum)
            {
                return default(T);
            }

            return (T)Enum.Parse(t, value, ignorecase);
        }
    }

}