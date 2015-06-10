using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Tests.Domains.BaseEntity
{
    using CAF.Exceptions;
    using CAF.Model;
    using CAF.Utility;
    using System.Collections.Generic;

    [TestClass]
    public class BaseEntityTest
    {
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {

        }

        /// <summary>
        /// 测试对象相等
        /// </summary>
        [TestMethod]
        public void TestEqual()
        {
            var id = Guid.NewGuid();
            var u1 = new User() { Id = id, Name = "u1" };
            var u2 = new User() { Id = id, Name = "u2" };
            var users = new List<User> { u2 };
            Assert.AreEqual(u1, u2);//Id相等即为相等
            Assert.IsTrue(u1 == u2);
            Assert.IsFalse(u1 != u2);
            Assert.IsTrue(users.Contains(u1));
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        [TestMethod]
        public void TestCopy()
        {
            var u1 = new User();
            var r1 = new Role { Name = "r1" };
            u1.Roles.Add(r1);
            u1.Name = "u1";
            Assert.AreEqual(u1.Roles[0].Name, "r1");
            var u2 = u1.GetShallowCopy();
            u2.Roles[0].Name = "r2";
            Assert.AreEqual(u2.Roles[0].Name, "r2");
            Assert.AreEqual(u2.Roles[0].Name, u1.Roles[0].Name);//浅拷贝引用相等
            var u3 = u1.Clone();
            u3.Roles[0].Name = "r3";
            Assert.AreEqual(u3.Roles[0].Name, "r3");
            Assert.AreNotEqual(u3.Roles[0].Name, u1.Roles[0].Name);//深拷贝引用不等
            Assert.AreNotEqual(u3.Roles[0].Name, u2.Roles[0].Name);
            Assert.AreEqual(u1, u2);
            Assert.AreEqual(u1, u3);
            Assert.AreEqual(u2, u3);
        }

        /// <summary>
        /// 测试初始化状态
        /// </summary>
        [TestMethod]
        public void TestDefaultStatus()
        {
            User u = new User();
            Assert.AreNotEqual(u.Id, Guid.Empty);
            Assert.AreEqual(u.Status, 1);
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
                User u = new User { Abb = "hxh", Email = "hxh@126.com", OrganizeId = Guid.NewGuid(), Pass = "pass" };
                u.Create();
            }
            catch (Warning ex)
            {

                Assert.IsNotNull(ex.Message);
            }
        }

        /// <summary>
        /// 测试列表增减项
        /// </summary>
        [TestMethod]
        public void TestCollectionAddRemove()
        {
            var list = new UserList();
            User b = new User();
            b.Name = "name1";
            User c = new User();
            b.Name = "name1";
            list.Add(b);
            list.Add(c);
            Assert.AreEqual(list.Count, 2);
            list.RemoveAt(1);
            Assert.AreEqual(list.Count, 1);
        }


        /// <summary>
        /// 测试数据库表名
        /// </summary>
        [TestMethod]
        public void TestTableAttribute()
        {
            Assert.AreEqual(Reflection.GetTableName<Post>(), "");
            //                        Assert.AreEqual(Reflection.GetTableName<Post>(), "Sys_Posts");
        }
    }
}
