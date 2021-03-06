﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Test
{
    using CAF.Model;

    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void TestBusinessModel()
        {
            Store s2 = Store.Get(new Guid("7CC3A0F7-54DB-4567-B50A-5669B6F890C4"));
            Assert.IsNotNull(s2);
            Assert.IsNotNull(s2.Books);
            Assert.AreEqual(s2.Books[0].Name, "xx");
        }

        [TestMethod]
        public void TestUpdate()
        {
            Book b = Book.New();
            b.Author = "hxh";
            b.Page = 5;
            b.Name = "test book";
            b.Publication = DateTime.Now;
            b.StoreId = new Guid("7CC3A0F7-54DB-4567-B50A-5669B6F890C4");
            Assert.IsTrue(b.IsValid);
            if (b.IsValid)
            {
                b.Create();
            }
            Book b2 = Book.Get(b.Id);
            Assert.IsNotNull(b2);
            b2.Name = "mm";
            b2.Name = "rr";
            b2.Page = 10;
            b2.Save();
        }

        [TestMethod]
        public void TestCollection()
        {
            BookList list = new BookList();
            Book b = Book.New();
            b.Isbn = "111";
            b.Name = "name1";
            Book c = Book.New();
            b.Isbn = "111";
            b.Name = "name1";
            list.Add(b);
            list.Add(c);
            Assert.AreEqual(list.Count, 2);
            list.RemoveAt(1);
            Assert.AreEqual(list.Count, 1);
        }

        [TestMethod]
        public void TestCollectionQuery()
        {
            BookList list = BookList.Query(new { Id = new Guid("3B7F3C81-5A63-4C7E-ADFD-24E1FE3C0F93") }, "AND Id=@Id");
            Assert.AreEqual(list[0].Name, "test book");

            int? count = BookList.QueryCount(new { Id = new Guid("3B7F3C81-5A63-4C7E-ADFD-24E1FE3C0F93") }, "AND Id=@Id");
            Assert.AreEqual(count, 1);

            bool hasone = BookList.Exists(new { Id = new Guid("3B7F3C81-5A63-4C7E-ADFD-24E1FE3C0F93") }, "AND Id=@Id");
            Assert.IsTrue(hasone);
        }

        [TestMethod]
        public void TestReadonlyList()
        {
            var readOlyBookList = ReadOnlyBookList.Instance.Query("Name", 1, new { Id = new Guid("3B7F3C81-5A63-4C7E-ADFD-24E1FE3C0F93") }, sum: "Page,Status", average: "Page,Status", queryWhere: "  Id=@Id");
            Assert.AreEqual(45, readOlyBookList.Sum["Page"]);
            Assert.AreEqual(0, readOlyBookList.Average["Status"]);
        }
    }
}
