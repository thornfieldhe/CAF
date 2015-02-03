﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Test
{

    [TestClass]
    public class ModelTest
    {
        /// <summary>
        /// 测试日期字符串
        /// </summary>
        [TestMethod]
        public void TestDateString()
        {
            Assert.AreEqual(new DateTime(2014, 2, 3), ("2014-2-3").ToDate());
            DateTime d = DateTime.Now;
            Assert.AreEqual(d.GetCountDaysOfMonth(), 31);
            Assert.AreEqual(d.GetFirstDayOfMonth(), new DateTime(2015, 1, 1));
            Assert.AreEqual(d.GetFirstDayOfWeek().Date, new DateTime(2015, 1, 25));
            Assert.AreEqual(d.GetLastDayOfMonth(), new DateTime(2015, 1, 31));
            Assert.AreEqual(new DateTime(2014, 11, 1).ToAgo(), "2 月前");
            Assert.AreEqual(d.WeekOfYear(), 5);
        }
    }


}