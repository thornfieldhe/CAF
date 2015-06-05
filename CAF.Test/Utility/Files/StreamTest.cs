using CAF.Files;
using CAF.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Files {
    /// <summary>
    /// 流操作测试
    /// </summary>
    [TestClass]
    public class StreamTest {
        /// <summary>
        /// 字节流添加长度
        /// </summary>
        [TestMethod]
        public void TestAddLength() {
            byte[] result = File.AddLength( new byte[] { 97 } );
            Assert.AreEqual( 1, result[0] );
            Assert.AreEqual( 0, result[1] );
            Assert.AreEqual( 0, result[2] );
            Assert.AreEqual( 0, result[3] );
            Assert.AreEqual( 97, result[4] );
        }
    }
}
