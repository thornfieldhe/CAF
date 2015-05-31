using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Extensions {
    /// <summary>
    /// 格式化扩展测试
    /// </summary>
    [TestClass]
    public class FormatExtensionTest {
       

        /// <summary>
        /// 格式化整数
        /// </summary>
        [TestMethod]
        public void TestFormat_Int() {
            int? value = null;
            Assert.AreEqual( "",value.Format() );
            Assert.AreEqual( "", 0.Format() );
            Assert.AreEqual( "1",1.Format() );
            Assert.AreEqual( ".", 0.Format(".") );
        }

        /// <summary>
        /// 格式化Decimal
        /// </summary>
        [TestMethod]
        public void TestFormat_Decimal() {
            decimal? value = null;
            Assert.AreEqual( "", value.Format() );
            Assert.AreEqual( "", 0M.Format() );
            Assert.AreEqual( "123456789.12", 123456789.123456789M.Format() );
            Assert.AreEqual( ".", 0M.Format( "." ) );
        }

        /// <summary>
        /// 格式化Double
        /// </summary>
        [TestMethod]
        public void TestFormat_Double() {
            double? value = null;
            Assert.AreEqual( "", value.Format() );
            Assert.AreEqual( "", 0d.Format() );
            Assert.AreEqual( "123456789.12", 123456789.123456789d.Format() );
            Assert.AreEqual( ".", 0d.Format( "." ) );
        }

        /// <summary>
        /// 格式化为￥
        /// </summary>
        [TestMethod]
        public void TestFormatRmb() {
            decimal? value = null;
            Assert.AreEqual( "￥0", value.FormatRmb() );
            Assert.AreEqual( "￥0", 0M.FormatRmb() );
            Assert.AreEqual( "￥123456789.12", 123456789.123456789M.FormatRmb() );
        }

        /// <summary>
        /// 格式化为%
        /// </summary>
        [TestMethod]
        public void TestFormatPercent_Decimal() {
            decimal? value = null;
            Assert.AreEqual( "", value.FormatPercent() );
            Assert.AreEqual( "", 0M.FormatPercent() );
            Assert.AreEqual( "123456789.12%", 123456789.123456789M.FormatPercent() );
        }

        /// <summary>
        /// 格式化为%
        /// </summary>
        [TestMethod]
        public void TestFormatPercent_Double() {
            double? value = null;
            Assert.AreEqual( "", value.FormatPercent() );
            Assert.AreEqual( "", 0d.FormatPercent() );
            Assert.AreEqual( "123456789.12%", 123456789.1234d.FormatPercent() );
        }
    }
}
