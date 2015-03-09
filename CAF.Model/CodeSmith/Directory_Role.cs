
namespace CAF.Model.CodeSmith
{
    using CAF.Data;
    using CAF.Validation;
    using System;
    using System.Data;
    using System.Linq;

    public class Directory_Role : BaseEntity<Directory_Role>
    {
        public Directory_Role()
        {
            Connection = SqlService.Instance.Connection;
            base.MarkNew();
            _directoryInitalizer = new Lazy<Directory>(() => Directory.Get(Id), isThreadSafe: true);
            _roleInitalizer = new Lazy<Role>(() => Role.Get(Id), isThreadSafe: true);
        }


        #region 公共属性

        /// <summary>
        /// 角色Id
        /// </summary>
        [GuidRequired(ErrorMessage = "角色不允许为空")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 目录Id
        /// </summary>
        [GuidRequired(ErrorMessage = "目录不允许为空")]
        public Guid DirectoryId { get; set; }



        private Lazy<Directory> _directoryInitalizer;
        private Lazy<Role> _roleInitalizer;
        private Role _role;
        private Directory _directory;


        public Role Role
        {
            get
            {
                if (!_roleInitalizer.IsValueCreated)
                {
                    _role = _roleInitalizer.Value;
                }
                return _role;
            }
            set
            {
                if (!_roleInitalizer.IsValueCreated)
                {
                    _role = _roleInitalizer.Value;
                }
                _role = value;
                if (_role == null)
                {
                    return;
                }
                _role.OnPropertyChange += MarkDirty;
            }
        }

        public Directory Directory
        {
            get
            {
                if (!_directoryInitalizer.IsValueCreated)
                {
                    _directory = _directoryInitalizer.Value;
                }
                return _directory;
            }
            set
            {
                if (!_directoryInitalizer.IsValueCreated)
                {
                    _directory = _directoryInitalizer.Value;
                }
                _directory = value;
                if (_directory == null)
                {
                    return;
                }
                _directory.OnPropertyChange += MarkDirty;
            }
        }



        #endregion

        #region 常量定义

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_RE_Directory_Role WHERE RoleId = @RoleId AND DirectoryId=@DirectoryId AND Status!=-1";
        const string QUERY_GETAllROLES = "SELECT t1.* FROM Sys_Roles t1 INNER JOIN Sys_RE_Directory_Role t2 ON t1.ID=t2.RoleId WHERE t2.DirectoryId=@DirectoryId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_GETAllDIRECTORIES = "SELECT t1.* FROM Sys_Directories t1 INNER JOIN Sys_RE_Directory_Role t2 ON t1.ID=t2.DirectoryId WHERE t2.RoleId=@RoleId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_RE_Directory_Role SET Status=-1 WHERE RoleId = @RoleId AND DirectoryId=@DirectoryId AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_RE_Directory_Role WHERE RoleId = @RoleId AND DirectoryId=@DirectoryId AND  Status!=-1";
        const string QUERY_INSERT = "INSERT INTO Sys_RE_Directory_Role (Id,Status, CreatedDate, ChangedDate, Note, DirectoryId,RoleId) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @DirectoryId,@RoleId)";
        const string QUERY_UPDATE = "UPDATE Sys_RE_Directory_Role SET {0} WHERE  RoleId = @RoleId AND DirectoryId=@DirectoryId";

        #endregion

        #region 静态方法

        public static Directory_Role Get(Guid roleId, Guid directoryId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<Directory_Role>(QUERY_GETBYID, new { RoleId = roleId, DirectoryId = directoryId }).SingleOrDefault<Directory_Role>();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                item._roleInitalizer = new Lazy<Role>(() => InitRole(item), isThreadSafe: true);
                item._directoryInitalizer = new Lazy<Directory>(() => InitDirectory(item), isThreadSafe: true);
                return item;
            }
        }



        public static Directory_RoleList GetDirectoriesByRoleId(Guid roleId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory_Role>(QUERY_GETAllDIRECTORIES, new { RoleId = roleId }).ToList();

                var list = new Directory_RoleList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._roleInitalizer = new Lazy<Role>(() => InitRole(item), isThreadSafe: true);
                    item._directoryInitalizer = new Lazy<Directory>(() => InitDirectory(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }

        public static Directory_RoleList GetRolesByDirectoryId(Guid directoryId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory_Role>(QUERY_GETAllROLES, new { DirectoryId = directoryId }).ToList();

                var list = new Directory_RoleList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._roleInitalizer = new Lazy<Role>(() => InitRole(item), isThreadSafe: true);
                    item._directoryInitalizer = new Lazy<Directory>(() => InitDirectory(item), isThreadSafe: true);
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
        public static int Delete(Guid roleId, Guid directoryId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Execute(QUERY_DELETE, new { RoleId = roleId, DirectoryId = directoryId });
            }
        }

        public static bool Exists(Guid roleId, Guid directoryId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(QUERY_EXISTS, new { RoleId = roleId, DirectoryId = directoryId }).Single() >= 1;
            }
        }

        #endregion


        internal override int Delete(IDbConnection conn, IDbTransaction transaction)
        {
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, new { RoleId = RoleId, DirectoryId = DirectoryId }, transaction, null, null);
        }

        internal override int Update(IDbConnection conn, IDbTransaction transaction)
        {
            if (!IsDirty)
            {
                return _changedRows;
            }
            _updateParameters += ", ChangedDate = GetDate()";
            var query = String.Format(QUERY_UPDATE, _updateParameters.TrimStart(','));
            _changedRows += conn.Execute(query, this, transaction, null, null);
            return _changedRows;
        }

        internal override int Insert(IDbConnection conn, IDbTransaction transaction)
        {
            _changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return _changedRows;
        }

        #region 私有方法


        protected static Role InitRole(Directory_Role item)
        {
            var roel = Role.Get(item.RoleId);
            if (roel != null)
            {
                roel.OnPropertyChange += item.MarkDirty;
            }
            return roel;
        }

        protected static Directory InitDirectory(Directory_Role item)
        {
            var directory = Directory.Get(item.DirectoryId);
            if (directory != null)
            {
                directory.OnPropertyChange += item.MarkDirty;
            }
            return directory;
        }


        #endregion
    }

    [Serializable]
    public class Directory_RoleList : CollectionBase<Directory_RoleList, Directory_Role>
    {
        public Directory_RoleList() { }

        protected const string tableName = "Sys_RE_Directory_RoleList";

        public static Directory_RoleList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory_Role>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new Directory_RoleList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }
                return list;
            }
        }

        public static int QueryCount(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single() > 0;
            }
        }
    }
}
