using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Files {
    using CAF.Files;
    using CAF.Utility;

    /// <summary>
    /// 文件大小测试
    /// </summary>
    [TestClass]
    public class FileSizeTest {
        /// <summary>
        /// 文件字节数
        /// </summary>
        [TestMethod]
        public void TestSize() {
            Assert.AreEqual( 1,new FileSize( 1 ).Size );
            Assert.AreEqual( 1*1024, new FileSize( 1,FileUnit.K ).Size );
            Assert.AreEqual( 1 * 1024 * 1024, new FileSize( 1, FileUnit.M ).Size );
            Assert.AreEqual( 1 * 1024 * 1024 * 1024, new FileSize( 1, FileUnit.G ).Size );
        }

        /// <summary>
        /// 输出描述
        /// </summary>
        [TestMethod]
        public void TestToString() {
            Assert.AreEqual( "1 B",new FileSize(1).ToString() );
            Assert.AreEqual( "1 KB", new FileSize( 1 * 1024 ).ToString() );
            Assert.AreEqual( "1 MB", new FileSize( 1 * 1024 * 1024 ).ToString() );
            Assert.AreEqual( "1 GB", new FileSize( 1 * 1024 * 1024 * 1024 ).ToString() );
        }

        /// <summary>
        /// 获取文件大小，单位：K
        /// </summary>
        [TestMethod]
        public void TestGetSizeByK() {
            Assert.AreEqual( 0, new FileSize(0).GetSizeByK() );
            Assert.AreEqual( 1, new FileSize( 1024 ).GetSizeByK() );
            Assert.AreEqual( 0.5, new FileSize( 512 ).GetSizeByK() );
        }

        /// <summary>
        /// 获取文件大小，单位：M
        /// </summary>
        [TestMethod]
        public void TestGetSizeByM() {
            Assert.AreEqual( 0, new FileSize( 0 ).GetSizeByM() );
            Assert.AreEqual( 1, new FileSize( 1024 * 1024 ).GetSizeByM() );
            Assert.AreEqual( 0.5, new FileSize( 512 * 1024 ).GetSizeByM() );
        }

        /// <summary>
        /// 获取文件大小，单位：G
        /// </summary>
        [TestMethod]
        public void TestGetSizeByG() {
            Assert.AreEqual( 0, new FileSize( 0 ).GetSizeByG() );
            Assert.AreEqual( 1, new FileSize( 1024 * 1024 * 1024 ).GetSizeByG() );
            Assert.AreEqual( 0.5, new FileSize( 512 * 1024 * 1024 ).GetSizeByG() );
        }
    }
}
