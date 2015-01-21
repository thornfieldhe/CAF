using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using CAF.Validation;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    public partial class Book : BaseEntity<Book>
    {
        private Book()
        {
            MarkNew();
        }

        #region 公共属性

        private string _name = String.Empty;
        private string _isbn = String.Empty;
        private int _page;
        private string _author = String.Empty;
        private DateTime _publication;
        private Guid _storeId = Guid.Empty;
        private DateTime? _testDate;

        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
        }

        public string Isbn
        {
            get { return _isbn; }
            set { SetProperty("Isbn", ref _isbn, value); }
        }

        [Required(ErrorMessage = "Page不允许为空")]
        public int Page
        {
            get { return _page; }
            set { SetProperty("Page", ref _page, value); }
        }

        [Required(ErrorMessage = "Author不允许为空")]
        [StringLength(200, ErrorMessage = "Author长度不能超过200")]
        public string Author
        {
            get { return _author; }
            set { SetProperty("Author", ref _author, value); }
        }

        [Required(ErrorMessage = "Publication不允许为空")]
        [DateTimeRequired(ErrorMessage = "Publication不允许为空")]
        public DateTime Publication
        {
            get { return _publication; }
            set { SetProperty("Publication", ref _publication, value); }
        }

        [Required(ErrorMessage = "Store不允许为空")]
        [GuidRequired(ErrorMessage = "Store不允许为空")]
        public Guid StoreId
        {
            get { return _storeId; }
            set { SetProperty("StoreId", ref _storeId, value); }
        }

        public Store Store
        {
            get { return Store.Get(this.StoreId); }
        }

        /// <summary>
        /// ddd
        /// </summary>
        [DateTimeRequired(ErrorMessage = "ddd不允许为空")]
        public DateTime? TestDate
        {
            get { return _testDate; }
            set { SetProperty("TestDate", ref _testDate, value); }
        }


        #endregion

        #region 常量定义

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Books WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Books WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Books SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Books WHERE Id = @Id";
        const string QUERY_GETALLBYSTOREID = "SELECT * FROM Books WHERE  Status!=-1 And StoreId=@StoreId";
        const string QUERY_INSERT = "INSERT INTO Books (Id, Name, Isbn, Page, Author, Publication, CreatedDate, ChangedDate, Status, StoreId, TestDate) VALUES (@Id, @Name, @Isbn, @Page, @Author, @Publication, @CreatedDate, @ChangedDate, @Status, @StoreId, @TestDate)";
        const string QUERY_UPDATE = "UPDATE Books SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static Book Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                Book item = conn.Query<Book>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Book>();
                if (item != null)
                {
                    item.MarkOld();
                }
                return item;
            }
        }

        public static BookList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Book> items = conn.Query<Book>(QUERY_GETAll, null).ToList();

                BookList list = new BookList();
                foreach (Book item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }

                return list;
            }
        }

        public static BookList GetAllByStoreId(Guid storeId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Book> items = conn.Query<Book>(QUERY_GETALLBYSTOREID, new { StoreId = storeId }).ToList();

                BookList list = new BookList();
                foreach (Book item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }
                return list;
            }
        }


        /// <summary>
        /// 直接删除
        /// </summary>
        /// <returns></returns>
        public static int Delete(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Execute(QUERY_DELETE, new { Id = id });
            }
        }

        public static bool Exists(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(QUERY_EXISTS, new { Id = id }).Single() >= 1;
            }
        }

        public static Book New()
        {
            return new Book();
        }

        #endregion

        internal override int Delete(IDbConnection conn, IDbTransaction transaction)
        {
            MarkDelete();
            return conn.Execute(QUERY_EXISTS, new { Id = this.Id }, transaction, null, null);
        }

        internal override int Update(IDbConnection conn, IDbTransaction transaction)
        {
            int row = 0;
            if (this.IsDirty)
            {
                _updateParameters += ", ChangedDate = GetDate()";
                _updateParameters += ", Status = @Status";
                string query = String.Format(QUERY_UPDATE, _updateParameters.TrimStart(','));
                row = conn.Execute(query, this, transaction, null, null);
            }
            return row;
        }

        internal override int Insert(IDbConnection conn, IDbTransaction transaction)
        {
            int row = conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return row;
        }

    }

    public class BookList : CollectionBase<BookList, Book>
    {
        public BookList() { }

        protected const string tableName = "Books";

        public static BookList Query(Object dynamicObj, string query = " AND 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Book> items = conn.Query<Book>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                BookList list = new BookList();
                foreach (Book item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }
                return list;
            }
        }

        public static int? QueryCount(Object dynamicObj, string query = " AND 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).SingleOrDefault();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " AND 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single() > 0;
            }
        }
    }
}


