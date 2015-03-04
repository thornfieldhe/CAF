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
        public Role()
        {
            base.MarkNew();
            _userListInitalizer = new Lazy<UserList>(() => InitUsers(this), isThreadSafe: true);
            _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(this), isThreadSafe: true);
            Users = new UserList();
            Organizes = new OrganizeList();
        }


        #region 公共属性

        private string _name = String.Empty;
        private UserList _userList;
        private Lazy<UserList> _userListInitalizer;
        private OrganizeList _organizeList;
        private Lazy<OrganizeList> _organizeListInitalizer;

        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(20, ErrorMessage = "角色名称长度不能超过20")]
        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
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
        public OrganizeList Organizes
        {
            get
            {
                if (!_organizeListInitalizer.IsValueCreated)
                {
                    _organizeList = _organizeListInitalizer.Value;
                }
                return _organizeList;
            }
            internal set
            {
                _organizeList = value;
            }
        }
        public override bool IsValid
        {
            get
            {
                Errors = new List<string>();
                var isValid = true;
                var baseValid = base.IsValid;
                _userListInitalizer.IsValueCreated.IfIsTrue(
               () =>
               {
                   foreach (var item in Users.Where(item => !item.IsValid))
                   {
                       Errors.AddRange(item.Errors);
                       isValid = false;
                   }
               });
                _organizeListInitalizer.IsValueCreated.IfIsTrue(
               () =>
               {
                   foreach (var item in Organizes.Where(item => !item.IsValid))
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

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Roles WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Roles WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Roles SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Roles WHERE Id = @Id";
        const string QUERY_GETALLBYUSERID = "SELECT t1.* FROM Sys_Roles t1 INNER JOIN Sys_R_User_Role t2 on t1.Id=t2.RoleId  where t2.UserId=@UserId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSUSERROLE = "SELECT COUNT(*) FROM Sys_R_User_Role WHERE  RoleId = @RoleId AND UserId=@UserId";
        const string QUERY_ADDRELARIONSHIPWITHUSERROLE = "INSERT INTO Sys_R_User_Role (RoleId,UserId,Status)VALUES(@RoleId, @UserId,0)";
        const string QUERY_DELETERELARIONSHIPWITHUSERROLE = "UPDATE Sys_R_User_Role SET Status=-1 WHERE RoleId=@RoleId AND UserId=@UserId AND Status!=-1";
        const string QUERY_GETALLBYORGANIZEID = "SELECT t1.* FROM Sys_Roles t1 INNER JOIN Sys_R_Organize_Role t2 on t1.Id=t2.RoleId  where t2.OrganizeId=@OrganizeId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSORGANIZEROLE = "SELECT COUNT(*) FROM Sys_R_Organize_Role WHERE  RoleId = @RoleId AND OrganizeId=@OrganizeId";
        const string QUERY_ADDRELARIONSHIPWITHORGANIZEROLE = "INSERT INTO Sys_R_Organize_Role (RoleId,OrganizeId,Status)VALUES(@RoleId, @OrganizeId,0)";
        const string QUERY_DELETERELARIONSHIPWITHORGANIZEROLE = "UPDATE Sys_R_Organize_Role SET Status=-1 WHERE RoleId=@RoleId AND OrganizeId=@OrganizeId AND Status!=-1";
        const string QUERY_INSERT = "INSERT INTO Sys_Roles (Id, Status, CreatedDate, ChangedDate, Note, Name) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @Name)";
        const string QUERY_UPDATE = "UPDATE Sys_Roles SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static Role Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<Role>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Role>();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                item._organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                return item;
            }
        }

        public static RoleList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Role>(QUERY_GETAll, null).ToList();
                var list = new RoleList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    item._organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
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
                var items = conn.Query<Role>(QUERY_GETALLBYUSERID, new { UserId = userId }).ToList();

                var list = new RoleList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    item._organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }

        public static RoleList GetAllByOrganizeId(Guid organizeId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Role>(QUERY_GETALLBYORGANIZEID, new { OrganizeId = organizeId }).ToList();

                var list = new RoleList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    item._organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
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
            _organizeListInitalizer.IsValueCreated.IfIsTrue(
           () =>
           {
               _changedRows += Organizes.SaveChanges(conn, transaction);
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
            _organizeListInitalizer.IsValueCreated.IfIsTrue(
           () =>
           {
               _changedRows += Organizes.SaveChanges(conn, transaction);
           });
            return _changedRows;
        }

        #region 私有方法

        protected int RelationshipWithUser(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var user in Users.Members)
            {
                if (user.IsDelete && Users.IsChangeRelationship)
                {
                    _changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHUSERROLE, new { UserId = this.Id, RoleId = user.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSUSERROLE, new { RoleId = Id, UserId = user.Id }, transaction).Single() >= 1;
                    if (!isExist)
                    {
                        _changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHUSERROLE, new { RoleId = Id, UserId = user.Id }, transaction, null, null);
                    }
                }
            }
            return _changedRows;
        }

        protected static UserList InitUsers(Role role)
        {
            var userList = User.GetAllByRoleId(role.Id);
            userList.OnSaved += role.RelationshipWithUser;
            userList.OnMarkDirty += role.MarkDirty;
            userList.IsChangeRelationship = true;
            return userList;
        }

        protected int RelationshipWithOrganize(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var organize in Organizes.Members)
            {
                if (organize.IsDelete && Organizes.IsChangeRelationship)
                {
                    _changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHORGANIZEROLE, new { UserId = this.Id, RoleId = organize.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSORGANIZEROLE, new { RoleId = Id, OrganizeId = organize.Id }, transaction).Single() >= 1;
                    if (!isExist)
                    {
                        _changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHORGANIZEROLE, new { RoleId = Id, OrganizeId = organize.Id }, transaction, null, null);
                    }
                }
            }
            return _changedRows;
        }

        protected static OrganizeList InitOrganizes(Role role)
        {
            var organizeList = Organize.GetAllByRoleId(role.Id);
            organizeList.OnSaved += role.RelationshipWithOrganize;
            organizeList.OnMarkDirty += role.MarkDirty;
            organizeList.IsChangeRelationship = true;
            return organizeList;
        }

        #endregion

    }

    [Serializable]
    public class RoleList : CollectionBase<RoleList, Role>
    {
        public RoleList() { }

        protected const string tableName = "Sys_Roles";

        public static RoleList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Role>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new RoleList();
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


