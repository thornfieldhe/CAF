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
	public partial class Post :  BaseEntity<Post>
	{   
        public Post()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Posts";
            base.MarkNew();
    		this. _userListInitalizer = new Lazy<UserList>(() => InitUsers(this), isThreadSafe: true);
            this.Users= new UserList();        
		}
		
            
		#region 公共属性

        private string _name = String.Empty;
        private UserList  _userList;
        private Lazy<UserList>  _userListInitalizer;       
        
        /// <summary>
        /// 岗位名称
        /// </summary>
        [Required(ErrorMessage="岗位名称不允许为空")]
        [StringLength(50,ErrorMessage="岗位名称长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
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
				this. _userListInitalizer.IsValueCreated.IfIsTrue(
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
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Posts WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Posts WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Posts SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Posts WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYUSERID = "SELECT t1.* FROM Sys_Posts t1 INNER JOIN Sys_R_User_Post t2 on t1.Id=t2.PostId  where t2.UserId=@UserId AND t1.Status!=-1 AND t2.Status!=-1";
        const string QUERY_CONTAINSUSERPOST = "SELECT COUNT(*) FROM Sys_R_User_Post WHERE  PostId = @PostId AND UserId=@UserId";
        const string QUERY_ADDRELARIONSHIPWITHUSERPOST = "INSERT INTO Sys_R_User_Post (PostId,UserId,Status)VALUES(@PostId, @UserId,0)";
        const string QUERY_DELETERELARIONSHIPWITHUSERPOST = "UPDATE Sys_R_User_Post SET Status=-1 WHERE PostId=@PostId AND UserId=@UserId AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_Posts (Id, Name, CreatedDate, ChangedDate, Status, Note) VALUES (@Id, @Name, @CreatedDate, @ChangedDate, @Status, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_Posts SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static Post Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Post>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Post>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                return item;
            }
		}
		 
		public static PostList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<Post>(QUERY_GETAll, null).ToList();                
                var list=new PostList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                     item. _userListInitalizer = new Lazy<UserList>(() => InitUsers(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
		}        
		
       public static PostList GetAllByUserId(Guid userId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<Post>(QUERY_GETALLBYUSERID, new { UserId = userId }).ToList();
                
                var list=new PostList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
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
        
        public static bool Exists(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                return conn.Query<int>(QUERY_EXISTS, new { Id = id }).Single() >= 1;
            }
        }      
        
        #endregion
        
		
		public override int Delete(IDbConnection conn, IDbTransaction transaction)
		{
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, new { Id = Id }, transaction, null, null);
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
			this. _userListInitalizer.IsValueCreated.IfIsTrue(
            () =>
            {
 				this._changedRows+=this.Users.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
			this. _userListInitalizer.IsValueCreated.IfIsTrue(
            () =>
            {
 				this._changedRows+=this.Users.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		#region 私有方法
		
		protected  int RelationshipWithUser(IDbConnection conn, IDbTransaction transaction)
        {
            foreach (var user in this.Users.Members)
            {
                if (user.IsDelete && this.Users.IsChangeRelationship)
                {
                    this._changedRows += conn.Execute(QUERY_DELETERELARIONSHIPWITHUSERPOST, new { PostId = this.Id,  UserId = user.Id }, transaction, null, null);
                }
                else
                {
                    var isExist = conn.Query<int>(QUERY_CONTAINSUSERPOST , new { PostId = this.Id, UserId = user.Id },transaction).Single() >= 1;
                    if (!isExist)
                    {
                        this._changedRows += conn.Execute(QUERY_ADDRELARIONSHIPWITHUSERPOST, new { PostId = this.Id, UserId = user.Id }, transaction, null, null);
                    }
                }
            }
            return this._changedRows;
        }

        protected static UserList InitUsers(Post post)
        {
            var list = User.GetAllByPostId(post.Id);
            list.OnSaved += post.RelationshipWithUser;
            list.OnMarkDirty += post.MarkDirty;
            list.IsChangeRelationship = true;
			return list;
        }
		
		#endregion
				
	}
    
	[Serializable]
    public class PostList:CollectionBase<PostList,Post>
    {
        public PostList() {this.Connection = SqlService.Instance.Connection; }

        protected const string tableName = "Sys_Posts";
        
        public static PostList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Post>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new PostList();
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


