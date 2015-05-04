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
	public partial class Directory :  BaseEntity<Directory>
	{   
        public Directory()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Directories";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private string _name = String.Empty;
        private string _url = String.Empty;
        private Guid? _parentId = Guid.Empty;
        private string _level = String.Empty;
        private int _sort;
        
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage="名称不允许为空")]
        [StringLength(50,ErrorMessage="名称长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// Url地址
        /// </summary>
        [StringLength(100,ErrorMessage="Url地址长度不能超过100")]
		public string Url
		{
			get {return this._url;} 
            set {this.SetProperty("Url",ref this._url, value);}           	
		}
        
        /// <summary>
        /// 父目录
        /// </summary>
		public Guid? ParentId
		{
			get {return this._parentId;} 
            set {this.SetProperty("ParentId",ref this._parentId, value);}           	
		}
        
        /// <summary>
        /// 父目录
        /// </summary>
        public Directory Parent
		{
			get
			{ 
				return !this.ParentId.HasValue ? null : Directory.Get(this.ParentId.Value);
			}        	
		}

        
        /// <summary>
        /// 层级
        /// </summary>
        [StringLength(20,ErrorMessage="层级长度不能超过20")]
		public string Level
		{
			get {return this._level;} 
            set {this.SetProperty("Level",ref this._level, value);}           	
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
        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Directories WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Directories WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Directories SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Directories WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYPARENTID = "SELECT * FROM Sys_Directories WHERE  Status!=-1 And ParentId=@ParentId";
        const string QUERY_INSERT="INSERT INTO Sys_Directories (Id, Name, Url, ParentId, Level, Sort, Note, Status, CreatedDate, ChangedDate) VALUES (@Id, @Name, @Url, @ParentId, @Level, @Sort, @Note, @Status, @CreatedDate, @ChangedDate)";
        const string QUERY_UPDATE = "UPDATE Sys_Directories SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static Directory Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Directory>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Directory>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static DirectoryList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<Directory>(QUERY_GETAll, null).ToList();                
                var list=new DirectoryList();
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
		
       public static DirectoryList GetAllByParentId(Guid parentId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<Directory>(QUERY_GETALLBYPARENTID, new { ParentId = parentId }).ToList();
              	var list=new DirectoryList();
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
    public class DirectoryList:CollectionBase<DirectoryList,Directory>
    {
        public DirectoryList() {this.Connection = SqlService.Instance.Connection; }

        protected const string tableName = "Sys_Directories";
        
        public static DirectoryList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new DirectoryList();
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


