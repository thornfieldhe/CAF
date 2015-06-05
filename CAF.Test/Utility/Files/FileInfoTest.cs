using CAF.Files;
using CAF.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Tests.Files
{
    /// <summary>
    /// 文件信息测试
    /// </summary>
    [TestClass]
    public class FileInfoTest
    {
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            Time.SetTime("2010-11-11 11:22:33");
        }

        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void TestClear()
        {
            Time.Reset();
        }

        /// <summary>
        /// 从文件路径中获取文件名
        /// </summary>
        [TestMethod]
        public void TestGetFileName()
        {
            Assert.AreEqual(null, FileInfo.GetFileName(null));
            Assert.AreEqual("", FileInfo.GetFileName(""));
            Assert.AreEqual("a.jpg", FileInfo.GetFileName("c:\\a.jpg"));
            Assert.AreEqual("a.jpg", FileInfo.GetFileName("c:/a.jpg"));
            Assert.AreEqual("b.jpg", FileInfo.GetFileName("c:\\a\\b.jpg"));
            Assert.AreEqual("a.jpg", FileInfo.GetFileName("\\a.jpg"));
            Assert.AreEqual("b.jpg", FileInfo.GetFileName("\\a\\b.jpg"));
            Assert.AreEqual("a.jpg", FileInfo.GetFileName("/a.jpg"));
            Assert.AreEqual("b.jpg", FileInfo.GetFileName("/a/b.jpg"));
            Assert.AreEqual("a.jpg", FileInfo.GetFileName("a.jpg"));
        }

        /// <summary>
        /// 从文件路径中获取扩展名
        /// </summary>
        [TestMethod]
        public void TestGetExtension()
        {
            Assert.AreEqual("", FileInfo.GetExtension(null));
            Assert.AreEqual("", FileInfo.GetExtension(""));
            Assert.AreEqual("jpg", FileInfo.GetExtension("c:\\a.jpg"));
        }

        /// <summary>
        /// 获取安全文件名
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetSafeName_Validate_Empty()
        {
            FileInfo.GetSafeName("");
        }

        /// <summary>
        /// 获取安全文件名
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetSafeName_Validate_null()
        {
            FileInfo.GetSafeName(null);
        }

        /// <summary>
        /// 获取安全文件名
        /// </summary>
        [TestMethod]
        public void TestGetSafeName()
        {
            Assert.AreEqual("A-112233.jpg", FileInfo.GetSafeName("a.jpg"));
            Assert.AreEqual("ZG-112233.jpg", FileInfo.GetSafeName("中国.jpg"));
            Assert.AreEqual("ZGR-112233.jpg", FileInfo.GetSafeName("~!@# $中国_'人%^{}&*()》《.jpg"));
        }
    }
}
