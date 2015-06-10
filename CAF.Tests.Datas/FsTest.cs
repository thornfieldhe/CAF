using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Datas
{
    using System.Collections.Generic;
    using System.Linq;

    using CAF.Exceptions;
    using CAF.FS.Core.Data.Table;
    using CAF.FS.Models;
    using CAF.Utility;

    [TestClass]
    public class FsTest
    {
        [TestMethod]
        public void TestGetFromDb()
        {
            var db = new Table();
            var list = db.Users.Where(r=>r.Abb=="HXH").ToList();
            Assert.IsTrue(list.Count==1);
            
        }

        [TestMethod]
        public void TestUpdate()
        {
            var db = new Table();
            var list = db.Users.ToList();
            var user=list.Where(u => u.Abb == "HXH").First();
            user.Note = "xxxxx";
            db.Users.Update(user);
            db.Users.Insert(this.NewUser());
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
            var u1 = new User() { Id = id, Name = "u1" };
            var u2 = new User() { Id = id, Name = "u2" };
            var users = new List<User> { u2 };
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
                u.Validate();
            }
            catch (Warning ex)
            {

                Assert.IsNotNull(ex.Message);
            }
        }

        private User NewUser()
        {
            return   new User
                         {
                             Abb = "hxh",
                             Email = "hxh@126.com"+Randoms.GetRandomInt(1,10),
                             Pass = "pass",
                             PhoneNum = "13666188693",
                             Name = "何翔华",
                             LoginName = "00001",
                             OrganizeId = Guid.NewGuid()
                             
                         };
        }

    }
}
