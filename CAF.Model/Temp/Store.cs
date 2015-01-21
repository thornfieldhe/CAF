using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    public partial class Store : BaseEntity<Store>
    {
        private Store()
        {
            MarkNew();
            Books = new BookList();
        }

        #region 公共属性

        private string _name = String.Empty;

        [Required(ErrorMessage = "Name不允许为空")]
        [StringLength(50, ErrorMessage = "Name长度不能超过50")]
        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
        }

        public BookList Books { get; set; }

        public override bool IsValid
        {
            get
            {
                bool isValid = true;
                bool baseValid = base.IsValid;
                Errors = new List<string>();
                foreach (var item in Books)
                {
                    if (!item.IsValid)
                    {
                        this.Errors.AddRange(item.Errors);
                        isValid = false;
                    }
                }
                return baseValid && isValid;
            }
            protected set { _isValid = value; }
        }


        #endregion

        #region 常量定义

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Stores WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Stores WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Stores SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Stores WHERE Id = @Id";
        const string QUERY_INSERT = "INSERT INTO Stores (Id, Name, ChangedDate, CreatedDate, Status) VALUES (@Id, @Name, @ChangedDate, @CreatedDate, @Status)";
        const string QUERY_UPDATE = "UPDATE Stores SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static Store Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                Store item = conn.Query<Store>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Store>();
                if (item != null)
                {
                    item.MarkOld();
                    item.Books = Book.GetAllByStoreId(id);
                }
                return item;
            }
        }

        public static StoreList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Store> items = conn.Query<Store>(QUERY_GETAll, null).ToList();

                StoreList list = new StoreList();
                foreach (Store item in items)
                {
                    item.MarkOld();
                    item.Books = Book.GetAllByStoreId(item.Id);
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

        public static Store New()
        {
            return new Store();
        }

        #endregion

        internal  int Delete(IDbConnection conn, IDbTransaction transaction)
        {
            MarkDelete();
            return conn.Execute(QUERY_EXISTS, new { Id = this.Id }, transaction, null, null);
        }

        internal  int Update(IDbConnection conn, IDbTransaction transaction)
        {
            int row = 0;
            if (this.IsDirty)
            {
                _updateParameters += ", ChangedDate = GetDate()";
                _updateParameters += ", Status = @Status";
                string query = String.Format(QUERY_UPDATE, _updateParameters.TrimStart(','));
                row = conn.Execute(query, this, transaction, null, null);
                row += Books.SaveChanges(conn, transaction);
            }
            return row;
        }

        internal  int Insert(IDbConnection conn, IDbTransaction transaction)
        {
            int row = conn.Execute(QUERY_INSERT, this, transaction, null, null);
            row += Books.SaveChanges(conn, transaction);
            return row;
        }

    }

    public class StoreList : CollectionBase<StoreList, Store>
    {
        public StoreList() { }

        protected const string tableName = "Stores";

        public static StoreList Query(Object dynamicObj, string query = " AND 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Store> items = conn.Query<Store>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                StoreList list = new StoreList();
                foreach (Store item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }
                return list;
            }
        }

        public static int QueryCount(Object dynamicObj, string query = " AND 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single();
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


