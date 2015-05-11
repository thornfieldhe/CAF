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
	public partial class WorkflowProcess :  BaseEntity<WorkflowProcess>
	{   
        public WorkflowProcess()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WorkflowProcesses";
            base.MarkNew();
            this. _workflowActivityListInitalizer = new Lazy<WorkflowActivityList>(() => InitWorkflowActivitys(this), isThreadSafe: true);          
            this. _workflowRuleListInitalizer = new Lazy<WorkflowRuleList>(() => InitWorkflowRules(this), isThreadSafe: true);          
            this.WorkflowActivitys= new WorkflowActivityList();        
            this.WorkflowRules= new WorkflowRuleList();        
		}
		
            
		#region 公共属性

        private string _name = String.Empty;
        private string _document = String.Empty;
        private Guid _moduleId = Guid.Empty;
        private WorkflowActivityList  _workflowActivityList;
        private Lazy<WorkflowActivityList>  _workflowActivityListInitalizer;       
        private WorkflowRuleList  _workflowRuleList;
        private Lazy<WorkflowRuleList>  _workflowRuleListInitalizer;       
        
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
        /// 序列化工作流对象
        /// </summary>
        [Required(ErrorMessage="序列化工作流对象不允许为空")]
		public string Document
		{
			get {return this._document;} 
            set {this.SetProperty("Document",ref this._document, value);}           	
		}
        
        /// <summary>
        /// 模块Id
        /// </summary>
        [GuidRequired(ErrorMessage="模块不允许为空")]
		public Guid ModuleId
		{
			get {return this._moduleId;} 
            set {this.SetProperty("ModuleId",ref this._moduleId, value);}           	
		}
        
        /// <summary>
        /// 模块
        /// </summary>
        public Module Module
		{
			get
			{ 
				return Module.Get(this.ModuleId);
			}        	
		}

        
        public WorkflowActivityList WorkflowActivitys
        {
            get
            {
                if (!this. _workflowActivityListInitalizer.IsValueCreated)
                {
                    this. _workflowActivityList = this. _workflowActivityListInitalizer.Value;
                }
                return this. _workflowActivityList;
            }
            set
            {
                this. _workflowActivityList = value;
            }
        }
        
        public WorkflowRuleList WorkflowRules
        {
            get
            {
                if (!this. _workflowRuleListInitalizer.IsValueCreated)
                {
                    this. _workflowRuleList = this. _workflowRuleListInitalizer.Value;
                }
                return this. _workflowRuleList;
            }
            set
            {
                this. _workflowRuleList = value;
            }
        }
        
        public override bool IsValid
        {
            get
            {
			    this.Errors=new List<string>();
                var isValid = true;
                var baseValid = base.IsValid;
                foreach (var item in this.WorkflowActivitys.Where(item => !item.IsValid))
                {
                    this.Errors.AddRange(item.Errors);
                    isValid = false;
                }
                foreach (var item in this.WorkflowRules.Where(item => !item.IsValid))
                {
                    this.Errors.AddRange(item.Errors);
                    isValid = false;
                }
               return baseValid && isValid;
            }
            protected set { this._isValid = value; }
        }
        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WorkflowProcesses WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WorkflowProcesses WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WorkflowProcesses SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WorkflowProcesses WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETBYMODULEID = "SELECT Top 1 * FROM Sys_WorkflowProcesses WHERE Status!=-1 And ModuleId=@ModuleId";
        const string QUERY_INSERT="INSERT INTO Sys_WorkflowProcesses ([Id], [Name], [Document], [CreatedDate], [ChangedDate], [Status], [Note], [ModuleId]) VALUES (@Id, @Name, @Document, @CreatedDate, @ChangedDate, @Status, @Note, @ModuleId)";
        const string QUERY_UPDATE = "UPDATE Sys_WorkflowProcesses SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static WorkflowProcess Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<WorkflowProcess>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<WorkflowProcess>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                item. _workflowActivityListInitalizer = new Lazy<WorkflowActivityList>(() => InitWorkflowActivitys(item), isThreadSafe: true);
                item. _workflowRuleListInitalizer = new Lazy<WorkflowRuleList>(() => InitWorkflowRules(item), isThreadSafe: true);
                return item;
            }
		}
		 
		public static WorkflowProcessList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<WorkflowProcess>(QUERY_GETAll, null).ToList();                
                var list=new WorkflowProcessList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _workflowActivityListInitalizer = new Lazy<WorkflowActivityList>(() => InitWorkflowActivitys(item), isThreadSafe: true);
                    item. _workflowRuleListInitalizer = new Lazy<WorkflowRuleList>(() => InitWorkflowRules(item), isThreadSafe: true);
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
		}        
		
		public static WorkflowProcess GetByModuleId(Guid moduleId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var item= conn.Query<WorkflowProcess>(QUERY_GETBYMODULEID, new { ModuleId = moduleId }).SingleOrDefault<WorkflowProcess>();
                if (item != null)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _workflowActivityListInitalizer = new Lazy<WorkflowActivityList>(() => InitWorkflowActivitys(item), isThreadSafe: true);
                    item. _workflowRuleListInitalizer = new Lazy<WorkflowRuleList>(() => InitWorkflowRules(item), isThreadSafe: true);
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
    		this. _workflowActivityListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WorkflowActivitys.SaveChanges(conn,transaction);
            });
    		this. _workflowRuleListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WorkflowRules.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
    		this. _workflowActivityListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WorkflowActivitys.SaveChanges(conn,transaction);
            });
    		this. _workflowRuleListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WorkflowRules.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		#region 私有方法
		
		protected static WorkflowActivityList InitWorkflowActivitys(WorkflowProcess workflowProcess)
        {
            var list = WorkflowActivity.GetAllByWorkflowProcessId(workflowProcess.Id);
            list.OnMarkDirty += workflowProcess.MarkDirty;
			list.OnInsert += workflowProcess.PostAddWorkflowActivity;
            return list;
        }
		
		protected  void PostAddWorkflowActivity(WorkflowActivity workflowActivity)
        {
			workflowActivity.WorkflowProcessId=this.Id;
        }
		
		protected static WorkflowRuleList InitWorkflowRules(WorkflowProcess workflowProcess)
        {
            var list = WorkflowRule.GetAllByWorkflowProcessId(workflowProcess.Id);
            list.OnMarkDirty += workflowProcess.MarkDirty;
			list.OnInsert += workflowProcess.PostAddWorkflowRule;
            return list;
        }
		
		protected  void PostAddWorkflowRule(WorkflowRule workflowRule)
        {
			workflowRule.WorkflowProcessId=this.Id;
        }
		
		#endregion
				
	}
    
	[Serializable]
    public class WorkflowProcessList:CollectionBase<WorkflowProcessList,WorkflowProcess>
    {
        public WorkflowProcessList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WorkflowProcesses";
        }

        public static WorkflowProcessList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<WorkflowProcess>(string.Format(QUERY, "Sys_WorkflowProcesses", query), dynamicObj).ToList();

                var list = new WorkflowProcessList();
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
                return conn.Query<int>(string.Format(COUNT, "Sys_WorkflowProcesses", query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
               return conn.Query<int>(string.Format(COUNT, "Sys_WorkflowProcesses", query), dynamicObj).Single()>0;
            }
        }
    }
}


