using System.Collections.Generic;
using System.Linq;

namespace CAF.Tests.Extensions
{
    using CAF.Model;

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
            Assert.AreEqual("1,2,3", new List<int> { 1, 2, 3 }.ToString(","));
            Assert.AreEqual("2pp,3pp,4pp", new List<int> { 1, 2, 3 }.ToString(r => ((++r) + "pp").ToString(), ","));
        }

        [TestMethod]
        public void Test_Foreach_WithIndex()
        {
            List<string> data = new List<string>()
            {
                "A", "B", "C", "D"
            };

            var counter = 0;
            var text = "";
            data.ForEach((item, index) =>
            {
                counter += 1;
                (index == 3).IfTrue(() => text = "M");

            });
            Assert.AreEqual(4, counter);
            Assert.AreEqual("M", text);
        }
        [TestMethod]
        public void Test_Foreach()
        {
            List<string> data = new List<string>()
            {
                "A", "B", "C", "D"
            };

            var counter = 0;
            data.ForEach((item) =>
            {
                counter += 1;
            });
            Assert.AreEqual(4, counter);
        }
        [TestMethod]
        public void Test_Random()
        {
            var list = new List<int>() { 1, 2, 3 };
            Assert.IsTrue(list.Contains(list.Random()));
        }

        [TestMethod]
        public void Test_Contains()
        {
            var list = new List<int>() { 1, 2, 3 };
            Assert.IsTrue(list.Random().In(list));
            Assert.IsTrue((5).NotIn(list));
        }
        [TestMethod]
        public void Test_IsEmpty()
        {
            var list = new List<int>();
            Assert.IsTrue(list.IsNullOrEmpty());
            list.Add(2);
            Assert.IsFalse(list.IsNullOrEmpty());
            list = null;
            Assert.IsTrue(list.IsNullOrEmpty());
        }
        [TestMethod]
        public void Test_MaxOrMinValue()
        {
            var u1 = new User() { Name = "AB", Email = "234" };
            var u2 = new User() { Name = "BC", Email = "123" };
            var list = new List<User>() { u1, u2 };
            Assert.AreEqual(u1.Name, list.Min(u => u.Name));//User需要继承IComparable<User>接口
            Assert.AreEqual(u2.Name, list.Max(u => u.Name));
            Assert.AreEqual(u2.Email, list.Min(u => u.Email));
            Assert.AreEqual(u1.Email, list.Max(u => u.Email));

            Assert.AreEqual(u1.Name, list.MinBy(u => u.Name).Name);//User不需要继承IComparable接口即可实现
            Assert.AreEqual(u2.Name, list.MaxBy(u => u.Name).Name);
            Assert.AreEqual(u2.Email, list.MinBy(u => u.Email).Email);
            Assert.AreEqual(u1.Email, list.MaxBy(u => u.Email).Email);

            var list2 = new List<int>() { 1, 2, 3 };
            Assert.AreEqual(1, list2.Min());
            Assert.AreEqual(3, list2.Max());
        }
        [TestMethod]
        public void Test_Shuffle()
        {
            var list = new List<int>() { 1, 2, 3 };
            Assert.IsFalse((list.Shuffle().ToList()[0] == list[0])
                .And(list.Shuffle().ToList()[1] == list[1])
                .And(list.Shuffle().ToList()[2] == list[2]));
        }

        [TestMethod]
        public void Test_Reversal()
        {
            var list = new int[] { 1, 2, 3 };
            list.Reversal();
            Assert.AreEqual(list[0], new int[] { 3, 2, 1 }[0]);
        }
        [TestMethod]
        public void Test_Swap()
        {
            var list = new int[] { 1, 2, 3 };
            list.Swap(0, 2);
            Assert.AreEqual(list[0], new int[] { 1, 2, 3 }[2]);
        }
    }
}
