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
	public partial class WorkflowProcess :  BaseEntity<WorkflowProcess>,IEntityBase
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
        private string _note = String.Empty;
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
        
        [StringLength(50,ErrorMessage="Note长度不能超过50")]
		public string Note
		{
			get {return this._note;} 
            set {this.SetProperty("Note",ref this._note, value);}           	
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
        
        public override void Validate()
        {
            foreach (var item in this.WorkflowActivitys)
            {
                item.Validate();
            }
            foreach (var item in this.WorkflowRules)
            {
                item.Validate();
            }
           base.Validate();
        }
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_WorkflowProcesses Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WorkflowProcesses WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WorkflowProcesses WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WorkflowProcesses SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WorkflowProcesses WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_WorkflowProcesses ([Id], [Name], [Document], [CreatedDate], [ChangedDate], [Status], [Note]) VALUES (@Id, @Name, @Document, @CreatedDate, @ChangedDate, @Status, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_WorkflowProcesses SET {0} WHERE  Id = @Id  AND Version=@Version";
                
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
        public static WorkflowProcessList Query(Expression<Func<IQueryable<WorkflowProcess>, IQueryable<WorkflowProcess>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<WorkflowProcess>();
                expc.Add(exp);
                var items = conn.Query<WorkflowProcess>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
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
        
                /// <summary>
        /// 表达式查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static WorkflowProcessList Query(Expression<Func<IQueryable<WorkflowProcess>, IQueryable<WorkflowProcess>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<WorkflowProcess>();
            expc.Add(exp);
            var items = conn.Query<WorkflowProcess>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
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

        /// <summary>
        /// 数量查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static int QueryCount(Expression<Func<IQueryable<WorkflowProcess>, IQueryable<WorkflowProcess>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WorkflowProcess>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<WorkflowProcess>, IQueryable<WorkflowProcess>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WorkflowProcess>();
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
    		this. _workflowActivityListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.WorkflowActivitys.SaveChanges(conn,transaction);
            });
    		this. _workflowRuleListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.WorkflowRules.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
    		this. _workflowActivityListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.WorkflowActivitys.SaveChanges(conn,transaction);
            });
    		this. _workflowRuleListInitalizer.IsValueCreated.IfTrue(
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
		
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
		    this.AddDescription( "Id:"+ this.Id + "," );        
		    this.AddDescription( "Name:"+ this.Name + "," );        
		    this.AddDescription( "Document:"+ this.Document + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "Version:"+ this.Version + "," );        
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
    }
}


