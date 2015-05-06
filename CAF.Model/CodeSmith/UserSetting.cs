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
	public partial class UserSetting :  BaseEntity<UserSetting>
	{   
        public UserSetting()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_UserSettings";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private string _settings = String.Empty;
        private Guid _userId = Guid.Empty;
        
        /// <summary>
        /// 配置文件
        /// </summary>
		public string Settings
		{
			get {return this._settings;} 
            set {this.SetProperty("Settings",ref this._settings, value);}           	
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

        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_UserSettings WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_UserSettings WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_UserSettings SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_UserSettings WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETBYUSERID = "SELECT Top 1 * FROM Sys_UserSettings WHERE Status!=-1 And UserId=@UserId";
        const string QUERY_INSERT="INSERT INTO Sys_UserSettings (Id, Status, CreatedDate, ChangedDate, Note, Settings, UserId) VALUES (@Id, @Status, @CreatedDate, @ChangedDate, @Note, @Settings, @UserId)";
        const string QUERY_UPDATE = "UPDATE Sys_UserSettings SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static UserSetting Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<UserSetting>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<UserSetting>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static UserSettingList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<UserSetting>(QUERY_GETAll, null).ToList();                
                var list=new UserSettingList();
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
		
		public static UserSetting GetByUserId(Guid userId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var item= conn.Query<UserSetting>(QUERY_GETBYUSERID, new { UserId = userId }).SingleOrDefault<UserSetting>();
                if (item != null)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                }                
                return item;
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
    public class UserSettingList:CollectionBase<UserSettingList,UserSetting>
    {
        public UserSettingList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_UserSettings";
        }

        public static UserSettingList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<UserSetting>(string.Format(QUERY, "Sys_UserSettings", query), dynamicObj).ToList();

                var list = new UserSettingList();
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
                return conn.Query<int>(string.Format(COUNT, "Sys_UserSettings", query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
               return conn.Query<int>(string.Format(COUNT, "Sys_UserSettings", query), dynamicObj).Single()>0;
            }
        }
    }
}


