using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Tests.Datas
{
    using CAF.Exceptions;
    using CAF.FSModels;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class FsTest
    {

        [TestMethod]
        public void TestInsert()
        {
            var id = Guid.NewGuid();
            var test = new Directory(id) { Name = "mmm", Level = "" };
            test.Create();
            Assert.AreEqual(1, Directory.Get(r => r.Id == id).Count());
        }

        [TestMethod]
        public void TestUpdate()
        {
            var id = Guid.NewGuid();
            var t1 = new Directory(id) { Name = "tttt", Level = "" };
            t1.Create();
            var t2 = Directory.Get(id);
            t2.Name = "11111";
            t2.Save();
            Assert.AreEqual(Directory.Get(id).Name, "11111");
        }

        [TestMethod]
        public void TestDelete()
        {
            var id = Guid.NewGuid();
            var t1 = new Directory(id) { Name = "tttt", Level = "" };
            t1.Create();
            var t2 = Directory.Get(id);
            Assert.IsNotNull(t2);
            t2.Delete();
            Assert.IsNull(Directory.Get(t2.Id));//实例删除


            var t21 = new Post() { Name = "tttt" };
            t21.Create();
            t21 = Post.Get(t21.Id);
            Assert.IsNotNull(t21);
            t21.Delete();
            Assert.IsNull(Post.Get(t21.Id));//实例删除
        }


        [TestMethod]
        public void TestGet()
        {
            var id = Guid.NewGuid();
            var test = new Directory(id) { Name = "xxxx", Level = "" };
            test.Create();
            Assert.IsNotNull(Directory.Get(id));//获取单条数据
            Assert.IsTrue(Directory.GetAll().Any());//获取条件数据
        }

        /// <summary>
        /// 测试从缓存中读取数据
        /// </summary>
        [TestMethod]
        public void TestCache()
        {
            var count1 = Directory.GetAll(true).Count;
            Assert.IsTrue(count1 > 0);//缓存读取
            var d = new Directory() { Name = "abc", Level = "" };
            d.Create();//新增
            var count2 = Directory.GetAll(true).Count;
            Assert.AreEqual(count1, count2);//缓存未更新
            var count3 = Directory.GetAll(false).Count;
            Assert.AreNotEqual(count1, count3);//数据库值
        }

        /// <summary>
        /// 1:1对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate2()
        {
            //新增子对象

            //更新子对象
            //删除子对象
        }

        /// <summary>
        /// 1:n对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate3()
        {
            //新增子对象

            //更新子对象
            //删除子对象
        }

        /// <summary>
        /// n:1对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate4()
        {
            //更新父对象
            //删除父对象
        }


        /// <summary>
        /// n:1对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate5()
        {
            //新增关系对象
            //更新关系对象
            //删除关系
        }
        /// <summary>
        /// 测试关系
        /// </summary>
        [TestMethod]
        public void TestUpdateP()
        {
            var id = Guid.NewGuid();
            var t1 = new Directory() { Name = "u1", Parent = new Directory(id) { Name = "t2" } };
            t1.Create();
            var t2 = Directory.Get(id);
            Assert.IsNotNull(t2);//新增1:1对象
            t2.Name = "nbnb";
            t2.Save();
            Assert.Equals(t2.Name, Directory.Get(id).Name);
            t1.Parent.Delete();
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
