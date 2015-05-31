using System;

namespace CAF
{
    using System.Globalization;
    using System.Text;

    public partial class Extensions
    {
        private static readonly TimeSpan _OneMinute = new TimeSpan(0, 1, 0);
        private static readonly TimeSpan _TwoMinutes = new TimeSpan(0, 2, 0);
        private static readonly TimeSpan _OneHour = new TimeSpan(1, 0, 0);
        private static readonly TimeSpan _TwoHours = new TimeSpan(2, 0, 0);
        private static readonly TimeSpan _OneDay = new TimeSpan(1, 0, 0, 0);
        private static readonly TimeSpan _TwoDays = new TimeSpan(2, 0, 0, 0);
        private static readonly TimeSpan _OneWeek = new TimeSpan(7, 0, 0, 0);
        private static readonly TimeSpan _TwoWeeks = new TimeSpan(14, 0, 0, 0);
        private static readonly TimeSpan _OneMonth = new TimeSpan(31, 0, 0, 0);
        private static readonly TimeSpan _TwoMonths = new TimeSpan(62, 0, 0, 0);
        private static readonly TimeSpan _OneYear = new TimeSpan(365, 0, 0, 0);
        private static readonly TimeSpan _TwoYears = new TimeSpan(730, 0, 0, 0);

        /// <summary>
        /// 获取两个时间间隔
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static TimeSpan GetTimeSpan(this DateTime startTime, DateTime endTime)
        {
            return endTime - startTime;
        }

        /// <summary>
        /// 判断日期是否是今日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsToday(this DateTime dt)
        {
            return (dt.Date == DateTime.Today);
        }

        /// <summary>
        /// 判断dto日期是否是今日
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool IsToday(this DateTimeOffset dto)
        {
            return (dto.Date.IsToday());
        }

        /// <summary>
        /// 计算指定月天数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }

        /// <summary>
        /// 获取日期周数
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime)
        {
            var dateinf = new DateTimeFormatInfo();
            var weekrule = dateinf.CalendarWeekRule;
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        /// <summary>
        /// 获取日期周数
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="weekrule"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule)
        {
            var dateinf = new System.Globalization.DateTimeFormatInfo();
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        /// <summary>
        /// 获取日期周数
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="firstDayOfWeek"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            var dateinf = new System.Globalization.DateTimeFormatInfo();
            var weekrule = dateinf.CalendarWeekRule;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        /// <summary>
        /// 获取日期周数
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="weekrule"></param>
        /// <param name="firstDayOfWeek"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule, DayOfWeek firstDayOfWeek)
        {
            var ciCurr = System.Globalization.CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        /// <summary>
        /// 获取季度
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static int GetQuarter(this DateTime datetime)
        {
            if (datetime.Month <= 3) return 1;
            if (datetime.Month <= 6) return 2;
            return datetime.Month <= 9 ? 3 : 4;
        }

        /// <summary>
        /// 是否是工作日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsWeekDay(this DateTime date)
        {
            return !date.IsWeekend();
        }

        /// <summary>
        /// 是否是周末
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime value)
        {
            return value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;
        }


        /// <summary>
        /// 时间是否处于时间范围中
        /// </summary>
        /// <param name="this"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool IsWithin(this DateTime @this, DateTime startDate, DateTime endDate)
        {
            return @this > startDate && @this < endDate;
        }

        #region 获取具体时间


        /// <summary>
        /// 返回当日结束时间 23:59:59;
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime @this)
        {
            return @this.Date.AddDays(1).AddSeconds(-1);
        }
        /// <summary>
        /// 返回当日开始时间 00:00:00
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime @this)
        {
            return @this.Date.Date;
        }
        /// <summary>
        /// 明天
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime NextDay(this DateTime @this)
        {
            return @this.StartOfDay().AddDays(1);
        }

        /// <summary>
        /// 昨天
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime Yesterday(this DateTime @this)
        {
            return @this.StartOfDay().AddDays(-1);
        }

        /// <summary>
        /// 日期所在月第一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 日期所在月第一天
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
            {
                dt = dt.AddDays(1);
            }
            return dt;
        }

        /// <summary>
        /// 日期所在月最后一天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }

        /// <summary>
        /// 日期所在月最后一天
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
            {
                dt = dt.AddDays(-1);
            }
            return dt;
        }


        /// <summary>
        /// 获取日期所在周一日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            return date.GetFirstDayOfWeek(null);
        }

        /// <summary>
        /// 获取日期所在周一日期
        /// </summary>
        /// <param name="date"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            while (date.DayOfWeek != firstDayOfWeek)
                date = date.AddDays(-1).Date;

            return date;
        }

        /// <summary>
        /// 获取日期所在周末日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(this DateTime date)
        {
            return date.GetLastDayOfWeek(null);
        }

        /// <summary>
        /// 获取日期所在周末日期
        /// </summary>
        /// <param name="date"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            return date.GetFirstDayOfWeek(cultureInfo).AddDays(6);
        }

        /// <summary>
        /// 获取工作日日期
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weekday"></param>
        /// <returns></returns>
        public static DateTime GetWeekday(this DateTime date, DayOfWeek weekday)
        {
            return date.GetWeekday(weekday, null);
        }

        /// <summary>
        /// 获取工作日日期
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weekday"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static DateTime GetWeekday(this DateTime date, DayOfWeek weekday, CultureInfo cultureInfo)
        {
            var firstDayOfWeek = date.GetFirstDayOfWeek(cultureInfo);
            return firstDayOfWeek.GetNextWeekday(weekday);
        }

        /// <summary>
        /// 获取下周周末
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weekday"></param>
        /// <returns></returns>
        public static DateTime GetNextWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
                date = date.AddDays(1);
            return date;
        }

        /// <summary>
        /// 获取下周日日期
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weekday"></param>
        /// <returns></returns>
        public static DateTime GetPreviousWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
                date = date.AddDays(-1);
            return date;
        }

        /// <summary>
        /// 返回几秒钟前时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime SecondsAgo(this int @this)
        {
            return DateTime.Now.AddSeconds(-@this);
        }
        /// <summary>
        /// 返回几分钟前时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime MinutesAgo(this int @this)
        {
            return DateTime.Now.AddMinutes(-@this);
        }
        /// <summary>
        /// 返回几小时前时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime HoursAgo(this int @this)
        {
            return DateTime.Now.AddHours(-@this);
        }
        /// <summary>
        /// 返回几天前时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime DaysAgo(this int @this)
        {
            return DateTime.Now.AddDays(-@this);
        }
        /// <summary>
        /// 返回几个月前时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime MonthsAgo(this int @this)
        {
            return DateTime.Now.AddMonths(-@this);
        }
        /// <summary>
        /// 返回几年前时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime YearsAgo(this int @this)
        {
            return DateTime.Now.AddYears(-@this);
        }
        /// <summary>
        /// 返回几秒钟后时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime SecondsFromNow(this int @this)
        {
            return DateTime.Now.AddSeconds(@this);
        }
        /// <summary>
        /// 返回几分钟后时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime MinutesFromNow(this int @this)
        {
            return DateTime.Now.AddMinutes(@this);
        }
        /// <summary>
        /// 返回几小时后时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime HoursFromNow(this int @this)
        {
            return DateTime.Now.AddHours(@this);
        }
        /// <summary>
        /// 返回几天后时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime DaysFromNow(this int @this)
        {
            return DateTime.Now.AddDays(@this);
        }
        /// <summary>
        /// 返回几月后时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime MonthsFromNow(this int @this)
        {
            return DateTime.Now.AddMonths(@this);
        }
        /// <summary>
        /// 返回几年后时间
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime YearsFromNow(this int @this)
        {
            return DateTime.Now.AddYears(@this);
        }

        /// <summary>
        /// 增加周
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DateTime AddWeeks(this DateTime dt, int count)
        {
            var dateBegin = GetWeekday(dt, DayOfWeek.Monday);
            return dateBegin.AddDays(7 * count);
        }

        /// <summary>
        /// 返回所在年的第几天的具体日期
        /// </summary>
        /// <param name="this"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime DayInYear(this int @this, int? year = null)
        {
            var firstDayOfYear = new DateTime(year ?? DateTime.Now.Year, 1, 1);
            return firstDayOfYear.AddDays(@this - 1);
        }

        #endregion

        #region 格式化时间


        /// <summary>
        /// 简化日期格式：xx分钟前
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToAgo(this DateTime date)
        {
            var timeSpan = date.GetTimeSpan(DateTime.Now);
            if (timeSpan < TimeSpan.Zero) return "未来";
            if (timeSpan < _OneMinute) return "现在";
            if (timeSpan < _TwoMinutes) return "1 分钟前";
            if (timeSpan < _OneHour) return String.Format("{0} 分钟前", timeSpan.Minutes);
            if (timeSpan < _TwoHours) return "1 小时前";
            if (timeSpan < _OneDay) return String.Format("{0} 小时前", timeSpan.Hours);
            if (timeSpan < _TwoDays) return "昨天";
            if (timeSpan < _OneWeek) return String.Format("{0} 天前", timeSpan.Days);
            if (timeSpan < _TwoWeeks) return "1 周前";
            if (timeSpan < _OneMonth) return String.Format("{0} 周前", timeSpan.Days / 7);
            if (timeSpan < _TwoMonths) return "1 月前";
            if (timeSpan < _OneYear) return String.Format("{0} 月前", timeSpan.Days / 31);
            return timeSpan < _TwoYears ? "1 年前" : String.Format("{0} 年前", timeSpan.Days / 365);
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            return dateTime.ToString(isRemoveSecond ? "yyyy-MM-dd HH:mm" : "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            return dateTime == null ? string.Empty : ToDateTimeString(dateTime.Value, isRemoveSecond);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ToDateString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ToTimeString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime? dateTime)
        {
            return dateTime == null ? string.Empty : ToMillisecondString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime dateTime)
        {
            return string.Format("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime? dateTime)
        {
            return !dateTime.HasValue ? string.Empty : ToChineseDateString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            var result = new StringBuilder();
            result.AppendFormat("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
            result.AppendFormat(" {0}时{1}分", dateTime.Hour, dateTime.Minute);
            if (isRemoveSecond == false)
                result.AppendFormat("{0}秒", dateTime.Second);
            return result.ToString();
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            return dateTime == null ? string.Empty : ToChineseDateTimeString(dateTime.Value);
        }
    }

        #endregion

}