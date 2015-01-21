using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CAF
{
    public class EnumExt
    {
        public static string GetDescription(System.Enum obj)
        {
            string objName = obj.ToString();
            Type t = obj.GetType();
            FieldInfo fi = t.GetField(objName);
            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return arrDesc[0].Description;
        }

        public static T GetEnumFromFlagsEnum<T>(long value) where T : struct
        {
            T[] values = (T[])System.Enum.GetValues(typeof(T));

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
            List<RichEnumContent> items = new List<RichEnumContent>();

            foreach (Enum s in Enum.GetValues(enumType))
            {
                items.Add(new RichEnumContent
                {
                    Description = EnumExt.GetDescription(s),
                    Text = s.ToString()
                });
            }
            int i = 0;
            foreach (int s in Enum.GetValues(enumType))
            {
                items[i].Value = s;
                i++;
            }
            return items;
        }
    }
}