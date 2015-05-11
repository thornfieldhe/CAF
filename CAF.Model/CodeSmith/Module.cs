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
	public partial class Module :  BaseEntity<Module>
	{   
        public Module()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Modules";
            base.MarkNew();
    		this. _workflowProcessInitalizer = new Lazy<WorkflowProcess>(() => WorkflowProcess.GetByModuleId(Id), isThreadSafe: true);
		}
		
            
		#region 公共属性

        private string _name = String.Empty;
        private string _key = String.Empty;
        private WorkflowProcess  _workflowProcess;
        private Lazy<WorkflowProcess>  _workflowProcessInitalizer;       
        
        /// <summary>
        /// 模块名
        /// </summary>
        [Required(ErrorMessage="模块名不允许为空")]
        [StringLength(50,ErrorMessage="模块名长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// 模块标识
        /// </summary>
        [Required(ErrorMessage="模块标识不允许为空")]
        [StringLength(50,ErrorMessage="模块标识长度不能超过50")]
		public string Key
		{
			get {return this._key;} 
            set {this.SetProperty("Key",ref this._key, value);}           	
		}
        
        public WorkflowProcess WorkflowProcess
        {
            get
            {
                if (!this. _workflowProcessInitalizer.IsValueCreated)
                {
                    this. _workflowProcess = this. _workflowProcessInitalizer.Value;
                }
                return this. _workflowProcess;
            }
            set
            {
                if (!this. _workflowProcessInitalizer.IsValueCreated)
                {
                    this. _workflowProcess = this. _workflowProcessInitalizer.Value;
                }
                this. _workflowProcess = value;
				if (this. _workflowProcess == null)
                {
                    return;
                }
                this. _workflowProcess.OnPropertyChange += this.MarkDirty;
                this. _workflowProcess.ModuleId = this.Id;
            }
        }
        public override bool IsValid
        {
            get
            {
			    this.Errors=new List<string>();
                var isValid = true;
                var baseValid = base.IsValid;
                if (this. _workflowProcessInitalizer.IsValueCreated && this.WorkflowProcess!=null && !this.WorkflowProcess.IsValid)
                {
                    this.Errors.AddRange(this.WorkflowProcess.Errors);
                    isValid = false;
                }
               return baseValid && isValid;
            }
            protected set { this._isValid = value; }
        }
        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Modules WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Modules WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Modules SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Modules WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_Modules ([Id], [Name], [CreatedDate], [ChangedDate], [Key], [Status], [Note]) VALUES (@Id, @Name, @CreatedDate, @ChangedDate, @Key, @Status, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_Modules SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static Module Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Module>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Module>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
				item. _workflowProcessInitalizer = new Lazy<WorkflowProcess>(() => InitWorkflowProcess(item), isThreadSafe: true);                   
                return item;
            }
		}
		 
		public static ModuleList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<Module>(QUERY_GETAll, null).ToList();                
                var list=new ModuleList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _workflowProcessInitalizer = new Lazy<WorkflowProcess>(() => InitWorkflowProcess(item), isThreadSafe: true);      
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
			if(this. _workflowProcessInitalizer.IsValueCreated && this.WorkflowProcess!=null) 
            {
 				this._changedRows += this.WorkflowProcess.SaveChange(conn, transaction);
            }
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
			if(this. _workflowProcessInitalizer.IsValueCreated && this.WorkflowProcess!=null) 
            {
 				this._changedRows += this.WorkflowProcess.SaveChange(conn, transaction);
            }
            return this._changedRows;
		}
		
		#region 私有方法
		
        protected static WorkflowProcess InitWorkflowProcess(Module module)
        {
            var item = WorkflowProcess.GetByModuleId(module.Id);
            if (item == null)
            {
                return null;
            }
            item.ModuleId = module.Id;
			item.OnPropertyChange += module.MarkDirty;
            return item;
        }
		#endregion
				
	}
    
	[Serializable]
    public class ModuleList:CollectionBase<ModuleList,Module>
    {
        public ModuleList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_Modules";
        }

        public static ModuleList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Module>(string.Format(QUERY, "Sys_Modules", query), dynamicObj).ToList();

                var list = new ModuleList();
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
                return conn.Query<int>(string.Format(COUNT, "Sys_Modules", query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
               return conn.Query<int>(string.Format(COUNT, "Sys_Modules", query), dynamicObj).Single()>0;
            }
        }
    }
}


