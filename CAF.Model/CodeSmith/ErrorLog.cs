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
	public partial class ErrorLog :  BaseEntity<ErrorLog>
	{   
        public ErrorLog()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_ErrorLogs";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private string _userName = String.Empty;
        private int _pageCode;
        private string _page = String.Empty;
        private string _ip = String.Empty;
        private string _message = String.Empty;
        private string _details = String.Empty;
        
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
        
        /// <summary>
        /// 错误代码
        /// </summary>
        [Required(ErrorMessage="错误代码不允许为空")]
		public int PageCode
		{
			get {return this._pageCode;} 
            set {this.SetProperty("PageCode",ref this._pageCode, value);}           	
		}
        
        /// <summary>
        /// 错误页
        /// </summary>
        [StringLength(50,ErrorMessage="错误页长度不能超过50")]
		public string Page
		{
			get {return this._page;} 
            set {this.SetProperty("Page",ref this._page, value);}           	
		}
        
        [Required(ErrorMessage="Ip不允许为空")]
        [StringLength(20,ErrorMessage="Ip长度不能超过20")]
		public string Ip
		{
			get {return this._ip;} 
            set {this.SetProperty("Ip",ref this._ip, value);}           	
		}
        
        /// <summary>
        /// 错误
        /// </summary>
        [Required(ErrorMessage="错误不允许为空")]
        [StringLength(200,ErrorMessage="错误长度不能超过200")]
		public string Message
		{
			get {return this._message;} 
            set {this.SetProperty("Message",ref this._message, value);}           	
		}
        
        /// <summary>
        /// 详细错误
        /// </summary>
        [Required(ErrorMessage="详细错误不允许为空")]
		public string Details
		{
			get {return this._details;} 
            set {this.SetProperty("Details",ref this._details, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_ErrorLogs Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_ErrorLogs WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_ErrorLogs WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_ErrorLogs SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_ErrorLogs WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_ErrorLogs ([Id], [UserName], [PageCode], [Page], [Ip], [Message], [Details], [CreatedDate], [ChangedDate], [Status], [Note]) VALUES (@Id, @UserName, @PageCode, @Page, @Ip, @Message, @Details, @CreatedDate, @ChangedDate, @Status, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_ErrorLogs SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static ErrorLog Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<ErrorLog>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<ErrorLog>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static ErrorLogList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<ErrorLog>(QUERY_GETAll, null).ToList();                
                var list=new ErrorLogList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
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
        public static ErrorLogList Query(Expression<Func<IQueryable<ErrorLog>, IQueryable<ErrorLog>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<ErrorLog>();
                expc.Add(exp);
                var items = conn.Query<ErrorLog>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new ErrorLogList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
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
        public static ErrorLogList Query(Expression<Func<IQueryable<ErrorLog>, IQueryable<ErrorLog>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<ErrorLog>();
            expc.Add(exp);
            var items = conn.Query<ErrorLog>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new ErrorLogList();
            foreach (var item in items)
            {
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
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
        public static int QueryCount(Expression<Func<IQueryable<ErrorLog>, IQueryable<ErrorLog>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<ErrorLog>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<ErrorLog>, IQueryable<ErrorLog>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<ErrorLog>();
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
		    this.AddDescription( "PageCode:"+ this.PageCode + "," );        
		    this.AddDescription( "Page:"+ this.Page + "," );        
		    this.AddDescription( "Ip:"+ this.Ip + "," );        
		    this.AddDescription( "Message:"+ this.Message + "," );        
		    this.AddDescription( "Details:"+ this.Details + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class ErrorLogList:CollectionBase<ErrorLogList,ErrorLog>
    {
        public ErrorLogList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_ErrorLogs";
        }
    }
}


