using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CAF
{

    /// <summary>
    /// 字符串操作辅助类
    /// </summary>
    public partial class Extensions
    {

        #region 字符串格式化

        /// <summary>
        /// 移除_并首字母小写的Camel样式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToCamel(this string name)
        {
            var clone = name.TrimStart('_');
            clone = RemoveSpaces(ToProperCase(clone));
            return String.Format("{0}{1}", Char.ToLower(clone[0]), clone.Substring(1, clone.Length - 1));
        }

        /// <summary>
        /// 移除_并首字母大写的Pascal样式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToCapit(this string name)
        {
            var clone = name.TrimStart('_');
            return RemoveSpaces(ToProperCase(clone));
        }
        /// <summary>
        /// 移除最后的字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveFinalChar(this string source)
        {
            if (source.Length > 1)
            {
                source = source.Substring(0, source.Length - 1);
            }
            return source;
        }
        /// <summary>
        /// 移除最后的逗号
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveFinalComma(this string source)
        {
            if (source.Trim().Length <= 0)
            {
                return source;
            }
            var c = source.LastIndexOf(",", System.StringComparison.Ordinal);
            if (c > 0)
            {
                source = source.Substring(0, source.Length - (source.Length - c));
            }
            return source;
        }

        /// <summary>
        /// 移除字符中的空格
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveSpaces(this string source)
        {
            source = source.Trim();
            source = source.Replace(" ", "");
            return source;
        }

        /// <summary>
        /// 字符串首字母大写
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToProperCase(this string source)
        {
            var revised = "";
            if (source.Length <= 0)
            {
                return revised;
            }
            var firstLetter = source.Substring(0, 1).ToUpper(new CultureInfo("en-US"));
            revised = firstLetter + source.Substring(1, source.Length - 1);
            return revised;
        }
        #endregion

        #region 字符串操作

        /// <summary>
        /// 将字符串移除最后一个分隔符并转换为列表
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> SplitToList(this string source, char separator)
        {
            source = source.RemoveFinalChar();
            return source.Split(separator).ToList();
        }



        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int StrLength(this string inputString)
        {
            var ascii = new System.Text.ASCIIEncoding();
            var tempLen = 0;
            var s = ascii.GetBytes(inputString);
            for (var i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }


        /// <summary>
        /// 如果字符串不为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfIsNotNullOrEmpty(this string source, Action<string> action)
        {
            if (!string.IsNullOrWhiteSpace(source)) { action.Invoke(source.Trim()); }
        }

        /// <summary>
        /// 如果字符串为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfIsNullOrEmpty(this string source, Action<string> action)
        {
            if (!string.IsNullOrWhiteSpace(source)) { action.Invoke(source.Trim()); }
        }

        /// <summary>
        /// 取左边n个字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string obj, int length) { return obj.Substring(0, length); }


        /// <summary>
        /// 取右边n个字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string obj, int length)
        {
            return obj.Substring(obj.Length - length, length);
        }

        /// <summary>
        /// 格式化字符串，是string.Format("",xx)的变体
        /// </summary>
        /// <param name="this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string @this, params object[] args)
        {
            return string.Format(@this, args);
        }

        /// <summary>
        ///判断字符串是否相等，忽略字符情况
        /// </summary>
        /// <param name="this"></param>
        /// <param name="compareOperand"></param>
        /// <returns></returns>
        public static bool IgnoreCaseEqual(this string @this, string compareOperand)
        {
            return @this.Equals(compareOperand, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 返回一个字符串用空格分隔如: thisIsGood => this Is Good
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string Wordify(this string @this)
        {
            // if the word is all upper, just return it
            return !Regex.IsMatch(@this, "[a-z]") ? @this : string.Join(" ", Regex.Split(@this, @"(?<!^)(?=[A-Z])"));
        }

        /// <summary>
        /// 翻转字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string Reverse(this string @this)
        {
            if (@this == null || @this.Length < 2)
            {
                return @this;
            }

            var length = @this.Length;
            var loop = (length >> 1) + 1;
            var charArray = new char[length];
            for (var i = 0; i < loop; i++)
            {
                var j = length - i - 1;
                charArray[i] = @this[j];
                charArray[j] = @this[i];
            }
            return new string(charArray);
        }

        /// <summary>
        /// 去除重复
        /// </summary>
        /// <param name="value">值，范例1："5555",返回"5",范例2："4545",返回"45"</param>
        public static string Distinct(this string value)
        {
            var array = value.ToCharArray();
            return new string(array.Distinct().ToArray());
        }

        /// <summary>
        /// 截断字符串
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="length">返回长度</param>
        /// <param name="endCharCount">添加结束符号的个数，默认0，不添加</param>
        /// <param name="endChar">结束符号，默认为省略号</param>
        public static string Truncate(this string text, int length, int endCharCount = 0, string endChar = ".")
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            if (text.Length < length)
                return text;
            return text.Substring(0, length) + GetEndString(endCharCount, endChar);
        }

        /// <summary>
        /// 是否包含数字
        /// </summary>
        /// <param name="text">文本</param>
        public static bool ContainsNumber(this string text)
        {
            const string pattern = "[0-9]+";
            return Regex.IsMatch(text, pattern);
        }

        /// <summary>
        /// 指定字符串是否在集合中
        /// </summary>
        /// <param name="stringList">字符串("A,B,C,D,E")</param>
        /// <param name="str">字符串("C")</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static bool IsInArryString(this string str, string stringList, char separator)
        {
            var list = stringList.Split(separator);
            return list.Any(t => t.Equals(str));
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 获取结束字符串
        /// </summary>
        private static string GetEndString(int endCharCount, string endChar)
        {
            var result = new StringBuilder();
            for (var i = 0; i < endCharCount; i++)
                result.Append(endChar);
            return result.ToString();
        }

        #endregion

    }


}