using CAF.Files;
using CAF.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Files
{
    /// <summary>
    /// 文件路径测试
    /// </summary>
    [TestClass]
    public class FilePathTest {
        /// <summary>
        /// 连接路径
        /// </summary>
        [TestMethod]
        public void TestJoinPath() {
            Assert.AreEqual( @"d:\a\b.txt",File.JoinPath(@"d:\a","b.txt") );
            Assert.AreEqual( @"d:\a\b.txt", File.JoinPath( @"d:\a\", "b.txt" ) );
            Assert.AreEqual( @"d:\a\b.txt", File.JoinPath( @"d:\a/", "b.txt" ) );
            Assert.AreEqual( @"d:\a\b.txt", File.JoinPath( @"d:\a", "/b.txt" ) );
            Assert.AreEqual( @"d:\a\b.txt", File.JoinPath( @"d:\a\", @"\b.txt" ) );
            Assert.AreEqual( @"d:\a\b.txt", File.JoinPath( @"d:\a/", @"/\b.txt" ) );
        }
    }
}
