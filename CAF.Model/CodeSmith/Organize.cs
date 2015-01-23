using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    [Serializable]
    public partial class Organize : BaseEntity<Organize>
    {
        private Organize() : this(Guid.NewGuid()) { }

        public Organize(Guid id)
            : base(id)
        {
            base.MarkNew();
            Users = new UserList();
        }

        #region 公共属性

        private string _name = String.Empty;
        private Guid? _parentId = Guid.Empty;
        private int _sort;
        private string _level = String.Empty;
        private string _code = String.Empty;
        private UserList _userList;
        private Lazy<UserList> _userListInitalizer;

        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(ErrorMessage = "部门名称不允许为空")]
        [StringLength(50, ErrorMessage = "部门名称长度不能超过50")]
        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
        }

        /// <summary>
        /// 父部门Id
        /// </summary>
        public Guid? ParentId
        {
            get { return _parentId; }
            set { SetProperty("ParentId", ref _parentId, value); }
        }

        /// <summary>
        /// 父部门
        /// </summary>
        public Organize Parent
        {
            get
            {
                return !this.ParentId.HasValue ? null : Organize.Get(this.ParentId.Value);
            }
        }


        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不允许为空")]
        public int Sort
        {
            get { return _sort; }
            set { SetProperty("Sort", ref _sort, value); }
        }

        /// <summary>
        /// 部门层级
        /// </summary>
        [Required(ErrorMessage = "部门层级不允许为空")]
        [StringLength(20, ErrorMessage = "部门层级长度不能超过20")]
        public string Level
        {
            get { return _level; }
            set { SetProperty("Level", ref _level, value); }
        }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "编码不允许为空")]
        [StringLength(50, ErrorMessage = "编码长度不能超过50")]
        public string Code
        {
            get { return _code; }
            set { SetProperty("Code", ref _code, value); }
        }

        public UserList Users
        {
            get
            {
                if (!_userListInitalizer.IsValueCreated)
                {
                    _userList = _userListInitalizer.Value;
                }
                return _userList;
            }
            internal set
            {
                _userList = value;
            }
        }

        public override bool IsValid
        {
            get
            {
                this.Errors = new List<string>();
                bool isValid = true;
                bool baseValid = base.IsValid;
                foreach (var item in Users)
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

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Organize WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Organize WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Organize SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Organize WHERE Id = @Id";
        const string QUERY_INSERT = "INSERT INTO Sys_Organize (Id, Status, CreatedDate, ChangedDate, Note, Name, ParentId, Sort, Level, Code) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @Name, @ParentId, @Sort, @Level, @Code)";
        const string QUERY_UPDATE = "UPDATE Sys_Organize SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static Organize Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                Organize item = conn.Query<Organize>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Organize>();
                if (item != null)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                }
                return item;
            }
        }

        public static OrganizeList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Organize> items = conn.Query<Organize>(QUERY_GETAll, null).ToList();
                OrganizeList list = new OrganizeList();
                foreach (Organize item in items)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
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

        public static Organize New()
        {
            var item = new Organize();
            item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
            return item;
        }

        #endregion

        internal override int Delete(IDbConnection conn, IDbTransaction transaction)
        {
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, new { Id = this.Id }, transaction, null, null);
        }

        internal override int Update(IDbConnection conn, IDbTransaction transaction)
        {
            if (this.IsDirty)
            {
                _updateParameters += ", ChangedDate = GetDate()";
                _updateParameters += ", Status = @Status";
                string query = String.Format(QUERY_UPDATE, _updateParameters.TrimStart(','));
                _changedRows += conn.Execute(query, this, transaction, null, null);
                _userListInitalizer.IsValueCreated.IfIsTrue(
               () =>
               {
                   _changedRows += Users.SaveChanges(conn, transaction);
               });
            }
            return _changedRows;
        }

        internal override int Insert(IDbConnection conn, IDbTransaction transaction)
        {
            _changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            _userListInitalizer.IsValueCreated.IfIsTrue(
           () =>
           {
               _changedRows += Users.SaveChanges(conn, transaction);
           });
            return _changedRows;
        }

        #region 私有方法

        protected static UserList InitUsers(Organize organize)
        {
            var userList = User.GetAllByOrganizeId(organize.Id);
            userList.OnMarkDirty += organize.MarkDirty;
            userList.OnInsert += organize.PostAddUser;
            return userList;
        }

        protected void PostAddUser(User user)
        {
            user.OrganizeId = this.Id;
        }

        #endregion

    }

    [Serializable]
    public class OrganizeList : CollectionBase<OrganizeList, Organize>
    {
        public OrganizeList() { }

        protected const string tableName = "Sys_Organize";

        public static OrganizeList Query(Object dynamicObj, string query = " AND 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Organize> items = conn.Query<Organize>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                OrganizeList list = new OrganizeList();
                foreach (Organize item in items)
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


