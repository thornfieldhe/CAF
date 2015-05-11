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
	public partial class Error :  BaseEntity<Error>
	{   
        public Error()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "ELMAH_Error";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private string _application = String.Empty;
        private string _host = String.Empty;
        private string _type = String.Empty;
        private string _source = String.Empty;
        private string _message = String.Empty;
        private string _user = String.Empty;
        private int _statusCode;
        private DateTime _timeUtc;
        private int _sequence;
        private string _allXml = String.Empty;
        
        [Required(ErrorMessage="Application不允许为空")]
        [StringLength(60,ErrorMessage="Application长度不能超过60")]
		public string Application
		{
			get {return this._application;} 
            set {this.SetProperty("Application",ref this._application, value);}           	
		}
        
        [Required(ErrorMessage="Host不允许为空")]
        [StringLength(50,ErrorMessage="Host长度不能超过50")]
		public string Host
		{
			get {return this._host;} 
            set {this.SetProperty("Host",ref this._host, value);}           	
		}
        
        [Required(ErrorMessage="Type不允许为空")]
        [StringLength(100,ErrorMessage="Type长度不能超过100")]
		public string Type
		{
			get {return this._type;} 
            set {this.SetProperty("Type",ref this._type, value);}           	
		}
        
        [Required(ErrorMessage="Source不允许为空")]
        [StringLength(60,ErrorMessage="Source长度不能超过60")]
		public string Source
		{
			get {return this._source;} 
            set {this.SetProperty("Source",ref this._source, value);}           	
		}
        
        [Required(ErrorMessage="Message不允许为空")]
        [StringLength(500,ErrorMessage="Message长度不能超过500")]
		public string Message
		{
			get {return this._message;} 
            set {this.SetProperty("Message",ref this._message, value);}           	
		}
        
        [Required(ErrorMessage="User不允许为空")]
        [StringLength(50,ErrorMessage="User长度不能超过50")]
		public string User
		{
			get {return this._user;} 
            set {this.SetProperty("User",ref this._user, value);}           	
		}
        
        [Required(ErrorMessage="StatusCode不允许为空")]
		public int StatusCode
		{
			get {return this._statusCode;} 
            set {this.SetProperty("StatusCode",ref this._statusCode, value);}           	
		}
        
        [Required(ErrorMessage="TimeUtc不允许为空")]
        [DateTimeRequired(ErrorMessage="TimeUtc不允许为空")]
		public DateTime TimeUtc
		{
			get {return this._timeUtc;} 
            set {this.SetProperty("TimeUtc",ref this._timeUtc, value);}           	
		}
        
        [Required(ErrorMessage="Sequence不允许为空")]
		public int Sequence
		{
			get {return this._sequence;} 
            set {this.SetProperty("Sequence",ref this._sequence, value);}           	
		}
        
        [Required(ErrorMessage="AllXml不允许为空")]
        [StringLength(16,ErrorMessage="AllXml长度不能超过16")]
		public string AllXml
		{
			get {return this._allXml;} 
            set {this.SetProperty("AllXml",ref this._allXml, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM ELMAH_Error WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM ELMAH_Error WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE ELMAH_Error SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM ELMAH_Error WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO ELMAH_Error ([ErrorId], [Application], [Host], [Type], [Source], [Message], [User], [StatusCode], [TimeUtc], [Sequence], [AllXml]) VALUES (@ErrorId, @Application, @Host, @Type, @Source, @Message, @User, @StatusCode, @TimeUtc, @Sequence, @AllXml)";
        const string QUERY_UPDATE = "UPDATE ELMAH_Error SET {0} WHERE  ErrorId = @ErrorId";
                
        #endregion
        		
        #region 静态方法
        
		public static Error Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Error>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Error>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static ErrorList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<Error>(QUERY_GETAll, null).ToList();                
                var list=new ErrorList();
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
		
		#endregion
				
	}
    
	[Serializable]
    public class ErrorList:CollectionBase<ErrorList,Error>
    {
        public ErrorList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "ELMAH_Error";
        }

        public static ErrorList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Error>(string.Format(QUERY, "ELMAH_Error", query), dynamicObj).ToList();

                var list = new ErrorList();
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
                return conn.Query<int>(string.Format(COUNT, "ELMAH_Error", query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
               return conn.Query<int>(string.Format(COUNT, "ELMAH_Error", query), dynamicObj).Single()>0;
            }
        }
    }
}


