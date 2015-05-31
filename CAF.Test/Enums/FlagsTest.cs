using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Enums {
    /// <summary>
    /// 测试位标志
    /// </summary>
    [TestClass]
    public class FlagsTest {
        /// <summary>
        /// 测试位标志的值1
        /// </summary>
        [TestMethod]
        public void TestValue_1() {
            Assert.AreEqual( 1, 0x0001 );
            Assert.AreEqual( 0x0001, 1 << 0 );

        }

        /// <summary>
        /// 测试位标志的值2
        /// </summary>
        [TestMethod]
        public void TestValue_2() {
            Assert.AreEqual( 2, 0x0002 );
            Assert.AreEqual( 0x0002, 1 << 1 );

        }

        /// <summary>
        /// 测试位标志的值4
        /// </summary>
        [TestMethod]
        public void TestValue_4() {
            Assert.AreEqual( 4, 0x0004 );
            Assert.AreEqual( 0x0004, 1 << 2 );

        }

        /// <summary>
        /// 测试位标志的值8
        /// </summary>
        [TestMethod]
        public void TestValue_8() {
            Assert.AreEqual( 8, 0x0008 );
            Assert.AreEqual( 0x0008, 1 << 3 );
        }

        /// <summary>
        /// 测试位标志的值16
        /// </summary>
        [TestMethod]
        public void TestValue_16() {
            Assert.AreEqual( 16, 0x0010 );
            Assert.AreEqual( 0x0010, 1 << 4 );
        }
    }
}
