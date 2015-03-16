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
	public partial class LoginLog :  BaseEntity<LoginLog>
	{   
        public LoginLog()
		{
            Connection = SqlService.Instance.Connection;
            base.MarkNew();
		}
		
            
		#region 公共属性

        private DateTime _logTime;
        private string _userName = String.Empty;
        private string _ip = String.Empty;
        private int _isLogin;
        
        /// <summary>
        /// 登陆时间
        /// </summary>
        [Required(ErrorMessage="登陆时间不允许为空")]
        [DateTimeRequired(ErrorMessage="登陆时间不允许为空")]
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
        
        [Required(ErrorMessage="Ip不允许为空")]
        [StringLength(20,ErrorMessage="Ip长度不能超过20")]
		public string Ip
		{
			get {return _ip;} 
            set {SetProperty("Ip",ref _ip, value);}           	
		}
        
        /// <summary>
        /// 是否登陆
        /// </summary>
        [Required(ErrorMessage="是否登陆不允许为空")]
		public int IsLogin
		{
			get {return _isLogin;} 
            set {SetProperty("IsLogin",ref _isLogin, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_LoginLogs WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_LoginLogs WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_LoginLogs SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_LoginLogs WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_LoginLogs (Id, LogTime, UserName, Ip, IsLogin) VALUES (@Id, @LogTime, @UserName, @Ip, @IsLogin)";
        const string QUERY_UPDATE = "UPDATE Sys_LoginLogs SET {0} WHERE  Id = @Id";
                
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
                item.MarkOld();
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
        
		
		public override int Delete(IDbConnection conn, IDbTransaction transaction)
		{
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, new { Id = Id }, transaction, null, null);
		}
		
		public override int Update(IDbConnection conn, IDbTransaction transaction)
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
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            _changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return _changedRows;
		}
		
		#region 私有方法
		
		#endregion
				
	}
    
	[Serializable]
    public class LoginLogList:CollectionBase<LoginLogList,LoginLog>
    {
        public LoginLogList() { }

        protected const string tableName = "Sys_LoginLogs";
        
        public static LoginLogList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<LoginLog>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new LoginLogList();
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


