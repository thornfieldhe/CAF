using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Randoms
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using CAF.Utility;

    [TestClass]
    public class RandomsTest
    {
        [TestMethod]
        public void TestRandomsInt()
        {
            var a = Randoms.GetRandomInt(10, 20);
            Assert.IsTrue(a.Between(10,20));
        }

        [TestMethod]
        public void TestRandomsDouble()
        {
            var a = Randoms.GetRandomDouble();
            Assert.IsTrue(a.Between(0.0, 1.0));
        }
        [TestMethod]
        public void TestRandomsRandomArray()
        {
            var a = new int[]{1,2,3,4,5,6};
            Randoms.GetRandomArray(a);
            Assert.IsFalse(a[0]!=1);
        }
        [TestMethod]
        public void TestRandomsGenerateCheckCodeNum()
        {
            var a=Randoms.GenerateCheckCodeNum(5);
            Console.WriteLine(a);
            Assert.IsNotNull(a);
            var b = Randoms.GenerateCheckCode(5);
            Console.WriteLine(b);
            Assert.IsNotNull(b);
            var c = Randoms.GetRandomCode(5);
            Console.WriteLine(c);
            Assert.IsNotNull(c);
        }
        [TestMethod]
        public void TestRnd()
        {
            var a = Randoms.GetDateRnd();
            Console.WriteLine(a);
            Assert.IsNotNull(a);
            var b = Randoms.GetRndKey();
            Console.WriteLine(b);
            Assert.IsNotNull(b);
            var c = Randoms.GetRndNum(5,true);
            Console.WriteLine(c);
            Assert.IsNotNull(c);
            var d = Randoms.GetRndNum(5, false);
            Console.WriteLine(d);
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void TestGetRndNext()
        {
            var a = Randoms.GetRndNext(10,15);
            var b = new int[] { 11, 12, 13, 14 };
            Console.WriteLine(a);
            Assert.IsTrue(a.In(b));
            

        }
    }
}
