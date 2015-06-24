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
        public void TestGetFromDb()
        {
            var db = new Table();
            var list = db.Posts.Where(r => r.Name == "部门经理").ToList();
            Assert.IsTrue(list.Count == 1);

        }

        [TestMethod]
        public void TestUpdate()
        {
            var db = new Table();
            var list = db.Posts.ToList();
            var post = list.Where(u => u.Name == "部门经理").ToList().First();
            post.Note = "xxxxx";
            db.Posts.Update(post);
            db.Posts.Insert(new Post() { Name = "xxx" });
            db.SaveChanges();
            Assert.IsTrue(list.Count == 7);

        }





        /// <summary>
        /// 测试对象相等
        /// </summary>
        [TestMethod]
        public void TestEqual()
        {
            var id = Guid.NewGuid();
            var u1 = new Post() { Id = id, Name = "u1" };
            var u2 = new Post() { Id = id, Name = "u2" };
            var users = new List<Post> { u2 };
            Assert.AreEqual(u1, u2);//Id相等即为相等
            Assert.IsTrue(u1 == u2);
            Assert.IsFalse(u1 != u2);
            Assert.IsTrue(users.Contains(u1));
        }



        /// <summary>
        /// 测试初始化状态
        /// </summary>
        [TestMethod]
        public void TestDefaultStatus()
        {
            Post u = new Post();
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
                Post u = new Post();
                u.Validate();
            }
            catch (Warning ex)
            {

                Assert.IsNotNull(ex.Message);
            }
        }



    }
}
