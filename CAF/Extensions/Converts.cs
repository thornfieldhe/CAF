using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF
{
    public static partial class Converts
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

        #region 字符串转换

        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="source">数据</param>
        public static int ToInt(this string source)
        {
            var target = 0;
            int.TryParse(source, out target);
            return target;
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="source">数据</param>
        public static int? ToIntOrNull(this string source)
        {
            if (source == null)
                return null;
            int target;
            var isValid = int.TryParse(source.ToString(), out target);
            if (isValid)
                return target;
            return null;
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="source">数据</param>
        public static double ToDouble(this string source)
        {
            var target = 0.0;
            double.TryParse(source, out target);
            return target;
        }

        /// <summary>
        /// 转换为可空double
        /// </summary>
        /// <param name="source">数据</param>
        public static double? ToDoubleOrNull(this string source)
        {
            if (source == null)
                return null;
            double target;
            var isValid = double.TryParse(source.ToString(), out target);
            if (isValid)
                return target;
            return null;
        }

        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <param name="source">数据</param>
        public static decimal ToDecimal(this string source)
        {
            var target = 0.0M;
            decimal.TryParse(source, out target);
            return target;
        }

        /// <summary>
        /// 转换为可空decimal
        /// </summary>
        /// <param name="source">数据</param>
        public static decimal? ToDecimalOrNull(this string source)
        {
            if (source == null)
                return null;
            decimal target;
            var isValid = decimal.TryParse(source, out target);
            if (isValid)
                return target;
            return null;
        }

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="source">数据</param>
        public static DateTime ToDate(this string source)
        {
            DateTime target;
            return DateTime.TryParse(source, out target) ? target : DateTime.Now;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="source">数据</param>
        public static DateTime? ToDateOrNull(this string source)
        {
            if (source == null)
                return null;
            DateTime target;
            var isValid = DateTime.TryParse(source.ToString(), out target);
            if (isValid)
                return target;
            return null;
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="source">数据</param>
        public static Guid ToGuid(this string source)
        {
            var target = Guid.Empty;
            Guid.TryParse(source, out target);
            return target;
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="source">数据</param>
        public static Guid? ToGuidOrNull(this string source)
        {
            if (source == null)
                return null;
            Guid target;
            var isValid = Guid.TryParse(source.ToString(), out target);
            if (isValid)
                return target;
            return null;
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="source">字符串集合</param>
        public static List<Guid> ToGuidList(this string source)
        {
            var listGuid = new List<Guid>();
            if (string.IsNullOrWhiteSpace(source))
                return listGuid;
            var arrayGuid = source.Split(',');
            listGuid.AddRange(from each in arrayGuid where !string.IsNullOrWhiteSpace(each) select new Guid(each));
            return listGuid;
        }


        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <param name="source">数据</param>
        public static bool ToBool(this string source)
        {
            if (source == null)
                return false;
            var value = GetBool(source);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(source.ToString(), out result) && result;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        private static bool? GetBool(string source)
        {
            switch (source.Trim().ToLower())
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
        /// <param name="data">数据</param>
        public static bool? ToBoolOrNull(string source)
        {
            if (source == null)
                return null;
            var value = GetBool(source);
            if (value != null)
                return value.Value;
            bool result;
            var isValid = bool.TryParse(source, out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                var intValue = Convert.ToInt32(value, from);  //先转成10进制
                var result = Convert.ToString(intValue, to);  //再转成目标进制
                if (to != 2)
                {
                    return result;
                }
                var resultLength = result.Length;  //获取二进制的长度
                switch (resultLength)
                {
                    case 7:
                        result = "0" + result;
                        break;
                    case 6:
                        result = "00" + result;
                        break;
                    case 5:
                        result = "000" + result;
                        break;
                    case 4:
                        result = "0000" + result;
                        break;
                    case 3:
                        result = "00000" + result;
                        break;
                }
                return result;
            }
            catch
            {

                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "0";
            }
        }


        #endregion
    }

}
