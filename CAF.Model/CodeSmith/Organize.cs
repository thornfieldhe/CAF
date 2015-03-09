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
        public Organize()
        {
            Connection = SqlService.Instance.Connection;
            base.MarkNew();
            _userListInitalizer = new Lazy<UserList>(() => InitUsers(this), isThreadSafe: true);
            _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(this), isThreadSafe: true);
            Users = new UserList();
            Roles = new RoleList();
        }


        #region 公共属性

        private string _name = String.Empty;
        private Guid? _parentId = Guid.Empty;
        private int _sort;
        private string _level = String.Empty;
        private string _code = String.Empty;
        private UserList _userList;
        private Lazy<UserList> _userListInitalizer;
        private RoleList _roleList;
        private Lazy<RoleList> _roleListInitalizer;

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
                return !ParentId.HasValue ? null : Organize.Get(ParentId.Value);
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

        public RoleList Roles
        {
            get
            {
                if (!_roleListInitalizer.IsValueCreated)
                {
                    _roleList = _roleListInitalizer.Value;
                }
                return _roleList;
            }
            internal set
            {
                _roleList = value;
            }
        }
        public override bool IsValid
        {
            get
            {
                Errors = new List<string>();
                var isValid = true;
                var baseValid = base.IsValid;
                foreach (var item in Users.Where(item => !item.IsValid))
                {
                    Errors.AddRange(item.Errors);
                    isValid = false;
                }
                _roleListInitalizer.IsValueCreated.IfIsTrue(
               () =>
               {
                   foreach (var item in Roles.Where(item => !item.IsValid))
                   {
                       Errors.AddRange(item.Errors);
                       isValid = false;
                   }
               });
                return baseValid && isValid;
            }
            protected set { _isValid = value; }
        }


        #endregion

        #region 常量定义

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Organizes WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Organizes WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Organizes SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Organizes WHERE Id = @Id";
        const string QUERY_GETALLBYROLEID = "SELECT t1.* FROM Sys_Organizes t1 INNER JOIN Sys_R_Organize_Role t2 on t1.Id=t2.OrganizeId  where t2.RoleId=@RoleId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSORGANIZEROLE = "SELECT COUNT(*) FROM Sys_R_Organize_Role WHERE  OrganizeId = @OrganizeId AND RoleId=@RoleId";
        const string QUERY_ADDRELARIONSHIPWITHORGANIZEROLE = "INSERT INTO Sys_R_Organize_Role (OrganizeId,RoleId,Status)VALUES(@OrganizeId, @RoleId,0)";
        const string QUERY_DELETERELARIONSHIPWITHORGANIZEROLE = "UPDATE Sys_R_Organize_Role SET Status=-1 WHERE OrganizeId=@OrganizeId AND RoleId=@RoleId AND Status!=-1";
        const string QUERY_INSERT = "INSERT INTO Sys_Organizes (Id, Status, CreatedDate, ChangedDate, Note, Name, ParentId, Sort, Level, Code) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @Name, @ParentId, @Sort, @Level, @Code)";
        const string QUERY_UPDATE = "UPDATE Sys_Organizes SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static Organize Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<Organize>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Organize>();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                return item;
            }
        }

        public static OrganizeList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Organize>(QUERY_GETAll, null).ToList();
                var list = new OrganizeList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }

        public static OrganizeList GetAllByRoleId(Guid roleId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Organize>(QUERY_GETALLBYROLEID, new { RoleId = roleId }).ToList();

                var list = new OrganizeList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
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

        #endregion


        internal override int Delete(IDbConnection conn, IDbTransaction transaction)
        {
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, new { Id = Id }, transaction, null, null);
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
            _userListInitalizer.IsValueCreated.IfIsTrue(
           () =>
           {
               _changedRows += Users.SaveChanges(conn, transaction);
           });
            _roleListInitalizer.IsValueCreated.IfIsTrue(
           () =>
           {
               _changedRows += Roles.SaveChanges(conn, transaction);
           });
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
            _roleListInitalizer.IsValueCreated.IfIsTrue(
           () =>
           {
               _changedRows += Roles.SaveChanges(conn, transaction);
           });
            return _changedRows;
        }

        #region 私有方法

        protected int RelationshipWithRole(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var role in Roles.Members)
            {
                if (role.IsDelete && Roles.IsChangeRelationship)
                {
                    _changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHORGANIZEROLE, new { UserId = this.Id, RoleId = role.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSORGANIZEROLE, new { OrganizeId = Id, RoleId = role.Id }, transaction).Single() >= 1;
                    if (!isExist)
                    {
                        _changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHORGANIZEROLE, new { OrganizeId = Id, RoleId = role.Id }, transaction, null, null);
                    }
                }
            }
            return _changedRows;
        }

        protected static RoleList InitRoles(Organize organize)
        {
            var roleList = Role.GetAllByOrganizeId(organize.Id);
            roleList.OnSaved += organize.RelationshipWithRole;
            roleList.OnMarkDirty += organize.MarkDirty;
            roleList.IsChangeRelationship = true;
            return roleList;
        }

        protected static UserList InitUsers(Organize organize)
        {
            var userList = User.GetAllByOrganizeId(organize.Id);
            userList.OnMarkDirty += organize.MarkDirty;
            userList.OnInsert += organize.PostAddUser;
            return userList;
        }

        protected void PostAddUser(User user)
        {
            user.OrganizeId = Id;
        }

        #endregion

    }

    [Serializable]
    public class OrganizeList : CollectionBase<OrganizeList, Organize>
    {
        public OrganizeList() { }

        protected const string tableName = "Sys_Organizes";

        public static OrganizeList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Organize>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new OrganizeList();
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


