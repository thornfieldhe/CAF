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
	public partial class LoginLog :  BaseEntity<LoginLog>,IEntityBase
	{   
        public LoginLog()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_LoginLogs";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private string _userName = String.Empty;
        private string _ip = String.Empty;
        private string _note = String.Empty;
        
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage="用户名不允许为空")]
        [StringLength(20,ErrorMessage="用户名长度不能超过20")]
		public string UserName
		{
			get {return this._userName;} 
            set {this.SetProperty("UserName",ref this._userName, value);}           	
		}
        
        [Required(ErrorMessage="Ip不允许为空")]
        [StringLength(20,ErrorMessage="Ip长度不能超过20")]
		public string Ip
		{
			get {return this._ip;} 
            set {this.SetProperty("Ip",ref this._ip, value);}           	
		}
        
        [StringLength(500,ErrorMessage="Note长度不能超过500")]
		public string Note
		{
			get {return this._note;} 
            set {this.SetProperty("Note",ref this._note, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_LoginLogs Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_LoginLogs WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_LoginLogs WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_LoginLogs SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_LoginLogs WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_LoginLogs ([Id], [UserName], [Ip], [CreatedDate], [ChangedDate], [Status], [Note]) VALUES (@Id, @UserName, @Ip, @CreatedDate, @ChangedDate, @Status, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_LoginLogs SET {0} WHERE  Id = @Id  AND Version=@Version";
                
        #endregion
        		
        #region 静态方法
        
		public static LoginLog Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<LoginLog>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<LoginLog>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkClean();
                return item;
            }
		}
		 
		public static LoginLogList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<LoginLog>(QUERY_GETAll, null).ToList();                
                var list=new LoginLogList();
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
        public static LoginLogList Query(Expression<Func<IQueryable<LoginLog>, IQueryable<LoginLog>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<LoginLog>();
                expc.Add(exp);
                var items = conn.Query<LoginLog>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new LoginLogList();
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
        public static LoginLogList Query(Expression<Func<IQueryable<LoginLog>, IQueryable<LoginLog>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<LoginLog>();
            expc.Add(exp);
            var items = conn.Query<LoginLog>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new LoginLogList();
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
        public static int QueryCount(Expression<Func<IQueryable<LoginLog>, IQueryable<LoginLog>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<LoginLog>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<LoginLog>, IQueryable<LoginLog>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<LoginLog>();
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
		    this.AddDescription( "UserName:"+ this.UserName + "," );        
		    this.AddDescription( "Ip:"+ this.Ip + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "Version:"+ this.Version + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class LoginLogList:CollectionBase<LoginLogList,LoginLog>
    {
        public LoginLogList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_LoginLogs";
        }
    }
}


