using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Test
{

    using CAF.Model;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class UnitModel
    {
        /// <summary>
        /// 是否相等
        /// </summary>
        [TestMethod]
        public void TestMethod1()
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
        public void TestMethod2()
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
        public void TestMethod3()
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
        public void TestMethod4()
        {
            User u = new User { Abb = "hxh", Email = "hxh@126.com", OrganizeId = Guid.NewGuid(), Pass = "pass" };
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
            var u = CreateUser();
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
            var o = new Organize { Sort = 0, Name = "o1", Level = "01", Code = "00001" };
            var u = CreateUser();
            o.Users.Add(u);
            if (o.IsValid)
            {
                o.Create();
            }
            var o1 = Organize.Get(o.Id);
            Assert.AreEqual(o1.Users.Count, 1);
            //新增子项列表中子项
            var u1 = CreateUser();
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
            var o = CreateUser();
            o.UserSetting = new UserSetting();
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
            var o = new Organize { Sort = 0, Name = "o1", Level = "01", Code = "00001" };
            var u = CreateUser();
            var u1 = CreateUser();
            o.Users.Add(u);
            o.Users.Add(u1);
            o.Create();
            var u2 = User.Get(u.Id);
            Assert.IsNotNull(u2.Organize);
            u2.Organize.Name = "xxx";
            u2.Save();//不支持在子对象中更新父对象
            var o2 = Organize.Get(u2.OrganizeId);
            Assert.AreEqual(o2.Name, "o1");
        }

        /// <summary>
        /// 1：1 子：父关系 只允许读取父对象，不允许在子对象中更新父对象
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            var u = CreateUser();
            u.UserSetting = new UserSetting();
            u.UserSetting.Settings = "s1";
            u.OrganizeId = Guid.NewGuid();
            u.Create();
            var s = UserSetting.Get(u.UserSetting.Id);
            Assert.IsNotNull(s.User);
        }

        /// <summary>
        /// 测试n:n关系
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            var u = CreateUser();
            u.OrganizeId = Guid.NewGuid();
            var r = new Role { Name = "r1" };
            u.Roles.Add(r);
            u.Create();


            var u1 = User.Get(u.Id);
            Assert.AreEqual(u1.Roles.Count, 1);//create with new item
            var u2 = CreateUser();
            u2.OrganizeId = Guid.NewGuid();
            var r1 = Role.Get(u1.Roles[0].Id);
            u2.Roles.Add(r1);
            u2.Create();
            var u3 = User.Get(u2.Id);
            Assert.AreEqual(u3.Roles.Count, 1);//create with exist item
            u3.Roles[0].Name = "r2";
            u3.Save();
            var u4 = User.Get(u3.Id);
            Assert.AreEqual(u4.Roles[0].Name, "r2");//update item
            u4.Roles.Remove(u4.Roles[0]);//delete item 多对多应该移除关系
            u4.Save();
            var u5 = User.Get(u4.Id);
            Assert.AreEqual(u5.Roles.Count, 0);
            var r3 = Role.Get(r.Id);
            Assert.IsNotNull(r3);
        }

        /// <summary>
        /// 测试n:n关系[关系表为有单独属性的实体表]
        /// </summary>
        [TestMethod]
        public void TestMethod12()
        {
            var r = new Role { Name = "rr1" };
            r.Create();
            var d = new Directory { Level = "00", Name = "testdir" };
            d.Create();
            var rd = new Directory_Role() { Status = 1, RoleId = r.Id, DirectoryId = d.Id };
            rd.Create();
            var rd2 = Directory_Role.Get(r.Id, d.Id);
            Assert.IsNotNull(rd2);// create
            Assert.AreEqual(rd2.Role.Name, "rr1");
            Assert.AreEqual(rd2.Directory.Name, "testdir");
            rd2.Status = 4;
            rd2.Save();
            var rd4 = Directory_Role.Get(r.Id, d.Id);
            Assert.AreEqual(4, rd4.Status);//update
            var roles = Directory_Role.GetAllByDirectoryId(d.Id);
            Assert.AreEqual(1, roles.Count);
            var dirs = Directory_Role.GetAllByRoleId(r.Id);
            Assert.AreEqual(1, dirs.Count);
            rd4.Delete();//delete
            var rd3 = Directory_Role.Exists(r.Id, d.Id);//exist
            Assert.AreEqual(rd3, false);

        }

        /// <summary>
        /// 测试列表增减项
        /// </summary>
        [TestMethod]
        public void TestMethod11()
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
        /// 集合列表查询
        /// </summary>
        [TestMethod]
        public void TestCollectionQuery()
        {
            var users = User.GetAll();
            foreach (var user in users)
            {
                user.Delete();
            }
            for (var i = 0; i < 5; i++)
            {
                var u = CreateUser();
                u.Name = "user" + i;
                u.OrganizeId = Guid.NewGuid();
                if (u.IsValid)
                {
                    u.Create();
                }
            }
            var list = UserList.Query(new { Name = "user" }, " Name Like @Name+'%'");
            Assert.AreEqual(list.Count, 5);
            var count = UserList.QueryCount(new { Name = "user" }, " Name Like @Name+'%'");
            Assert.AreEqual(count, 5);
            var hasone = UserList.Exists(new { Name = "user" }, " Name Like @Name+'%'");
            Assert.IsTrue(hasone);
        }

        /// <summary>
        /// 只读列表分页查询
        /// </summary>
        [TestMethod]
        public void TestReadonlyList()
        {
            var o = new Organize { Name = "o1", Level = "01", Code = "0001" };
            for (var i = 0; i < 50; i++)
            {
                var u = CreateUser();
                u.Name = "user" + i;
                u.OrganizeId = Guid.NewGuid();
                o.Users.Add(u);
            }
            o.IsValid.IfIsTrue(() => o.Create());
            var readOlyBookList = ReadOnlyCollectionBase<ReadOnlyUser>.Query("Name", 2, new ReadOnlyUser { OrganizeId = o.Id },
                   sum: "Status", average: "Status", queryWhere: "   OrganizeId =@OrganizeId", pageIndex: 2);
            Assert.AreEqual(10, readOlyBookList.TotalCount);
            Assert.AreEqual(2, readOlyBookList.Result.Count());
            Assert.AreEqual(10, readOlyBookList.Sum["Status"]);
            Assert.AreEqual(1, readOlyBookList.Average["Status"]);
        }

        private static User CreateUser()
        {
            var u = new User
                         {
                             Abb = "hxh",
                             Email = "hxh@126.com",
                             Pass = "pass",
                             PhoneNum = "13666188693",
                             Name = "何翔华",
                             LoginName = "00001"
                         };
            return u;
        }
    }
}
