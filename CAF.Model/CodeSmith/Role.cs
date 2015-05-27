using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using CAF.Validation;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
        using System.Linq.Expressions;

    [Serializable]
	public partial class Role :  BaseEntity<Role>
	{   
        public Role()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Roles";
            base.MarkNew();
    		this. _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(this), isThreadSafe: true);
    		this. _userListInitalizer = new Lazy<UserList>(() => InitUsers(this), isThreadSafe: true);
            this.Organizes= new OrganizeList();        
            this.Users= new UserList();        
		}
		
            
		#region 公共属性

        private string _name = String.Empty;
        private OrganizeList  _organizeList;
        private Lazy<OrganizeList>  _organizeListInitalizer;       
        private UserList  _userList;
        private Lazy<UserList>  _userListInitalizer;       
        
        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(20,ErrorMessage="角色名称长度不能超过20")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        public OrganizeList Organizes
        {
            get
            {
                if (!this. _organizeListInitalizer.IsValueCreated)
                {
                    this. _organizeList = this. _organizeListInitalizer.Value;
                }
                return this. _organizeList;
            }
             set
            {
                this. _organizeList = value;
            }
        }
        public UserList Users
        {
            get
            {
                if (!this. _userListInitalizer.IsValueCreated)
                {
                    this. _userList = this. _userListInitalizer.Value;
                }
                return this. _userList;
            }
             set
            {
                this. _userList = value;
            }
        }
        public override bool IsValid
        {
            get
            {
			    this.Errors=new List<string>();
                var isValid = true;
                var baseValid = base.IsValid;
				this. _organizeListInitalizer.IsValueCreated.IfTrue(
                () =>
                {
                    foreach (var item in this.Organizes.Where(item => !item.IsValid))
                    {
                        this.Errors.AddRange(item.Errors);
                        isValid = false;
                    }
                });
				this. _userListInitalizer.IsValueCreated.IfTrue(
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
            protected set { this._isValid = value; }
        }
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_Roles Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Roles WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Roles WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Roles SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Roles WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYORGANIZEID = "SELECT t1.* FROM Sys_Roles t1 INNER JOIN Sys_R_Organize_Role t2 on t1.Id=t2.RoleId  where t2.OrganizeId=@OrganizeId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSORGANIZEROLE = "SELECT COUNT(*) FROM Sys_R_Organize_Role WHERE  RoleId = @RoleId AND OrganizeId=@OrganizeId";
        const string QUERY_ADDRELARIONSHIPWITHORGANIZEROLE = "INSERT INTO Sys_R_Organize_Role (RoleId,OrganizeId,Status)VALUES(@RoleId, @OrganizeId,0)";
        const string QUERY_DELETERELARIONSHIPWITHORGANIZEROLE = "UPDATE Sys_R_Organize_Role SET Status=-1 WHERE RoleId=@RoleId AND OrganizeId=@OrganizeId AND Status!=-1";
        const string QUERY_GETALLBYUSERID = "SELECT t1.* FROM Sys_Roles t1 INNER JOIN Sys_R_User_Role t2 on t1.Id=t2.RoleId  where t2.UserId=@UserId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSUSERROLE = "SELECT COUNT(*) FROM Sys_R_User_Role WHERE  RoleId = @RoleId AND UserId=@UserId";
        const string QUERY_ADDRELARIONSHIPWITHUSERROLE = "INSERT INTO Sys_R_User_Role (RoleId,UserId,Status)VALUES(@RoleId, @UserId,0)";
        const string QUERY_DELETERELARIONSHIPWITHUSERROLE = "UPDATE Sys_R_User_Role SET Status=-1 WHERE RoleId=@RoleId AND UserId=@UserId AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_Roles ([Id], [Status], [CreatedDate], [ChangedDate], [Note], [Name]) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @Name)";
        const string QUERY_UPDATE = "UPDATE Sys_Roles SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static Role Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Role>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Role>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                item. _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                return item;
            }
		}
		 
		public static RoleList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<Role>(QUERY_GETAll, null).ToList();                
                var list=new RoleList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                     item. _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                     item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
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
                
                var list=new RoleList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                    item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
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
                
                var list=new RoleList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                    item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
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
        
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        public static bool Exists(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                return conn.Query<int>(QUERY_EXISTS, new { Id = id }).Single() >= 1;
            }
        }      
        
        /// <summary>
        /// 表达式查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static RoleList Query(Expression<Func<IQueryable<Role>, IQueryable<Role>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<Role>();
                expc.Add(exp);
                var items = conn.Query<Role>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new RoleList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                     item. _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                     item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    list.Add(item);
                }
				list.MarkOld();
                return list;
            }
        }
        
                /// <summary>
        /// 表达式查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static RoleList Query(Expression<Func<IQueryable<Role>, IQueryable<Role>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<Role>();
            expc.Add(exp);
            var items = conn.Query<Role>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new RoleList();
            foreach (var item in items)
            {
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                 item. _organizeListInitalizer = new Lazy<OrganizeList>(() => InitOrganizes(item), isThreadSafe: true);
                 item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                list.Add(item);
            }
			list.MarkOld();
            return list;
        }

        /// <summary>
        /// 数量查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static int QueryCount(Expression<Func<IQueryable<Role>, IQueryable<Role>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<Role>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<Role>, IQueryable<Role>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<Role>();
                expc.Add(exp);
               return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single()>0;
            }
        }
        
        #endregion
        
		
		public override int Delete(IDbConnection conn, IDbTransaction transaction)
		{
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, new { Id = this.Id }, transaction, null, null);
		}
		
		public override int Update(IDbConnection conn, IDbTransaction transaction)
		{
             if (!this.IsDirty)
             {
                return this._changedRows;
             }  
            this._updateParameters+=", ChangedDate = GetDate()";
			var query = String.Format(QUERY_UPDATE, this._updateParameters.TrimStart(','));
			this._changedRows+= conn.Execute(query, this, transaction, null, null);
			this. _organizeListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Organizes.SaveChanges(conn,transaction);
            });
			this. _userListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Users.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
			this. _organizeListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Organizes.SaveChanges(conn,transaction);
            });
			this. _userListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Users.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		#region 私有方法
		
		protected  int RelationshipWithOrganize(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var organize in this.Organizes.Members)
            {
                if (organize.IsDelete && this.Organizes.IsChangeRelationship)
                {
                    this._changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHORGANIZEROLE, new { RoleId = this.Id,  OrganizeId = organize.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSORGANIZEROLE , new { RoleId = this.Id, OrganizeId = organize.Id },transaction).Single() >= 1;
                    if (!isExist)
                    {
                        this._changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHORGANIZEROLE, new { RoleId = this.Id, OrganizeId = organize.Id }, transaction, null, null);
                    }
                }
            }
            return this._changedRows;
        }

        protected static OrganizeList InitOrganizes(Role role)
        {
            var list = Organize.GetAllByRoleId(role.Id);
            list.OnSaved += role.RelationshipWithOrganize;
            list.OnMarkDirty += role.MarkDirty;
            list.IsChangeRelationship = true;
			return list;
        }
		
		protected  int RelationshipWithUser(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var user in this.Users.Members)
            {
                if (user.IsDelete && this.Users.IsChangeRelationship)
                {
                    this._changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHUSERROLE, new { RoleId = this.Id,  UserId = user.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSUSERROLE , new { RoleId = this.Id, UserId = user.Id },transaction).Single() >= 1;
                    if (!isExist)
                    {
                        this._changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHUSERROLE, new { RoleId = this.Id, UserId = user.Id }, transaction, null, null);
                    }
                }
            }
            return this._changedRows;
        }

        protected static UserList InitUsers(Role role)
        {
            var list = User.GetAllByRoleId(role.Id);
            list.OnSaved += role.RelationshipWithUser;
            list.OnMarkDirty += role.MarkDirty;
            list.IsChangeRelationship = true;
			return list;
        }
		
		#endregion
				
	}
    
	[Serializable]
    public class RoleList:CollectionBase<RoleList,Role>
    {
        public RoleList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Roles";
        }
    }
}


