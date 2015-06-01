using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Util.Tests.Samples;

namespace CAF.Tests.Extensions {
    using CAF.Utility;

    /// <summary>
    /// 验证特性扩展
    /// </summary>
    [TestClass]
    public class BoolTest {
        /// <summary>
        /// 获取验证特性的错误消息
        /// </summary>
        [TestMethod]
        public void TestLogic()
        {
            Assert.IsFalse(true.Not());
            Assert.IsFalse(true.And(false));
            Assert.IsTrue(true.And(()=>1+1==2));
            Assert.IsFalse(true.AndNot(() => 1 + 1 == 2));
            Assert.IsTrue(true.AndNot(false));
            Assert.IsTrue(true.Or(false));
            Assert.IsTrue(true.Or(() => 1 + 1 == 2));
            Assert.IsTrue(true.OrNot(true));
            Assert.IsTrue(true.OrNot(() => 1 + 1 == 2));
            Assert.IsTrue(true.Xor(false));
            Assert.IsTrue(true.Xor(() => 1 + 1 != 2));
        }
    }
}
