using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Tests.Extensions
{
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
    }
}
