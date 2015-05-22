using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAF
{
    using System.Linq;
    /// <summary>
    /// 富枚举内容
    /// </summary>
    public class RichEnumContent
    {
        public string Description { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }

        public static List<RichEnumContent> Get(Type enumType)
        {
            var items = (from Enum s in Enum.GetValues(enumType) select new RichEnumContent { Description = GetDescription(s), Text = s.ToString() }).ToList();

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

        public static string GetDescription<T>(long value) where T : struct
        {
            var obj = GetEnumFromFlagsEnum<T>(value);
            var objName = obj.ToString();
            var t = obj.GetType();
            var fi = t.GetField(objName);
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return arrDesc[0].Description;
        }

        public static T GetEnumFromFlagsEnum<T>(long value) where T : struct
        {
            var values = (T[])System.Enum.GetValues(typeof(T));

            foreach (var itemValue in values)
            {
                if (Convert.ToInt64(itemValue) == value)
                {
                    return itemValue;
                }
            }
            return default(T);
        }
    }
}