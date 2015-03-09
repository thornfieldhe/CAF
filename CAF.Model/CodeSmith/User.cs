﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using CAF.Validation;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    [Serializable]
    public partial class User : BaseEntity<User>
    {
        public User()
        {
            Connection = SqlService.Instance.Connection;
            base.MarkNew();
            _userSettingInitalizer = new Lazy<UserSetting>(() => UserSetting.GetByUserId(Id), isThreadSafe: true);
            _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(this), isThreadSafe: true);
            Roles = new RoleList();
        }


        #region 公共属性

        private string _loginName = String.Empty;
        private string _abb = String.Empty;
        private string _name = String.Empty;
        private string _pass = String.Empty;
        private string _phoneNum = String.Empty;
        private Guid _organizeId = Guid.Empty;
        private string _email = String.Empty;
        private UserSetting _userSetting;
        private Lazy<UserSetting> _userSettingInitalizer;
        private RoleList _roleList;
        private Lazy<RoleList> _roleListInitalizer;

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名不允许为空")]
        [StringLength(20, ErrorMessage = "登录名长度不能超过20")]
        public string LoginName
        {
            get { return _loginName; }
            set { SetProperty("LoginName", ref _loginName, value); }
        }

        /// <summary>
        /// 用户简称
        /// </summary>
        [Required(ErrorMessage = "用户简称不允许为空")]
        [StringLength(20, ErrorMessage = "用户简称长度不能超过20")]
        public string Abb
        {
            get { return _abb; }
            set { SetProperty("Abb", ref _abb, value); }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage = "用户姓名不允许为空")]
        [StringLength(20, ErrorMessage = "用户姓名长度不能超过20")]
        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "用户密码不允许为空")]
        [StringLength(50, ErrorMessage = "用户密码长度不能超过50")]
        public string Pass
        {
            get { return _pass; }
            set { SetProperty("Pass", ref _pass, value); }
        }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(30, ErrorMessage = "电话长度不能超过30")]
        public string PhoneNum
        {
            get { return _phoneNum; }
            set { SetProperty("PhoneNum", ref _phoneNum, value); }
        }

        /// <summary>
        /// 组织架构Id
        /// </summary>
        [GuidRequired(ErrorMessage = "组织架构不允许为空")]
        public Guid OrganizeId
        {
            get { return _organizeId; }
            set { SetProperty("OrganizeId", ref _organizeId, value); }
        }

        /// <summary>
        /// 组织架构
        /// </summary>
        public Organize Organize
        {
            get
            {
                return Organize.Get(OrganizeId);
            }
        }


        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage = "电子邮件不允许为空")]
        [StringLength(50, ErrorMessage = "电子邮件长度不能超过50")]
        public string Email
        {
            get { return _email; }
            set { SetProperty("Email", ref _email, value); }
        }

        public UserSetting UserSetting
        {
            get
            {
                if (!_userSettingInitalizer.IsValueCreated)
                {
                    _userSetting = _userSettingInitalizer.Value;
                }
                return _userSetting;
            }
            set
            {
                if (!_userSettingInitalizer.IsValueCreated)
                {
                    _userSetting = _userSettingInitalizer.Value;
                }
                _userSetting = value;
                if (_userSetting == null)
                {
                    return;
                }
                _userSetting.OnPropertyChange += MarkDirty;
                _userSetting.UserId = Id;
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
                if (_userSettingInitalizer.IsValueCreated && UserSetting != null && !UserSetting.IsValid)
                {
                    Errors.AddRange(UserSetting.Errors);
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

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Users WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Users WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Users SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Users WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYORGANIZEID = "SELECT * FROM Sys_Users WHERE  Status!=-1 And OrganizeId=@OrganizeId";
        const string QUERY_GETALLBYROLEID = "SELECT t1.* FROM Sys_Users t1 INNER JOIN Sys_R_User_Role t2 on t1.Id=t2.UserId  where t2.RoleId=@RoleId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSUSERROLE = "SELECT COUNT(*) FROM Sys_R_User_Role WHERE  UserId = @UserId AND RoleId=@RoleId";
        const string QUERY_ADDRELARIONSHIPWITHUSERROLE = "INSERT INTO Sys_R_User_Role (UserId,RoleId,Status)VALUES(@UserId, @RoleId,0)";
        const string QUERY_DELETERELARIONSHIPWITHUSERROLE = "UPDATE Sys_R_User_Role SET Status=-1 WHERE UserId=@UserId AND RoleId=@RoleId AND Status!=-1";
        const string QUERY_INSERT = "INSERT INTO Sys_Users (Id, Status, CreatedDate, ChangedDate, Note, LoginName, Abb, Name, Pass, PhoneNum, OrganizeId, Email) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @LoginName, @Abb, @Name, @Pass, @PhoneNum, @OrganizeId, @Email)";
        const string QUERY_UPDATE = "UPDATE Sys_Users SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static User Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<User>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<User>();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                item._userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);
                item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                return item;
            }
        }

        public static UserList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<User>(QUERY_GETAll, null).ToList();
                var list = new UserList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);
                    item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }

        public static UserList GetAllByOrganizeId(Guid organizeId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<User>(QUERY_GETALLBYORGANIZEID, new { OrganizeId = organizeId }).ToList();
                var list = new UserList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);
                    item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }

        public static UserList GetAllByRoleId(Guid roleId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<User>(QUERY_GETALLBYROLEID, new { RoleId = roleId }).ToList();

                var list = new UserList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    item._userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);
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
            if (_userSettingInitalizer.IsValueCreated && UserSetting != null)
            {
                _changedRows += UserSetting.SaveChange(conn, transaction);
            }
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
            if (_userSettingInitalizer.IsValueCreated && UserSetting != null)
            {
                _changedRows += UserSetting.SaveChange(conn, transaction);
            }
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
                    _changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHUSERROLE, new { UserId = this.Id, RoleId = role.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSUSERROLE, new { UserId = Id, RoleId = role.Id }, transaction).Single() >= 1;
                    if (!isExist)
                    {
                        _changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHUSERROLE, new { UserId = Id, RoleId = role.Id }, transaction, null, null);
                    }
                }
            }
            return _changedRows;
        }

        protected static RoleList InitRoles(User user)
        {
            var list = Role.GetAllByUserId(user.Id);
            list.OnSaved += user.RelationshipWithRole;
            list.OnMarkDirty += user.MarkDirty;
            list.IsChangeRelationship = true;
            return list;
        }

        protected static UserSetting InitUserSetting(User user)
        {
            var item = UserSetting.GetByUserId(user.Id);
            if (item == null)
            {
                return item;
            }
            item.UserId = user.Id;
            item.OnPropertyChange += user.MarkDirty;
            return item;
        }
        #endregion

    }

    [Serializable]
    public class UserList : CollectionBase<UserList, User>
    {
        public UserList() { }

        protected const string tableName = "Sys_Users";

        public static UserList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<User>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new UserList();
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


