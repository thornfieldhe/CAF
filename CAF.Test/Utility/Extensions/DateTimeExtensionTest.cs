using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Extensions {
    using System;

    /// <summary>
    /// 日期时间扩展测试
    /// </summary>
    [TestClass]
    public class DateTimeExtensionTest {
        /// <summary>
        /// 获取格式化日期时间字符串
        /// </summary>
        [TestMethod]
        public void TestToDateTimeString() {
            string date = "2012-01-02 11:11:11";
            Assert.AreEqual(date, date.ToDate().ToDateTimeString());
            Assert.AreEqual( "2012-01-02 11:11", date.ToDate().ToDateTimeString( true ) );
    
        }

        /// <summary>
        /// 获取格式化日期字符串
        /// </summary>
        [TestMethod]
        public void TestToDateString() {
            string date = "2012-01-02";
            Assert.AreEqual( date, date.ToDate().ToDateString() );

        }

        /// <summary>
        /// 获取格式化时间字符串
        /// </summary>
        [TestMethod]
        public void TestToTimeString() {
            string date = "2012-01-02 11:11:11";
            Assert.AreEqual("11:11:11", date.ToDate().ToTimeString());
        }

        /// <summary>
        /// 获取格式化毫秒字符串
        /// </summary>
        [TestMethod]
        public void TestToMillisecondString() {
            string date = "2012-01-02 11:11:11.123";
            Assert.AreEqual(date, date.ToDate().ToMillisecondString());
        }

        /// <summary>
        /// 获取格式化中文日期字符串
        /// </summary>
        [TestMethod]
        public void TestToChineseDateString() {
            string date = "2012-01-02";
            Assert.AreEqual("2012年1月2日", date.ToDate().ToChineseDateString());
        }

        /// <summary>
        /// 获取格式化中文日期时间字符串
        /// </summary>
        [TestMethod]
        public void TestToChineseDateTimeString() {
            string date = "2012-01-02 11:11:11";
            Assert.AreEqual("2012年1月2日 11时11分11秒", date.ToDate().ToChineseDateTimeString());
        }

        /// <summary>
        /// 获取时间间隔
        /// </summary>
        [TestMethod]
        public void TestTimeSpan()
        {
            var timeSpan=new DateTime(2015,1,1).GetTimeSpan(new DateTime(2015,1,2));
            
            Assert.AreEqual(timeSpan.Days, 1);
        }

        /// <summary>
        /// 计算指定月天数
        /// </summary>
        [TestMethod]
        public void TestGetCountDaysOfMonth()
        {
            var days = new DateTime(2015, 1, 1).GetCountDaysOfMonth();

            Assert.AreEqual(days, 31);
        }

        /// <summary>
        /// 计算指定月天数
        /// </summary>
        [TestMethod]
        public void TestWeekOfYear()
        {
            var weeks = new DateTime(2015, 1, 6).WeekOfYear();
            Assert.AreEqual(weeks, 2);
        }

        /// <summary>
        /// 获取季度
        /// </summary>
        [TestMethod]
        public void TestGetQuarter()
        {
            var month = new DateTime(2015, 6, 6).GetQuarter();
            Assert.AreEqual(month, 2);
        }

        /// <summary>
        /// 是否是周末
        /// </summary>
        [TestMethod]
        public void TestIsWeekend()
        {
            var isWeekend = new DateTime(2015, 6, 6).IsWeekend();
            Assert.IsTrue(isWeekend);
        }


        /// <summary>
        /// 是否在日期区间内
        /// </summary>
        [TestMethod]
        public void TestIsWithin()
        {
            var isWithin = new DateTime(2015, 6, 6).IsWithin(new DateTime(2015, 1, 1), new DateTime(2015, 10, 1));
            Assert.IsTrue(isWithin);
        }


        /// <summary>
        /// 是否在日期区间内
        /// </summary>
        [TestMethod]
        public void TestToAgo()
        {
            var toAgo = new DateTime(2015, 6, 6).ToAgo();
            Assert.AreEqual("未来", toAgo);
        }

        /// <summary>
        /// 是否在日期区间内
        /// </summary>
        [TestMethod]
        public void TestCurrentDate()
        {
            var date = new DateTime(2015, 1, 30, 10, 30,5);
            var date1 = new DateTime(2015, 1, 30);
            var date2 = new DateTime(2015, 1, 30,23, 59, 59);
            var date3 = new DateTime(2015, 1, 31);
            var date4 = new DateTime(2015, 1, 29);
            var date5 = new DateTime(2015, 1, 1);
            var date6 = new DateTime(2015, 1, 31);
            var date7 = new DateTime(2015, 1, 25);
            var date8 = new DateTime(2015, 1, 31);
            var date9 = new DateTime(2015, 1, 29);

            Assert.AreEqual(date1,date.StartOfDay());
            Assert.AreEqual(date2,date.EndOfDay());
            Assert.AreEqual(date3, date.NextDay());
            Assert.AreEqual(date4, date.Yesterday());
            Assert.AreEqual(date5, date.GetFirstDayOfMonth());
            Assert.AreEqual(date6, date.GetLastDayOfMonth());
            Assert.AreEqual(date7, date.GetFirstDayOfWeek());
            Assert.AreEqual(date8, date.GetLastDayOfWeek());
            Assert.AreEqual(date9, date.GetWeekday(DayOfWeek.Thursday));
        }
    }
}
