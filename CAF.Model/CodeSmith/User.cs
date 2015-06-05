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
	public partial class User :  BaseEntity<User>
	{   
        public User()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Users";
            base.MarkNew();
    		this. _userSettingInitalizer = new Lazy<UserSetting>(() => UserSetting.GetByUserId(Id), isThreadSafe: true);
    		this. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(this), isThreadSafe: true);
    		this. _postListInitalizer = new Lazy<PostList>(() => InitPosts(this), isThreadSafe: true);
            this.Roles= new RoleList();        
            this.Posts= new PostList();        
		}
		
            
		#region 公共属性

        private string _loginName = String.Empty;
        private string _abb = String.Empty;
        private string _name = String.Empty;
        private string _pass = String.Empty;
        private string _phoneNum = String.Empty;
        private Guid _organizeId = Guid.Empty;
        private string _email = String.Empty;
        private UserSetting  _userSetting;
        private Lazy<UserSetting>  _userSettingInitalizer;       
        private RoleList  _roleList;
        private Lazy<RoleList>  _roleListInitalizer;       
        private PostList  _postList;
        private Lazy<PostList>  _postListInitalizer;       
        
        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage="登录名不允许为空")]
        [StringLength(20,ErrorMessage="登录名长度不能超过20")]
		public string LoginName
		{
			get {return this._loginName;} 
            set {this.SetProperty("LoginName",ref this._loginName, value);}           	
		}
        
        /// <summary>
        /// 用户简称
        /// </summary>
        [Required(ErrorMessage="用户简称不允许为空")]
        [StringLength(20,ErrorMessage="用户简称长度不能超过20")]
		public string Abb
		{
			get {return this._abb;} 
            set {this.SetProperty("Abb",ref this._abb, value);}           	
		}
        
        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage="用户姓名不允许为空")]
        [StringLength(20,ErrorMessage="用户姓名长度不能超过20")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage="用户密码不允许为空")]
        [StringLength(50,ErrorMessage="用户密码长度不能超过50")]
		public string Pass
		{
			get {return this._pass;} 
            set {this.SetProperty("Pass",ref this._pass, value);}           	
		}
        
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(30,ErrorMessage="电话长度不能超过30")]
		public string PhoneNum
		{
			get {return this._phoneNum;} 
            set {this.SetProperty("PhoneNum",ref this._phoneNum, value);}           	
		}
        
        /// <summary>
        /// 组织架构Id
        /// </summary>
        [GuidRequired(ErrorMessage="组织架构不允许为空")]
		public Guid OrganizeId
		{
			get {return this._organizeId;} 
            set {this.SetProperty("OrganizeId",ref this._organizeId, value);}           	
		}
        
        /// <summary>
        /// 组织架构
        /// </summary>
        public Organize Organize
		{
			get
			{ 
				return Organize.Get(this.OrganizeId);
			}        	
		}

        
        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage="电子邮件不允许为空")]
        [StringLength(50,ErrorMessage="电子邮件长度不能超过50")]
		public string Email
		{
			get {return this._email;} 
            set {this.SetProperty("Email",ref this._email, value);}           	
		}
        
        public UserSetting UserSetting
        {
            get
            {
                if (!this. _userSettingInitalizer.IsValueCreated)
                {
                    this. _userSetting = this. _userSettingInitalizer.Value;
                }
                return this. _userSetting;
            }
            set
            {
                if (!this. _userSettingInitalizer.IsValueCreated)
                {
                    this. _userSetting = this. _userSettingInitalizer.Value;
                }
                this. _userSetting = value;
				if (this. _userSetting == null)
                {
                    return;
                }
                this. _userSetting.OnPropertyChange += this.MarkDirty;
                this. _userSetting.UserId = this.Id;
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
        public PostList Posts
        {
            get
            {
                if (!this. _postListInitalizer.IsValueCreated)
                {
                    this. _postList = this. _postListInitalizer.Value;
                }
                return this. _postList;
            }
             set
            {
                this. _postList = value;
            }
        }
        public override void Validate()
        {
            if (this. _userSettingInitalizer.IsValueCreated && this.UserSetting!=null)
            {
                this.UserSetting.Validate();
            }
			this. _roleListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
                foreach (var item in this.Roles)
                {
                    item.Validate();
                }
            });
			this. _postListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
                foreach (var item in this.Posts)
                {
                    item.Validate();
                }
            });
           base.Validate();
        }
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_Users Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Users WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Users WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Users SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Users WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYORGANIZEID = "SELECT * FROM Sys_Users WHERE  Status!=-1 And OrganizeId=@OrganizeId";
        const string QUERY_GETALLBYROLEID = "SELECT t1.* FROM Sys_Users t1 INNER JOIN Sys_R_User_Role t2 on t1.Id=t2.UserId  where t2.RoleId=@RoleId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSUSERROLE = "SELECT COUNT(*) FROM Sys_R_User_Role WHERE  UserId = @UserId AND RoleId=@RoleId";
        const string QUERY_ADDRELARIONSHIPWITHUSERROLE = "INSERT INTO Sys_R_User_Role (UserId,RoleId,Status)VALUES(@UserId, @RoleId,0)";
        const string QUERY_DELETERELARIONSHIPWITHUSERROLE = "UPDATE Sys_R_User_Role SET Status=-1 WHERE UserId=@UserId AND RoleId=@RoleId AND Status!=-1";
        const string QUERY_GETALLBYPOSTID = "SELECT t1.* FROM Sys_Users t1 INNER JOIN Sys_R_User_Post t2 on t1.Id=t2.UserId  where t2.PostId=@PostId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSUSERPOST = "SELECT COUNT(*) FROM Sys_R_User_Post WHERE  UserId = @UserId AND PostId=@PostId";
        const string QUERY_ADDRELARIONSHIPWITHUSERPOST = "INSERT INTO Sys_R_User_Post (UserId,PostId,Status)VALUES(@UserId, @PostId,0)";
        const string QUERY_DELETERELARIONSHIPWITHUSERPOST = "UPDATE Sys_R_User_Post SET Status=-1 WHERE UserId=@UserId AND PostId=@PostId AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_Users ([Id], [Status], [CreatedDate], [ChangedDate], [Note], [LoginName], [Abb], [Name], [Pass], [PhoneNum], [OrganizeId], [Email]) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @LoginName, @Abb, @Name, @Pass, @PhoneNum, @OrganizeId, @Email)";
        const string QUERY_UPDATE = "UPDATE Sys_Users SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static User Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<User>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<User>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
				item. _userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);                   
                item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                item. _postListInitalizer = new Lazy<PostList>(() => InitPosts(item), isThreadSafe: true);
                return item;
            }
		}
		 
		public static UserList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<User>(QUERY_GETAll, null).ToList();                
                var list=new UserList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);      
                     item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                     item. _postListInitalizer = new Lazy<PostList>(() => InitPosts(item), isThreadSafe: true);
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
              	var list=new UserList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
            		item. _userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);      
                     item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                     item. _postListInitalizer = new Lazy<PostList>(() => InitPosts(item), isThreadSafe: true);
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
                
                var list=new UserList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
            		item. _userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);      
                    item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    item. _postListInitalizer = new Lazy<PostList>(() => InitPosts(item), isThreadSafe: true);
                    list.Add(item);
                }
				list.MarkOld();
                return list;
            }
		}
		
       public static UserList GetAllByPostId(Guid postId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<User>(QUERY_GETALLBYPOSTID, new { PostId = postId }).ToList();
                
                var list=new UserList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
            		item. _userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);      
                    item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                    item. _postListInitalizer = new Lazy<PostList>(() => InitPosts(item), isThreadSafe: true);
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
        public static UserList Query(Expression<Func<IQueryable<User>, IQueryable<User>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<User>();
                expc.Add(exp);
                var items = conn.Query<User>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new UserList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);      
                     item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                     item. _postListInitalizer = new Lazy<PostList>(() => InitPosts(item), isThreadSafe: true);
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
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static UserList Query(Expression<Func<IQueryable<User>, IQueryable<User>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<User>();
            expc.Add(exp);
            var items = conn.Query<User>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new UserList();
            foreach (var item in items)
            {
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                item. _userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);      
                 item. _roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                 item. _postListInitalizer = new Lazy<PostList>(() => InitPosts(item), isThreadSafe: true);
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
        public static int QueryCount(Expression<Func<IQueryable<User>, IQueryable<User>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<User>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<User>, IQueryable<User>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<User>();
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
			if(this. _userSettingInitalizer.IsValueCreated && this.UserSetting!=null) 
            {
 				this._changedRows += this.UserSetting.SaveChange(conn, transaction);
            }
			this. _roleListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Roles.SaveChanges(conn,transaction);
            });
			this. _postListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Posts.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
			if(this. _userSettingInitalizer.IsValueCreated && this.UserSetting!=null) 
            {
 				this._changedRows += this.UserSetting.SaveChange(conn, transaction);
            }
			this. _roleListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Roles.SaveChanges(conn,transaction);
            });
			this. _postListInitalizer.IsValueCreated.IfTrue(
            () =>
            {
 				this._changedRows+=this.Posts.SaveChanges(conn,transaction);
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
                    this._changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHUSERROLE, new { UserId = this.Id,  RoleId = role.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSUSERROLE , new { UserId = this.Id, RoleId = role.Id },transaction).Single() >= 1;
                    if (!isExist)
                    {
                        this._changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHUSERROLE, new { UserId = this.Id, RoleId = role.Id }, transaction, null, null);
                    }
                }
            }
            return this._changedRows;
        }

        protected static RoleList InitRoles(User user)
        {
            var list = Role.GetAllByUserId(user.Id);
            list.OnSaved += user.RelationshipWithRole;
            list.OnMarkDirty += user.MarkDirty;
            list.IsChangeRelationship = true;
			return list;
        }
		
		protected  int RelationshipWithPost(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var post in this.Posts.Members)
            {
                if (post.IsDelete && this.Posts.IsChangeRelationship)
                {
                    this._changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHUSERPOST, new { UserId = this.Id,  PostId = post.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSUSERPOST , new { UserId = this.Id, PostId = post.Id },transaction).Single() >= 1;
                    if (!isExist)
                    {
                        this._changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHUSERPOST, new { UserId = this.Id, PostId = post.Id }, transaction, null, null);
                    }
                }
            }
            return this._changedRows;
        }

        protected static PostList InitPosts(User user)
        {
            var list = Post.GetAllByUserId(user.Id);
            list.OnSaved += user.RelationshipWithPost;
            list.OnMarkDirty += user.MarkDirty;
            list.IsChangeRelationship = true;
			return list;
        }
		
        protected static UserSetting InitUserSetting(User user)
        {
            var item = UserSetting.GetByUserId(user.Id);
            if (item == null)
            {
                return null;
            }
            item.UserId = user.Id;
			item.OnPropertyChange += user.MarkDirty;
            return item;
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
		    this.AddDescription( "LoginName:"+ this.LoginName + "," );        
		    this.AddDescription( "Abb:"+ this.Abb + "," );        
		    this.AddDescription( "Name:"+ this.Name + "," );        
		    this.AddDescription( "Pass:"+ this.Pass + "," );        
		    this.AddDescription( "PhoneNum:"+ this.PhoneNum + "," );        
		    this.AddDescription( "OrganizeId:"+ this.OrganizeId + "," );        
		    this.AddDescription( "Email:"+ this.Email + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class UserList:CollectionBase<UserList,User>
    {
        public UserList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Users";
        }
    }
}


