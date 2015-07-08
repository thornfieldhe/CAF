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
	public partial class PostUserOrganize :  BaseEntity<PostUserOrganize>,IEntityBase
	{   
        public PostUserOrganize()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_PostUserOrganizes";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private Guid _postId = Guid.Empty;
        private Guid _userId = Guid.Empty;
        private Guid _organizeId = Guid.Empty;
        private string _note = String.Empty;
        
        /// <summary>
        /// 岗位Id
        /// </summary>
        [GuidRequired(ErrorMessage="岗位不允许为空")]
		public Guid PostId
		{
			get {return this._postId;} 
            set {this.SetProperty("PostId",ref this._postId, value);}           	
		}
        
        /// <summary>
        /// 岗位
        /// </summary>
        public Post Post
		{
			get
			{ 
				return Post.Get(this.PostId);
			}        	
		}

        
        /// <summary>
        /// 用户Id
        /// </summary>
        [GuidRequired(ErrorMessage="用户不允许为空")]
		public Guid UserId
		{
			get {return this._userId;} 
            set {this.SetProperty("UserId",ref this._userId, value);}           	
		}
        
        /// <summary>
        /// 用户
        /// </summary>
        public User User
		{
			get
			{ 
				return User.Get(this.UserId);
			}        	
		}

        
        /// <summary>
        /// 部门Id
        /// </summary>
        [GuidRequired(ErrorMessage="部门不允许为空")]
		public Guid OrganizeId
		{
			get {return this._organizeId;} 
            set {this.SetProperty("OrganizeId",ref this._organizeId, value);}           	
		}
        
        /// <summary>
        /// 部门
        /// </summary>
        public Organize Organize
		{
			get
			{ 
				return Organize.Get(this.OrganizeId);
			}        	
		}

        
        /// <summary>
        /// 备注
        /// </summary>
		public string Note
		{
			get {return this._note;} 
            set {this.SetProperty("Note",ref this._note, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_PostUserOrganizes Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_PostUserOrganizes WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_PostUserOrganizes WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_PostUserOrganizes SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_PostUserOrganizes WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_PostUserOrganizes ([Id], [PostId], [UserId], [OrganizeId], [Status], [CreatedDate], [ChangedDate], [Note]) VALUES (@Id, @PostId, @UserId, @OrganizeId, @Status, @CreatedDate, @ChangedDate, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_PostUserOrganizes SET {0} WHERE  Id = @Id  AND Version=@Version";
                
        #endregion
        		
        #region 静态方法
        
		public static PostUserOrganize Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<PostUserOrganize>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<PostUserOrganize>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkClean();
                return item;
            }
		}
		 
		public static PostUserOrganizeList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<PostUserOrganize>(QUERY_GETAll, null).ToList();                
                var list=new PostUserOrganizeList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkClean();
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
        public static PostUserOrganizeList Query(Expression<Func<IQueryable<PostUserOrganize>, IQueryable<PostUserOrganize>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<PostUserOrganize>();
                expc.Add(exp);
                var items = conn.Query<PostUserOrganize>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new PostUserOrganizeList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkClean();
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
        public static PostUserOrganizeList Query(Expression<Func<IQueryable<PostUserOrganize>, IQueryable<PostUserOrganize>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<PostUserOrganize>();
            expc.Add(exp);
            var items = conn.Query<PostUserOrganize>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new PostUserOrganizeList();
            foreach (var item in items)
            {
                item.Connection = SqlService.Instance.Connection;
                item.MarkClean();
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
        public static int QueryCount(Expression<Func<IQueryable<PostUserOrganize>, IQueryable<PostUserOrganize>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<PostUserOrganize>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<PostUserOrganize>, IQueryable<PostUserOrganize>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<PostUserOrganize>();
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
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return this._changedRows;
		}
		
		#region 私有方法
		
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
		    this.AddDescription( "Id:"+ this.Id + "," );        
		    this.AddDescription( "PostId:"+ this.PostId + "," );        
		    this.AddDescription( "UserId:"+ this.UserId + "," );        
		    this.AddDescription( "OrganizeId:"+ this.OrganizeId + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "Version:"+ this.Version + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class PostUserOrganizeList:CollectionBase<PostUserOrganizeList,PostUserOrganize>
    {
        public PostUserOrganizeList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_PostUserOrganizes";
        }
    }
}


