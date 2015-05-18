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
	public partial class WorkflowActivity :  BaseEntity<WorkflowActivity>
	{   
        public WorkflowActivity()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WorkflowActivities";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private string _name = String.Empty;
        private Guid _workflowProcessId = Guid.Empty;
        private string _type = String.Empty;
        private Guid? _post = Guid.Empty;
        
        /// <summary>
        /// 活动名称
        /// </summary>
        [StringLength(50,ErrorMessage="活动名称长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// 工作流Id
        /// </summary>
        [GuidRequired(ErrorMessage="工作流不允许为空")]
		public Guid WorkflowProcessId
		{
			get {return this._workflowProcessId;} 
            set {this.SetProperty("WorkflowProcessId",ref this._workflowProcessId, value);}           	
		}
        
        /// <summary>
        /// 工作流
        /// </summary>
        public WorkflowProcess WorkflowProcess
		{
			get
			{ 
				return WorkflowProcess.Get(this.WorkflowProcessId);
			}        	
		}

        
        /// <summary>
        /// 活动类型
        /// </summary>
        [Required(ErrorMessage="活动类型不允许为空")]
        [StringLength(50,ErrorMessage="活动类型长度不能超过50")]
		public string Type
		{
			get {return this._type;} 
            set {this.SetProperty("Type",ref this._type, value);}           	
		}
        
        /// <summary>
        /// 执行活动岗位
        /// </summary>
		public Guid? Post
		{
			get {return this._post;} 
            set {this.SetProperty("Post",ref this._post, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WorkflowActivities WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WorkflowActivities WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WorkflowActivities SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WorkflowActivities WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYWORKFLOWPROCESSID = "SELECT * FROM Sys_WorkflowActivities WHERE  Status!=-1 And WorkflowProcessId=@WorkflowProcessId";
        const string QUERY_INSERT="INSERT INTO Sys_WorkflowActivities ([Id], [Name], [WorkflowProcessId], [Type], [Post], [CreatedDate], [ChangedDate], [Status], [Note]) VALUES (@Id, @Name, @WorkflowProcessId, @Type, @Post, @CreatedDate, @ChangedDate, @Status, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_WorkflowActivities SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static WorkflowActivity Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<WorkflowActivity>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<WorkflowActivity>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static WorkflowActivityList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<WorkflowActivity>(QUERY_GETAll, null).ToList();                
                var list=new WorkflowActivityList();
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
		
       public static WorkflowActivityList GetAllByWorkflowProcessId(Guid workflowProcessId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<WorkflowActivity>(QUERY_GETALLBYWORKFLOWPROCESSID, new { WorkflowProcessId = workflowProcessId }).ToList();
              	var list=new WorkflowActivityList();
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
    public class WorkflowActivityList:CollectionBase<WorkflowActivityList,WorkflowActivity>
    {
        public WorkflowActivityList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WorkflowActivities";
        }

        public static WorkflowActivityList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<WorkflowActivity>(string.Format(QUERY, "Sys_WorkflowActivities", query), dynamicObj).ToList();

                var list = new WorkflowActivityList();
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
                return conn.Query<int>(string.Format(COUNT, "Sys_WorkflowActivities", query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
               return conn.Query<int>(string.Format(COUNT, "Sys_WorkflowActivities", query), dynamicObj).Single()>0;
            }
        }
    }
}


