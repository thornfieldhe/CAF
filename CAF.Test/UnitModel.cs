using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Test
{
    using CAF.Model;
    using System.Collections.Generic;

    [TestClass]
    public class UnitModel
    {
        /// <summary>
        /// 是否相等
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Guid id = Guid.NewGuid();
            User u1 = new User(id) { Name = "u1" };
            User u2 = new User(id) { Name = "u2" };
            List<User> users = new List<User> { u2 };
            Assert.AreEqual(u1, u2);//Id相等即为相等
            Assert.IsTrue(u1 == u2);
            Assert.IsFalse(u1 != u2);
            Assert.IsTrue(users.Contains(u1));
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            User u1 = User.New();
            Role r1 = Role.New();
            r1.Name = "r1";
            u1.Roles.Add(r1);
            u1.Name = "u1";
            Assert.AreEqual(u1.Roles[0].Name, "r1");
            User u2 = u1.GetShallowCopy();
            u2.Roles[0].Name = "r2";
            Assert.AreEqual(u2.Roles[0].Name, "r2");
            Assert.AreEqual(u2.Roles[0].Name, u1.Roles[0].Name);//浅拷贝引用相等
            User u3 = u1.Clone();
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
        public void TestMethod3()
        {
            User u = User.New();
            Assert.AreNotEqual(u.Id, Guid.Empty);
            Assert.AreEqual(u.Status, 1);
            Assert.AreEqual(u.CreatedDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            Assert.AreEqual(u.ChangedDate.ToShortDateString(), DateTime.Now.ToShortDateString());
        }

        /// <summary>
        /// 属性有效性验证
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            User u = User.New();
            u.Abb = "hxh";
            u.Email = "hxh@126.com";
            u.OrganizeId = Guid.NewGuid();
            u.Pass = "pass";
            Assert.IsTrue(!u.IsValid);//未验证
            u.PhoneNum = "13666188693";
            u.Name = "何翔华";
            u.LoginName = "00001";
            Assert.IsTrue(u.IsValid);//已验证
        }

        /// <summary>
        /// 简单增删改查
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            User u = CreateUser();
            u.OrganizeId = Guid.NewGuid();
            if (u.IsValid)
            {
                u.Create();//创建
            }
            var newUser = User.Get(u.Id);
            Assert.IsNotNull(newUser);//读取
            newUser.Note = "test";
            newUser.PhoneNum = null;
            newUser.Save();//更新
            var newUser4 = User.Get(u.Id);
            newUser4.Delete();//删除
            var newUser5 = User.Get(u.Id);
            Assert.IsNull(newUser5);
        }

        /// <summary>
        /// 1：n关系
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            //新增with子项列表
            Organize o = Organize.New();
            o.Sort = 0;
            o.Name = "o1";
            o.Level = "01";
            o.Code = "00001";
            User u = CreateUser();
            o.Users.Add(u);
            if (o.IsValid)
            {
                o.Create();
            }
            var o1 = Organize.Get(o.Id);
            Assert.AreEqual(o1.Users.Count, 1);
            //新增子项列表中子项
            User u1 = CreateUser();
            u1.OrganizeId = o1.Id;
            //todo 未实现增加子项
            o1.Users.Add(u1);
            if (o1.IsValid)
            {
                o1.Save();
            }
            var o2 = Organize.Get(o.Id);
            Assert.AreEqual(o2.Users.Count, 2);//检查用户信息是否延迟加载
            //编辑子项列表中子项
            o2.Users[0].Note = "ttttttttt";//检查用户信息是否延迟加载
            if (o2.IsValid)
            {
                o2.Save();
            }
            //编辑with子项列表中的子项
            var o3 = Organize.Get(o.Id);
            o3.Users[1].Note = "kkk";//检查用户信息是否延迟加载
            o3.Note = "12345";
            if (o3.IsValid)
            {
                o3.Save();
            }
            var o4 = Organize.Get(o.Id);
            Assert.AreEqual(o4.Users[1].Note, "kkk");
            Assert.AreEqual(o4.Note, "12345");
            //删除子项
            o4.Users.RemoveAt(1);
            o4.Save();
            var o5 = Organize.Get(o.Id);
            Assert.AreEqual(o5.Users.Count, 1);
        }

        /// <summary>
        /// 1：1关系 父：子
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            //新增with子项
            User o = CreateUser();
            o.UserSetting = UserSetting.New();
            o.UserSetting.Settings = "nomal";
            o.OrganizeId = Guid.NewGuid();
            if (o.IsValid)
            {
                o.Create();
            }
            var o1 = User.Get(o.Id);
            Assert.AreEqual(o1.UserSetting.Settings, "nomal");
            //编辑子项
            var o3 = User.Get(o.Id);
            o3.UserSetting.Note = "ppppppp";//检查用户信息是否延迟加载
            if (o3.IsValid)
            {
                o3.Save();
            }
            var o2 = User.Get(o.Id);
            Assert.AreEqual(o2.UserSetting.Note, "ppppppp");
        }

        /// <summary>
        /// n：1关系 只允许读取父对象，不允许在子对象中更新父对象
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            Organize o = Organize.New();
            o.Sort = 0;
            o.Name = "o1";
            o.Level = "01";
            o.Code = "00001";
            User u = CreateUser();
            User u1 = CreateUser();
            o.Users.Add(u);
            o.Users.Add(u1);
            o.Create();
            var u2 = User.Get(u.Id);
            Assert.IsNotNull(u2.Organize);
            u2.Organize.Name = "xxx";
            u2.Save();//不支持在子对象中更新父对象
            Organize o2 = Organize.Get(u2.OrganizeId);
            Assert.AreEqual(o2.Name,"o1");
        }

        /// <summary>
        /// 1：1 子：父关系 只允许读取父对象，不允许在子对象中更新父对象
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            User u = CreateUser();
            u.UserSetting = UserSetting.New();
            u.UserSetting.Settings = "s1";
            u.OrganizeId = Guid.NewGuid();
            u.Create();
            var s = UserSetting.Get(u.UserSetting.Id);
            Assert.IsNotNull(s.User);
        }

        private static User CreateUser()
        {
            User u = User.New();
            u.Abb = "hxh";
            u.Email = "hxh@126.com";
            u.Pass = "pass";
            u.PhoneNum = "13666188693";
            u.Name = "何翔华";
            u.LoginName = "00001";
            return u;
        }
    }
}
