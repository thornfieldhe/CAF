using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Tests.Datas
{
    using CAF.Exceptions;
    using CAF.Models;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class FsTest
    {

        [TestMethod]
        public void TestInsert()
        {
            var id = Guid.NewGuid();
            var test = new Post(id) { Name = "mmm" };
            test.Create();
            Assert.AreEqual(1, Post.Get(r => r.Id == id).Count());
        }

        [TestMethod]
        public void TestUpdate()
        {
            var id = Guid.NewGuid();
            var t1 = new Post(id) { Name = "tttt" };
            t1.Create();
            var t2 = Post.Get(id);
            t2.Name = "11111";
            t2.Save();
            Assert.AreEqual(Post.Get(id).Name, "11111");
        }

        [TestMethod]
        public void TestDelete()
        {
            var id = Guid.NewGuid();
            var t1 = new Post(id) { Name = "tttt" };
            t1.Create();
            var t2 = Post.Get(id);
            Assert.IsNotNull(t2);
            t2.Delete();
            Assert.IsNull(Post.Get(t2.Id));//实例删除


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
            var test = new Post(id) { Name = "xxxx" };
            test.Create();
            Assert.IsNotNull(Post.Get(id));//获取单条数据
            Assert.IsTrue(Post.GetAll().Any());//获取条件数据
        }

        /// <summary>
        /// 测试从缓存中读取数据
        /// </summary>
        [TestMethod]
        public void TestCache()
        {
            var count1 = Post.GetAll(true).Count;
            Assert.IsTrue(count1 == 0);//缓存读取
            var d = new Post() { Name = "abc" };
            d.Create();//新增
            var count2 = Post.GetAll(true).Count;
            Assert.AreEqual(count1, count2);//缓存未更新
            var count3 = Post.GetAll(false).Count;
            Assert.AreNotEqual(count1, count3);//数据库值
        }

        /// <summary>
        /// 1:1对象编辑
        /// 推荐所有1:1关系都转成值对象
        /// </summary>
        [TestMethod]
        public void TestUpdate2()
        {

            //新增子对象
            var u1 = new User()
                            {
                                Name = "001",
                                LoginName = "11",
                                Email = "a@123.com",
                                Pass = "ddd",
                                PhoneNum = "111111111111",
                                UserSetting = new UserSetting { Name = "s1" },
                                Description = new Description { Name = "p" }
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
            Assert.IsNotNull(u4.UserSetting);//不支持子对象自身删除

        }

        /// <summary>
        /// 1:n对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate3()
        {
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            //新增子对象
            var u1 = new User(id)

                        {
                            Name = "001",
                            LoginName = "11",
                            Email = "a@123.com",
                            Pass = "ddd",
                            PhoneNum = "111111111111",
                            Description = new Description() { Name = "op" },
                            UserNotes = new List<UserNote> { new UserNote(id2) { Desc = "pp", User_Id = id2 } }
                        };
            u1.Create();
            var u2 = User.Get(u1.Id);
            Assert.AreEqual(1, u2.UserNotes.Count);
            //更新子对象
            u2.UserNotes[0].Desc = "002";
            u2.Save();
            var u3 = User.Get(u2.Id);
            Assert.AreEqual("002", u3.UserNotes[0].Desc);
            //只支持移除子对象
            //如果usernote表中user_id允许为空
            //移除只是去掉关系，不删除对象
            var n0 = u3.UserNotes[0].Id;
            u3.UserNotes.RemoveAt(0);
            u3.Save();
            var u4 = User.Get(u2.Id);
            Assert.AreEqual(0, u4.UserNotes.Count);//移除关系
            var n1 = UserNote.Get(n0);
            Assert.IsNotNull(n1);
            n1.User_Id = u2.Id;
            n1.Save();
            var u5 = User.Get(u2.Id);
            Assert.AreEqual(1, u5.UserNotes.Count);
            var n2 = UserNote.Get(u5.UserNotes[0].Id);
            n2.Delete();
            var n3 = UserNote.Get(n0);
            Assert.IsNull(n3);
            var u6 = User.Get(u2.Id);
            Assert.AreEqual(1, u6.UserNotes.Count);//状态为-1的只要关系存在就不意味着删除
           

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
            var o1 =
                                         new User(id)
                                             {
                                                 Name = "001",
                                                 LoginName = "11",
                                                 Email = "a@123.com",
                                                 Pass = "ddd",
                                                 PhoneNum = "111111111111",
                                                 Description = new Description() { Name = "op" },
                                                 UserNotes = new List<UserNote> { new UserNote(id2) { Desc = "pp", User_Id = id } }
                                             };
            o1.Create();
            var u1 = UserNote.Get(id2);
            u1.User.Name = "pp";
            u1.Save();
            var o2 = User.Get(id);
            Assert.AreEqual(o2.Name, "pp");
        }


        /// <summary>
        /// n:1对象编辑
        /// </summary>
        [TestMethod]
        public void TestUpdate5()
        {
            //新增关系对象
            var id = Guid.NewGuid();
            var u1 = new User(id)
                         {
                             Name = "001",
                             LoginName = "11",
                             Email = "a@123.com",
                             Pass = "ddd",
                             PhoneNum = "111111111111",
                             Description = new Description() { Name = "dfd" },
                             Roles = new List<Role>() { new Role() { Name = "r1" } },
                         };
            u1.Create();
            var u2 = User.Get(id);
            Assert.AreEqual(u2.Roles.Count, 1);
            //更新关系对象
            u2.Roles[0].Name = "r2";
            u2.Save();
            var u3 = User.Get(id);
            Assert.AreEqual(u3.Roles[0].Name, "r2");
            var rid = u3.Roles[0].Id;
            //删除关系
            u3.Roles.RemoveAt(0);
            u3.Save();
            var u4 = User.Get(id);
            Assert.AreEqual(u4.Roles.Count, 0);
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
            var t1 = new User(id) { Name = "u1" };
            var t2 = new User(id) { Name = "u2" };
            var users = new List<User> { t2 };
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
            var u = new User();
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
                var u = new User();
                u.Validate();
            }
            catch (Warning ex)
            {
                Assert.IsNotNull(ex.Message);
            }
        }



    }
}
