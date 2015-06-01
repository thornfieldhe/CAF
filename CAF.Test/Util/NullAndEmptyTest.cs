using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Extensions
{
    using System;

    [TestClass]
    public class NullAndEmptyTest
    {
        [TestMethod]
        public void TestIsNull()
        {
            string a = null;
            Assert.IsTrue(a.IsNull());
            Assert.IsFalse(a.IsNotNull());
        }

        [TestMethod]
        public void TestStringIsEmpty()
        {
            var a = "";
            Assert.IsTrue(a.IsEmpty());
            a = null;
            Assert.IsTrue(a.IsEmpty());
        }

        [TestMethod]
        public void TestGuidIsEmpty()
        {
            Guid? a = null;
            Assert.IsTrue(a.IsEmpty());
            a = Guid.Empty;
            Assert.IsTrue(a.IsEmpty());

        }

        /// <summary>
        /// 测试可空整型
        /// </summary>
        [TestMethod]
        public void TestSafeValue_Int()
        {
            int? value = null;
            Assert.AreEqual(0, value.SafeValue());

            value = 1;
            Assert.AreEqual(1, value.SafeValue());
        }

        /// <summary>
        /// 测试可空DateTime
        /// </summary>
        [TestMethod]
        public void TestSafeValue_DateTime()
        {
            DateTime? value = null;
            Assert.AreEqual(DateTime.MinValue, value.SafeValue());

            value = ("2000-1-1").ToDate();
            Assert.AreEqual(value.Value, value.SafeValue());
        }

        /// <summary>
        /// 测试可空bool
        /// </summary>
        [TestMethod]
        public void TestSafeValue_Boolean()
        {
            bool? value = null;
            Assert.AreEqual(false, value.SafeValue());

            value = true;
            Assert.AreEqual(true, value.SafeValue());
        }

        /// <summary>
        /// 测试可空double
        /// </summary>
        [TestMethod]
        public void TestSafeValue_Double()
        {
            double? value = null;
            Assert.AreEqual(0, value.SafeValue());

            value = 1.1;
            Assert.AreEqual(1.1, value.SafeValue());
        }

        /// <summary>
        /// 测试可空decimal
        /// </summary>
        [TestMethod]
        public void TestSafeValue_Decimal()
        {
            decimal? value = null;
            Assert.AreEqual(0, value.SafeValue());

            value = 1.1M;
            Assert.AreEqual(1.1M, value.SafeValue());
        }
    }
}
