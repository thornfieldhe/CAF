//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//
//namespace CAF.Test
//{
//    using System.Diagnostics;
//
//    [TestClass]
//    public class UnitTest2
//    {
//        [TestMethod]
//        public void TestDateTime()
//        {
//            DateTime d = DateTime.Now;
//            Assert.AreEqual(d.GetCountDaysOfMonth(), 31);
//            Assert.AreEqual(d.GetFirstDayOfMonth(), new DateTime(2014, 12, 1));
//            Assert.AreEqual(d.GetFirstDayOfWeek().Date, new DateTime(2014, 12, 14));
//            Assert.AreEqual(d.GetLastDayOfMonth(), new DateTime(2014, 12, 31));
//            Assert.AreEqual(new DateTime(2014, 10, 1).ToAgo(), "2 月前");
//            Assert.AreEqual(d.GetLastDayOfMonth(), new DateTime(2014, 12, 31));
//        }
//    }
//}
