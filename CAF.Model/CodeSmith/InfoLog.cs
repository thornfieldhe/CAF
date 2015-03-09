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
	public partial class InfoLog :  BaseEntity<InfoLog>
	{   
        public InfoLog()
		{
            Connection = SqlService.Instance.Connection;
            base.MarkNew();
		}
		
            
		#region 公共属性

        private DateTime _logTime;
        private string _userName = String.Empty;
        private string _page = String.Empty;
        private string _action = String.Empty;
        
        /// <summary>
        /// 操作时间
        /// </summary>
        [Required(ErrorMessage="操作时间不允许为空")]
        [DateTimeRequired(ErrorMessage="操作时间不允许为空")]
		public DateTime LogTime
		{
			get {return _logTime;} 
            set {SetProperty("LogTime",ref _logTime, value);}           	
		}
        
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage="用户名不允许为空")]
        [StringLength(20,ErrorMessage="用户名长度不能超过20")]
		public string UserName
		{
			get {return _userName;} 
            set {SetProperty("UserName",ref _userName, value);}           	
		}
        
        /// <summary>
        /// 操作页
        /// </summary>
        [Required(ErrorMessage="操作页不允许为空")]
        [StringLength(50,ErrorMessage="操作页长度不能超过50")]
		public string Page
		{
			get {return _page;} 
            set {SetProperty("Page",ref _page, value);}           	
		}
        
        /// <summary>
        /// 活动
        /// </summary>
        [Required(ErrorMessage="活动不允许为空")]
        [StringLength(50,ErrorMessage="活动长度不能超过50")]
		public string Action
		{
			get {return _action;} 
            set {SetProperty("Action",ref _action, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_InfoLogs WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_InfoLogs WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_InfoLogs SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_InfoLogs WHERE Id = @Id";
        const string QUERY_INSERT="INSERT INTO Sys_InfoLogs (Id, LogTime, UserName, Page, Action) VALUES (@Id, @LogTime, @UserName, @Page, @Action)";
        const string QUERY_UPDATE = "UPDATE Sys_InfoLogs SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static InfoLog Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<InfoLog>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<InfoLog>();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                return item;
            }
		}
		 
		public static InfoLogList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<InfoLog>(QUERY_GETAll, null).ToList();                
                var list=new InfoLogList();
                foreach (var item in items)
                {
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
            _updateParameters+=", ChangedDate = GetDate()";
			var query = String.Format(QUERY_UPDATE, _updateParameters.TrimStart(','));
			_changedRows+= conn.Execute(query, this, transaction, null, null);
            return _changedRows;
		}
		
		internal override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            _changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return _changedRows;
		}
		
		#region 私有方法
		
		#endregion
				
	}
    
	[Serializable]
    public class InfoLogList:CollectionBase<InfoLogList,InfoLog>
    {
        public InfoLogList() { }

        protected const string tableName = "Sys_InfoLogs";
        
        public static InfoLogList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<InfoLog>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new InfoLogList();
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


