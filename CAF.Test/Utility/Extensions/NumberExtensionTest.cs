using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Tests.Extensions
{
    using CAF.Model;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NumberExtensionTest
    {
        /// <summary>
        /// 截断小数位数
        /// </summary>
        [TestMethod]
        
        public void TestFixed()
        {
            Assert.AreEqual(2.23,(2.2358).ToFixed(2));
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        [TestMethod]
        
        public void TestRound()
        {
            Assert.AreEqual(2.24, (2.2358).Round(2));
        }

        /// <summary>
        /// 是否在区间内
        /// </summary>
        [TestMethod]
        
        public void TestBetween()
        {
            Assert.IsTrue((2.2358).IsBetween(3.0,2.0));
        }

        /// <summary>
        /// 判断是否为空并执行操作
        /// </summary>
        [TestMethod]
        public void TestNullableAction()
        {
            string a = null;
            a.IfNull(() => a = "ccc");
            Assert.AreEqual("ccc", a);
            a.IfNotNull(r => a = r + "__");
            Assert.AreEqual("ccc__", a);

        }

        /// <summary>
        /// 判断是否为真并执行操作
        /// </summary>
        [TestMethod]
        public void TestTrueAction()
        {
            var a = "";
            true.IfTrue(() => a = "ccc");
            Assert.AreEqual("ccc", a);
            false.IfFalse(() => a = a + "__");
            Assert.AreEqual("ccc__", a);

        }


        /// <summary>
        /// 判断是否为真返回默认值
        /// </summary>
        [TestMethod]
        public void TestTrueDefault()
        {
            var a = "";
            a = true.WhenTrue<string>("ccc");
            Assert.AreEqual("ccc", a);
            false.WhenFalse(() => a = a + "__");
            Assert.AreEqual("ccc__", a);

        }


        /// <summary>
        /// 类型判断
        /// </summary>
        [TestMethod]
        public void TestIsAs()
        {
            var user = new User();
            Assert.IsNotNull(user.As<IBusinessBase>().Id);
            Assert.IsTrue(user.Is<IBusinessBase>());
        }

        /// <summary>
        /// 安全赋值
        /// </summary>
        [TestMethod]
        public void TestSet()
        {
            User user = null;
            user.SafeValue().Set(u => u.Name = "xxx");
            user = null;
            Assert.AreEqual(user.NullOr(u => u.Name), null);
        }

        /// <summary>
        /// 移除decimal尾随0
        /// </summary>
        [TestMethod]
        public void TestRemoveEnd0()
        {
            Assert.AreEqual("0.12", (0.12M).RemoveEnd0());
            Assert.AreEqual("0.12", (.12M).RemoveEnd0());
            Assert.AreEqual("12", (12M).RemoveEnd0());
            Assert.AreEqual("1200", (1200M).RemoveEnd0());
            Assert.AreEqual("120.01", (120.01M).RemoveEnd0());
            Assert.AreEqual("12", (12.00M).RemoveEnd0());
            Assert.AreEqual("12.00010001", (012.0001000100M).RemoveEnd0());
        }
    }
}
