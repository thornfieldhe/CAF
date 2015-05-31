using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Encrypts
{
    using CAF.Utility;

    /// <summary>
    /// 测试Md5算法
    /// </summary>
    [TestClass]
    public class Md5Test
    {
        /// <summary>
        /// 验证空值
        /// </summary>
        [TestMethod]
        public void TestMd5_Validate_Empty_16()
        {
            Assert.AreEqual("", Encrypt.Md5By16(null));
            Assert.AreEqual("", Encrypt.Md5By16(""));
        }

        /// <summary>
        /// 加密字符串，返回16位结果
        /// </summary>
        [TestMethod]
        public void TestMd5_String_16()
        {
            Assert.AreEqual("C0F1B6A831C399E2", Encrypt.Md5By16("a"));
            Assert.AreEqual("CB143ACD6C929826", Encrypt.Md5By16("中国"));
        }

        /// <summary>
        /// 验证空值
        /// </summary>
        [TestMethod]
        public void TestMd5_Validate_Empty_32()
        {
            Assert.AreEqual("", Encrypt.Md5By32(null));
            Assert.AreEqual("", Encrypt.Md5By32(""));
        }

        /// <summary>
        /// 加密字符串，返回32位结果
        /// </summary>
        [TestMethod]
        public void TestMd5_String_32()
        {
            Assert.AreEqual("0CC175B9C0F1B6A831C399E269772661", Encrypt.Md5By32("a"));
            Assert.AreEqual("C13DCEABCB143ACD6C9298265D618A9F", Encrypt.Md5By32("中国"));
        }

        /// <summary>
        /// 可逆加密解密
        /// </summary>
        [TestMethod]
        public void TestGetPass()
        {
            var pass = Encrypt.GetNewPassword(10);
            Assert.IsNotNull(pass);
            Assert.AreEqual(Encrypt.DesEncrypt("149162536"), "yNoRIOe2AZbE1DouLrHENQ==");
            Assert.AreEqual(Encrypt.DesDecrypt("yNoRIOe2AZbE1DouLrHENQ=="), "149162536");
        }
    }
}
