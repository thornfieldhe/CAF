using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq.Expressions;

    [Serializable]
	public partial class Organize : BaseBusiness<Organize>,IEntityBase
	{   
        public Organize()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Organizes";
            base.MarkNew();
            this. _userListInitalizer = new Lazy<UserList>(() => InitUsers(this), isThreadSafe: true);          
    		this. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(this), isThreadSafe: true);
            this.Users= new UserList();        
            this.Roles= new RoleList();        
		}
		
            
		#region 公共属性

        private string _note = String.Empty;
        private string _name = String.Empty;
        private Guid? _parentId = Guid.Empty;
        private int _sort;
        private string _level = String.Empty;
        private string _code = String.Empty;
        private UserList  _userList;
        private Lazy<UserList>  _userListInitalizer;       
        private RoleList  _roleList;
        private Lazy<RoleList>  _roleListInitalizer;       
        
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500,ErrorMessage="备注长度不能超过500")]
		public string Note
		{
			get {return this._note;} 
            set {this.SetProperty("Note",ref this._note, value);}           	
		}
        
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(ErrorMessage="部门名称不允许为空")]
        [StringLength(50,ErrorMessage="部门名称长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// 父部门Id
        /// </summary>
		public Guid? ParentId
		{
			get {return this._parentId;} 
            set {this.SetProperty("ParentId",ref this._parentId, value);}           	
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
        [Required(ErrorMessage="排序不允许为空")]
		public int Sort
		{
			get {return this._sort;} 
            set {this.SetProperty("Sort",ref this._sort, value);}           	
		}
        
        /// <summary>
        /// 部门层级
        /// </summary>
        [Required(ErrorMessage="部门层级不允许为空")]
        [StringLength(20,ErrorMessage="部门层级长度不能超过20")]
		public string Level
		{
			get {return this._level;} 
            set {this.SetProperty("Level",ref this._level, value);}           	
		}
        
        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage="编码不允许为空")]
        [StringLength(50,ErrorMessage="编码长度不能超过50")]
		public string Code
		{
			get {return this._code;} 
            set {this.SetProperty("Code",ref this._code, value);}           	
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
        
        public RoleList Roles
        {
            get
            {
                if (!this. _roleListInitalizer.IsValueCreated)
                {
                    this. _roleList = this. _roleListInitalizer.Value;
                }
                return this. _roleList;
            }
             set
            {
                this. _roleList = value;
            }
        }
        public override void Validate()
        {
            foreach (var item in this.Users)
            {
                item.Validate();
            }
			this. _roleListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
                foreach (var item in this.Roles)
                {
                    item.Validate();
                }
            });
           base.Validate();
        }
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_Organizes Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Organizes WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Organizes WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Organizes SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Organizes WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYPARENTID = "SELECT * FROM Sys_Organizes WHERE  Status!=-1 And ParentId=@ParentId";
        const string QUERY_GETALLBYROLEID = "SELECT t1.* FROM Sys_Organizes t1 INNER JOIN Sys_R_Organize_Role t2 on t1.Id=t2.OrganizeId  where t2.RoleId=@RoleId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSORGANIZEROLE = "SELECT COUNT(*) FROM Sys_R_Organize_Role WHERE  OrganizeId = @OrganizeId AND RoleId=@RoleId";
        const string QUERY_ADDRELARIONSHIPWITHORGANIZEROLE = "INSERT INTO Sys_R_Organize_Role (OrganizeId,RoleId,Status)VALUES(@OrganizeId, @RoleId,0)";
        const string QUERY_DELETERELARIONSHIPWITHORGANIZEROLE = "UPDATE Sys_R_Organize_Role SET Status=-1 WHERE OrganizeId=@OrganizeId AND RoleId=@RoleId AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_Organizes ([Id], [Status], [CreatedDate], [ChangedDate], [Note], [Name], [ParentId], [Sort], [Level], [Code]) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @Name, @ParentId, @Sort, @Level, @Code)";
        const string QUERY_UPDATE = "UPDATE Sys_Organizes SET {0} WHERE  Id = @Id  AND Version=@Version";
                
        #endregion
        		
        #region 静态方法
        
		public static Organize Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Organize>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Organize>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkClean();
                item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                return item;
            }
		}
		 
		public static OrganizeList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<Organize>(QUERY_GETAll, null).ToList();                
                var list=new OrganizeList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkClean();
                    item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                     item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkClean();
                return list;
            }
		}        
		
       public static OrganizeList GetAllByParentId(Guid parentId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<Organize>(QUERY_GETALLBYPARENTID, new { ParentId = parentId }).ToList();
              	var list=new OrganizeList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkClean();
					item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);                 
                     item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    list.Add(item);
                }
				list.MarkClean();
                return list;
            }
		}
		
       public static OrganizeList GetAllByRoleId(Guid roleId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<Organize>(QUERY_GETALLBYROLEID, new { RoleId = roleId }).ToList();
                
                var list=new OrganizeList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkClean();
                    item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);                 
                    item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    list.Add(item);
                }
				list.MarkClean();
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
        public static OrganizeList Query(Expression<Func<IQueryable<Organize>, IQueryable<Organize>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<Organize>();
                expc.Add(exp);
                var items = conn.Query<Organize>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new OrganizeList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkClean();
                    item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                     item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    list.Add(item);
                }
				list.MarkClean();
                return list;
            }
        }
        
                /// <summary>
        /// 表达式查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static OrganizeList Query(Expression<Func<IQueryable<Organize>, IQueryable<Organize>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<Organize>();
            expc.Add(exp);
            var items = conn.Query<Organize>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new OrganizeList();
            foreach (var item in items)
            {
                item.Connection = SqlService.Instance.Connection;
                item.MarkClean();
                item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                 item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                list.Add(item);
            }
			list.MarkClean();
            return list;
        }

        /// <summary>
        /// 数量查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static int QueryCount(Expression<Func<IQueryable<Organize>, IQueryable<Organize>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<Organize>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<Organize>, IQueryable<Organize>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<Organize>();
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
    		this. _userListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.Users.SaveChanges(conn,transaction);
            });
			this. _roleListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Roles.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
    		this. _userListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.Users.SaveChanges(conn,transaction);
            });
			this. _roleListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Roles.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		#region 私有方法
		
		protected  int RelationshipWithRole(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var role in this.Roles.Members)
            {
                if (role.IsDelete && this.Roles.IsChangeRelationship)
                {
                    this._changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHORGANIZEROLE, new { OrganizeId = this.Id,  RoleId = role.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSORGANIZEROLE , new { OrganizeId = this.Id, RoleId = role.Id },transaction).Single() >= 1;
                    if (!isExist)
                    {
                        this._changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHORGANIZEROLE, new { OrganizeId = this.Id, RoleId = role.Id }, transaction, null, null);
                    }
                }
            }
            return this._changedRows;
        }

        protected static RoleList InitRoles(Organize organize)
        {
            var list = Role.GetAllByOrganizeId(organize.Id);
            list.OnSaved += organize.RelationshipWithRole;
            list.OnMarkDirty += organize.MarkDirty;
            list.IsChangeRelationship = true;
			return list;
        }
		
		protected static UserList InitUsers(Organize organize)
        {
            var list = User.GetAllByOrganizeId(organize.Id);
            list.OnMarkDirty += organize.MarkDirty;
			list.OnInsert += organize.PostAddUser;
            return list;
        }
		
		protected  void PostAddUser(User user)
        {
			user.OrganizeId=this.Id;
        }
		
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
		    this.AddDescription( "Id:"+ this.Id + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "Name:"+ this.Name + "," );        
		    this.AddDescription( "ParentId:"+ this.ParentId + "," );        
		    this.AddDescription( "Sort:"+ this.Sort + "," );        
		    this.AddDescription( "Level:"+ this.Level + "," );        
		    this.AddDescription( "Code:"+ this.Code + "," );        
		    this.AddDescription( "Version:"+ this.Version + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class OrganizeList:CollectionBase<OrganizeList,Organize>
    {
        public OrganizeList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Organizes";
        }
    }
}


