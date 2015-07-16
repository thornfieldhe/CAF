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
        /// 推荐所有1:1关系都转成值对象
        /// </summary>
        [TestMethod]
        public void TestUpdate2()
        {
            var o1 = new Organize() { Name = "o1", Code = "01" };
            o1.Create();
            //新增子对象
            var u1 = new User()
                            {
                                Name = "001",
                                LoginName = "11",
                                Email = "a@123.com",
                                Pass = "ddd",
                                PhoneNum = "111111111111",
                                UserSetting = new UserSetting { Name = "s1" },
                                Description = new Description{Name = "p"},
                                Organize_Id = o1.Id
                            };

            u1.Create();
            var u2 = User.Get(u1.Id);
            Assert.IsNotNull(u2.UserSetting);
            //更新子对象
            u2.UserSetting.Name = "s2";
            u2.Save();
            var u3 = User.Get(u1.Id);
            Assert.AreEqual(u3.UserSetting.Name, "s2");
            //删除子对象(不支持删除子对象)
            //u3.UserSetting.Delete();

            var uu1 = UserSetting.Get(u3.UserSetting.Id);
            uu1.Delete();
            var uu2 = UserSetting.Get(uu1.Id);
            Assert.IsNull(uu2);
            var u4 = User.Get(u1.Id);
            Assert.IsNull(u4.UserSetting);//支持子对象自身删除

        }

        /// <summary>
        /// 1:n对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate3()
        {
            var id = Guid.NewGuid();
            //新增子对象
            var o1 = new Organize(id)
                         {
                             Name = "o1",
                             Code = "01",
                             Users =
                                 new List<User>
                                     {
                                         new User()
                                             {
                                                 Name = "001",
                                                 LoginName = "11",
                                                 Email = "a@123.com",
                                                 Pass = "ddd",
                                                 PhoneNum = "111111111111",
                                                 Organize_Id = id,
                                                 Description = new Description(){Name = "op"}
                                             }
                                     }
                         };
            o1.Create();
            var o2 = Organize.Get(o1.Id);
            Assert.AreEqual(1, o1.Users.Count);
            //更新子对象
            o1.Users[0].Name = "002";
            o1.Save();
            var o3 = Organize.Get(o2.Id);
            Assert.AreEqual("002", o1.Users[0].Name);
            //移除子对象
            //如果user表中organize_id不允许为空则不允许移除
            //否则可以移除
//            o3.Users.RemoveAt(0);
//            o3.Save();
//            var o4 = Organize.Get(o2.Id);
//            Assert.AreEqual(0, o4.Users.Count);
//            var u1 = User.Get(o2.Id);
//            Assert.IsNull(u1);
            //删除子对象
           
            Assert.AreEqual(1, o3.Users.Count);
            var u2 = User.Get(o3.Users[0].Id);
            u2.Delete();//子对象执行删除后，父对象就查询不到了
            var o6 = Organize.Get(o2.Id);
            Assert.AreEqual(0, o6.Users.Count);

        }

        /// <summary>
        /// n:1对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate4()
        {
            //更新父对象
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var o1 = new Organize(id)
            {
                Name = "o1",
                Code = "01",
                Users =
                    new List<User>
                                     {
                                         new User(id2)
                                             {
                                                 Name = "001",
                                                 LoginName = "11",
                                                 Email = "a@123.com",
                                                 Pass = "ddd",
                                                 PhoneNum = "111111111111",
                                                 Organize_Id = id,
                                                 Description = new Description(){Name = "op"}
                                             }
                                     }
            };
            o1.Create();
            var u1 = User.Get(id2);
            u1.Organize.Name = "pp";
            u1.Save();
            var o2 = Organize.Get(id);
            Assert.AreEqual(o2.Name,"pp");
        }


        /// <summary>
        /// n:1对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate5()
        {
            var o1 = new Organize() { Name = "o1", Code = "01" };
            o1.Create();
            //新增关系对象
            var id= Guid.NewGuid();
            var u1 = new User(id)
                         {
                             Name = "001",
                             LoginName = "11",
                             Email = "a@123.com",
                             Pass = "ddd",
                             PhoneNum = "111111111111",
                             Description = new Description() { Name = "dfd" },
                             Roles = new List<Role>() { new Role() { Name = "r1" } },
                             Organize_Id = o1.Id
                         };
            u1.Create();
            var u2 = User.Get(id);
            Assert.AreEqual(u2.Roles.Count,1);
            //更新关系对象
            u2.Roles[0].Name = "r2";
            u2.Save();
            var u3 = User.Get(id);
            Assert.AreEqual(u3.Roles[0].Name,"r2");
            var rid = u3.Roles[0].Id;
            //删除关系
            u3.Roles.RemoveAt(0);
            u3.Save();
            var u4 = User.Get(id);
            Assert.AreEqual(u4.Roles.Count,0);
            var r1 = Role.Get(rid);
            Assert.IsNotNull(r1);//只删除关系不删除对象
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
