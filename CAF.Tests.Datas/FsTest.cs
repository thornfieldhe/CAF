using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Tests.Datas
{
    using CAF.Exceptions;
    using CAF.FSModels;
    using System.Collections.Generic;

    [TestClass]
    public class FsTest
    {
        [TestMethod]
        public void TestGet()
        {
            var id = Guid.NewGuid();
            var test = new Directory(id) { Name = "xxxx" };
            test.Create();
            Assert.IsNotNull(Directory.Get(id));//获取单条数据
            Assert.IsTrue(Directory.GetAll().Count > 0);//获取列表数据
            Assert.AreEqual(Directory.Get(r => r.Id == id).Count, 1);//获取条件数据
        }



        [TestMethod]
        public void TestInsert()
        {
            var test = new Directory() { Name = "mmm" };
            test.Create();
            Assert.AreEqual(1, Directory.Get(r => r.Name == test.Name).Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var t1 = new Directory() { Name = "tttt" };
            t1.Create();
            var t2 = Directory.Get(r => r.Name == t1.Name)[0];
            t2.Name = "11111";
            t2.Save();
            Assert.IsTrue(Directory.Get(d=>d.Name==t2.Name).Count>0);
        }


        /// <summary>
        /// 测试对象相等
        /// </summary>
        [TestMethod]
        public void TestEqual()
        {
            var id = Guid.NewGuid();
            var t1 = new Directory(id) { Name = "u1" };
            var t2 = new Directory(id) { Name = "u2" };
            var users = new List<Directory> { t2 };
            Assert.AreEqual(t1, t2);//Id相等即为相等
            Assert.IsTrue(t1 == t2);
            Assert.IsFalse(t1 != t2);
            Assert.IsTrue(users.Contains(t1));
        }



        /// <summary>
        /// 测试初始化状态
        /// </summary>
        [TestMethod]
        public void TestDefaultStatus()
        {
            var u = new Directory();
            Assert.AreNotEqual(u.Id, Guid.Empty);
            Assert.AreEqual(u.Status, 0);
            Assert.AreEqual(u.CreatedDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            Assert.AreEqual(u.ChangedDate.ToShortDateString(), DateTime.Now.ToShortDateString());
        }

        /// <summary>
        /// 属性有效性验证
        /// </summary>
        [TestMethod]
        public void TestValidate()
        {
            try
            {
                var u = new Directory();
                u.Validate();
            }
            catch (Warning ex)
            {
                Assert.IsNotNull(ex.Message);
            }
        }



    }
}
