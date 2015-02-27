using System;

namespace CAF
{
    using System.Globalization;

    public static class DateTimeExt
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
        /// 格式化为yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateTime(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化为yyyy-MM-dd HH:mm:ss:fffffff
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateTimeF(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
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
            while (date.DayOfWeek != firstDayOfWeek) date = date.AddDays(-1);

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
            while (date.DayOfWeek != weekday) date = date.AddDays(1);
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
            while (date.DayOfWeek != weekday) date = date.AddDays(-1);
            return date;
        }

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
            if (timeSpan < _TwoYears) return "1 年前";

            return String.Format("{0} 年前", timeSpan.Days / 365);
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


        public static int GetQuarter(int month)
        {
            if (month <= 3) return 1;
            if (month <= 6) return 2;
            if (month <= 9) return 3;
            return 4;
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
        /// 本月月初
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthBegin(this DateTime dt)
        {
            var monthBegin = dt.AddDays(1 - dt.Day);
            return new DateTime(monthBegin.Year, monthBegin.Month, monthBegin.Day);
        }

        /// <summary>
        /// 本月月末
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthEnd(this DateTime dt)
        {
            var monthEnd = GetMonthBegin(dt).AddMonths(1).AddDays(-1);
            return new DateTime(monthEnd.Year, monthEnd.Month, monthEnd.Day);
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
    }
}