using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using CAF.Validation;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    [Serializable]
    public partial class Role : BaseEntity<Role>
    {
        private Role() : this(Guid.NewGuid()) { }

        public Role(Guid id)
            : base(id)
        {
            base.MarkNew();
            Users = new UserList();
        }

        #region 公共属性

        private string _name = String.Empty;
        private Guid? _parentId = Guid.Empty;
        private UserList _userList;
        private Lazy<UserList> _userListInitalizer;

        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(20, ErrorMessage = "角色名称长度不能超过20")]
        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
        }

        /// <summary>
        /// 父角色Id
        /// </summary>
        public Guid? ParentId
        {
            get { return _parentId; }
            set { SetProperty("ParentId", ref _parentId, value); }
        }

        /// <summary>
        /// 父角色
        /// </summary>
        public Role Parent
        {
            get
            {
                return !this.ParentId.HasValue ? null : Role.Get(this.ParentId.Value);
            }
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
                _userListInitalizer.IsValueCreated.IfIsTrue(
               () =>
               {
                   foreach (var item in this.Users.Where(item => !item.IsValid))
                   {
                       this.Errors.AddRange(item.Errors);
                       isValid = false;
                   }
               });
                return baseValid && isValid;
            }
            protected set { _isValid = value; }
        }


        #endregion

        #region 常量定义

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Role WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Role WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Role SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Role WHERE Id = @Id";
        const string QUERY_GETALLBYUSERID = "SELECT t1.* FROM Sys_Role t1 INNER JOIN Sys_R_User_Role t2 on t1.Id=t2.RoleId  where t2.UserId=@UserId AND t1.Status!=-1";
        const string QUERY_CONTAINSUSERROLE = "SELECT COUNT(*) FROM Sys_R_User_Role WHERE  RoleId = @RoleId AND UserId=@UserId";
        const string QUERY_ADDRELARIONSHIPWITHUSERROLE = "INSERT INTO Sys_R_User_Role (RoleId,UserId)VALUES(@RoleId, @UserId)";
        const string QUERY_INSERT = "INSERT INTO Sys_Role (Id, Status, CreatedDate, ChangedDate, Note, Name, ParentId) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @Name, @ParentId)";
        const string QUERY_UPDATE = "UPDATE Sys_Role SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static Role Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                Role item = conn.Query<Role>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Role>();
                if (item != null)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                }
                return item;
            }
        }

        public static RoleList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Role> items = conn.Query<Role>(QUERY_GETAll, null).ToList();
                RoleList list = new RoleList();
                foreach (Role item in items)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }

        public static RoleList GetAllByUserId(Guid userId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Role> items = conn.Query<Role>(QUERY_GETALLBYUSERID, new { UserId = userId }).ToList();

                RoleList list = new RoleList();
                foreach (Role item in items)
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

        public static Role New()
        {
            var item = new Role();
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

        protected int AddRelationshipWithUser(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var user in this.Users)
            {
                var isExist = conn.Query<int>(QUERY_CONTAINSUSERROLE, new { RoleId = this.Id, UserId = user.Id }, transaction).Single() >= 1;
                if (!isExist)
                {
                    _changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHUSERROLE, new { RoleId = this.Id, UserId = user.Id }, transaction, null, null);
                }
            }
            return _changedRows;
        }

        protected static UserList InitUsers(Role role)
        {
            var userList = User.GetAllByRoleId(role.Id);
            userList.OnSaved += role.AddRelationshipWithUser;
            userList.OnMarkDirty += role.MarkDirty;
            return userList;
        }

        #endregion

    }

    [Serializable]
    public class RoleList : CollectionBase<RoleList, Role>
    {
        public RoleList() { }

        protected const string tableName = "Sys_Role";

        public static RoleList Query(Object dynamicObj, string query = "  1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                List<Role> items = conn.Query<Role>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                RoleList list = new RoleList();
                foreach (Role item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }
                return list;
            }
        }

        public static int QueryCount(Object dynamicObj, string query = "  1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = "  1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single() > 0;
            }
        }
    }
}


