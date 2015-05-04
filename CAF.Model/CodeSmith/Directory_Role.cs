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
	public partial class Directory_Role :  BaseEntity<Directory_Role>
	{   
        public Directory_Role()
        {
            this.Connection = SqlService.Instance.Connection;
            base.MarkNew();
            this. _roleInitalizer = new Lazy<Role>(() => InitRole(this), isThreadSafe: true);
            this. _directoryInitalizer = new Lazy<Directory>(() => InitDirectory(this), isThreadSafe: true);
        }
        
        #region 公共属性

        private Guid _roleId = Guid.Empty;
        private Guid _directoryId = Guid.Empty;
        private Lazy<Role>  _roleInitalizer;
        private Role _role;
        private Lazy<Directory>  _directoryInitalizer;
        private Directory _directory;
        
        /// <summary>
        /// 角色Id
        /// </summary>
        [GuidRequired(ErrorMessage="角色不允许为空")]
		public Guid RoleId
		{
			get {return this._roleId;} 
            set {this.SetProperty("RoleId",ref this._roleId, value);}           	
		}
        
        /// <summary>
        /// 角色
        /// </summary>
        public Role Role
        {
            get
            {
                if (!this. _roleInitalizer.IsValueCreated)
                {
                    this._role = this. _roleInitalizer.Value;
                }
                return this._role;
            }
        }
        
        /// <summary>
        /// 目录Id
        /// </summary>
        [GuidRequired(ErrorMessage="目录不允许为空")]
		public Guid DirectoryId
		{
			get {return this._directoryId;} 
            set {this.SetProperty("DirectoryId",ref this._directoryId, value);}           	
		}
        
        /// <summary>
        /// 目录
        /// </summary>
        public Directory Directory
        {
            get
            {
                if (!this. _directoryInitalizer.IsValueCreated)
                {
                    this._directory = this. _directoryInitalizer.Value;
                }
                return this._directory;
            }
        }
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_RE_Directory_Role WHERE RoleId = @RoleId  AND DirectoryId = @DirectoryId  AND Status!=-1";
        const string QUERY_GETAllBYROLE = "SELECT * FROM Sys_RE_Directory_Role WHERE RoleId = @RoleId  AND Status!=-1";
        const string QUERY_GETAllBYDIRECTORY = "SELECT * FROM Sys_RE_Directory_Role WHERE DirectoryId = @DirectoryId  AND Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_RE_Directory_Role SET Status=-1 WHERE RoleId = @RoleId  AND DirectoryId = @DirectoryId AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_RE_Directory_Role WHERE RoleId = @RoleId  AND DirectoryId = @DirectoryId AND Status!=-1";        
        const string QUERY_INSERT="INSERT INTO Sys_RE_Directory_Role (Id, Status, CreatedDate, ChangedDate, Note, RoleId, DirectoryId) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @RoleId, @DirectoryId)";
        const string QUERY_UPDATE = "UPDATE Sys_RE_Directory_Role SET {0} WHERE  RoleId = @RoleId  AND DirectoryId = @DirectoryId";
                
        #endregion
        
        #region 静态方法
        
        public static Directory_Role Get(Guid roleId,Guid directoryId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Directory_Role>(QUERY_GETBYID, new { RoleId = roleId,DirectoryId = directoryId }).SingleOrDefault<Directory_Role>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = conn;
                item.MarkOld();
                item. _roleInitalizer = new Lazy<Role>(() => InitRole(item), isThreadSafe: true);
                item. _directoryInitalizer = new Lazy<Directory>(() => InitDirectory(item), isThreadSafe: true);
                return item;
            }
		}
        
        public static Directory_RoleList GetAllByRoleId(Guid roleId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory_Role>(QUERY_GETAllBYROLE, new { RoleId = roleId }).ToList();

                var list = new Directory_RoleList();
                foreach (var item in items)
                {
                    item.Connection = conn;
                    item.MarkOld();
                    item. _roleInitalizer = new Lazy<Role>(() => InitRole(item), isThreadSafe: true);
                    item. _directoryInitalizer = new Lazy<Directory>(() => InitDirectory(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }
                
        public static Directory_RoleList GetAllByDirectoryId(Guid directoryId)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory_Role>(QUERY_GETAllBYDIRECTORY, new { DirectoryId = directoryId }).ToList();

                var list = new Directory_RoleList();
                foreach (var item in items)
                {
                    item.Connection = conn;
                    item.MarkOld();
                    item. _roleInitalizer = new Lazy<Role>(() => InitRole(item), isThreadSafe: true);
                    item. _directoryInitalizer = new Lazy<Directory>(() => InitDirectory(item), isThreadSafe: true);
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
                return conn.Query<int>(QUERY_EXISTS, new {  RoleId = roleId, DirectoryId = directoryId }).Single() >= 1;
            }
        }
        
        #endregion
        
        public override int Delete(IDbConnection conn, IDbTransaction transaction)
        {
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, this, transaction, null, null);
        }

        public override int Update(IDbConnection conn, IDbTransaction transaction)
        {
            if (!this.IsDirty)
            {
                return this._changedRows;
            }
            this._updateParameters += ", ChangedDate = GetDate()";
            var query = String.Format(QUERY_UPDATE, this._updateParameters.TrimStart(','));
            this._changedRows += conn.Execute(query, this, transaction, null, null);
            return this._changedRows;
        }

        public override int Insert(IDbConnection conn, IDbTransaction transaction)
        {
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return this._changedRows;
        }
        
        #region 私有方法

        protected static Role InitRole(Directory_Role directory_role)
        {
            var item = Role.Get(directory_role.RoleId);
            if (item != null)
            {
                item.OnPropertyChange += item.MarkDirty;
            }
            return item;
        }

        protected static Directory InitDirectory(Directory_Role directory_role)
        {
            var item = Directory.Get(directory_role.DirectoryId);
            if (item != null)
            {
                item.OnPropertyChange += item.MarkDirty;
            }
            return item;
        }


        #endregion

    }
    
	[Serializable]
    public class Directory_RoleList:CollectionBase<Directory_RoleList,Directory_Role>
    {
        public Directory_RoleList() { this.Connection = SqlService.Instance.Connection;}

        protected const string tableName = "Sys_RE_Directory_Role";
        
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
               return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single()>0;
            }
        }
    }
}


