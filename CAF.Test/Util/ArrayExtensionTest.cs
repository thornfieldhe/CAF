using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArrayExtensionTest
    {
        /// <summary>
        /// 转换为用分隔符拼接的字符串
        /// </summary>
        [TestMethod]
        public void TestSplice()
        {
            Assert.AreEqual("1,2,3", new List<int> { 1, 2, 3 }.Splice());
            Assert.AreEqual("'1','2','3'", new List<int> { 1, 2, 3 }.Splice("'"));
        }
    }
}
