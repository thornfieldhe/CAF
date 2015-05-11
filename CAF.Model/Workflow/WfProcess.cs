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
	public partial class WfProcess :  BaseEntity<WfProcess>
	{   
        public WfProcess()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfProcesses";
            base.MarkNew();
            this. _wfActivityListInitalizer = new Lazy<WfActivityList>(() => InitWfActivitys(this), isThreadSafe: true);          
            this. _wfRuleListInitalizer = new Lazy<WfRuleList>(() => InitWfRules(this), isThreadSafe: true);          
            this.WfActivitys= new WfActivityList();        
            this.WfRules= new WfRuleList();        
		}
		
            
		#region 公共属性

        private string _moduleName = String.Empty;
        private Guid _modelId = Guid.Empty;
        private string _document = String.Empty;
        private Guid _organizeId = Guid.Empty;
        private string _creator = String.Empty;
        private WfActivityList  _wfActivityList;
        private Lazy<WfActivityList>  _wfActivityListInitalizer;       
        private WfRuleList  _wfRuleList;
        private Lazy<WfRuleList>  _wfRuleListInitalizer;       
        
        /// <summary>
        /// 模块名称
        /// </summary>
        [Required(ErrorMessage="模块名称不允许为空")]
        [StringLength(50,ErrorMessage="模块名称长度不能超过50")]
		public string ModuleName
		{
			get {return this._moduleName;} 
            set {this.SetProperty("ModuleName",ref this._moduleName, value);}           	
		}
        
        /// <summary>
        /// 模块Id
        /// </summary>
        [GuidRequired(ErrorMessage="模块不允许为空")]
		public Guid ModelId
		{
			get {return this._modelId;} 
            set {this.SetProperty("ModelId",ref this._modelId, value);}           	
		}
        
        /// <summary>
        /// 模块
        /// </summary>
        public Model Model
		{
			get
			{ 
				return Model.Get(this.ModelId);
			}        	
		}

        
        /// <summary>
        /// 文档
        /// </summary>
        [Required(ErrorMessage="文档不允许为空")]
        [StringLength(16,ErrorMessage="文档长度不能超过16")]
		public string Document
		{
			get {return this._document;} 
            set {this.SetProperty("Document",ref this._document, value);}           	
		}
        
        /// <summary>
        /// 组织架构Id
        /// </summary>
        [GuidRequired(ErrorMessage="组织架构不允许为空")]
		public Guid OrganizeId
		{
			get {return this._organizeId;} 
            set {this.SetProperty("OrganizeId",ref this._organizeId, value);}           	
		}
        
        /// <summary>
        /// 组织架构
        /// </summary>
        public Organize Organize
		{
			get
			{ 
				return Organize.Get(this.OrganizeId);
			}        	
		}

        
        /// <summary>
        /// 创建者
        /// </summary>
        [StringLength(50,ErrorMessage="创建者长度不能超过50")]
		public string Creator
		{
			get {return this._creator;} 
            set {this.SetProperty("Creator",ref this._creator, value);}           	
		}
        
        public WfActivityList WfActivitys
        {
            get
            {
                if (!this. _wfActivityListInitalizer.IsValueCreated)
                {
                    this. _wfActivityList = this. _wfActivityListInitalizer.Value;
                }
                return this. _wfActivityList;
            }
            set
            {
                this. _wfActivityList = value;
            }
        }
        
        public WfRuleList WfRules
        {
            get
            {
                if (!this. _wfRuleListInitalizer.IsValueCreated)
                {
                    this. _wfRuleList = this. _wfRuleListInitalizer.Value;
                }
                return this. _wfRuleList;
            }
            set
            {
                this. _wfRuleList = value;
            }
        }
        
        public override bool IsValid
        {
            get
            {
			    this.Errors=new List<string>();
                var isValid = true;
                var baseValid = base.IsValid;
                foreach (var item in this.WfActivitys.Where(item => !item.IsValid))
                {
                    this.Errors.AddRange(item.Errors);
                    isValid = false;
                }
                foreach (var item in this.WfRules.Where(item => !item.IsValid))
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
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WfProcesses WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WfProcesses WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WfProcesses SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WfProcesses WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_WfProcesses (ID, ModuleName, ModelId, Document, OrganizeId, Status, Creator, CreatedDate, ChangedDate, Note) VALUES (@ID, @ModuleName, @ModelId, @Document, @OrganizeId, @Status, @Creator, @CreatedDate, @ChangedDate, @Note)";
        const string QUERY_UPDATE = "UPDATE Sys_WfProcesses SET {0} WHERE  ID = @ID";
                
        #endregion
        		
        #region 静态方法
        
		public static WfProcess Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<WfProcess>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<WfProcess>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                item. _wfActivityListInitalizer = new Lazy<WfActivityList>(() => InitWfActivitys(item), isThreadSafe: true);
                item. _wfRuleListInitalizer = new Lazy<WfRuleList>(() => InitWfRules(item), isThreadSafe: true);
                return item;
            }
		}
		 
		public static WfProcessList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<WfProcess>(QUERY_GETAll, null).ToList();                
                var list=new WfProcessList();
                foreach (var item in items)
                {
                    item.Connection = SqlService.Instance.Connection;
                    item.MarkOld();
                    item. _wfActivityListInitalizer = new Lazy<WfActivityList>(() => InitWfActivitys(item), isThreadSafe: true);
                    item. _wfRuleListInitalizer = new Lazy<WfRuleList>(() => InitWfRules(item), isThreadSafe: true);
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
    		this. _wfActivityListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WfActivitys.SaveChanges(conn,transaction);
            });
    		this. _wfRuleListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WfRules.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
    		this. _wfActivityListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WfActivitys.SaveChanges(conn,transaction);
            });
    		this. _wfRuleListInitalizer.IsValueCreated.IfIsTrue(
			() =>
            {
 				this._changedRows+=this.WfRules.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		#region 私有方法
		
		protected static WfActivityList InitWfActivitys(WfProcess wfProcess)
        {
            var list = WfActivity.GetAllByWfProcessId(wfProcess.Id);
            list.OnMarkDirty += wfProcess.MarkDirty;
			list.OnInsert += wfProcess.PostAddWfActivity;
            return list;
        }
		
		protected  void PostAddWfActivity(WfActivity wfActivity)
        {
			wfActivity.WfProcessId=this.Id;
        }
		
		protected static WfRuleList InitWfRules(WfProcess wfProcess)
        {
            var list = WfRule.GetAllByWfProcessId(wfProcess.Id);
            list.OnMarkDirty += wfProcess.MarkDirty;
			list.OnInsert += wfProcess.PostAddWfRule;
            return list;
        }
		
		protected  void PostAddWfRule(WfRule wfRule)
        {
			wfRule.WfProcessId=this.Id;
        }
		
		#endregion
				
	}
    
	[Serializable]
    public class WfProcessList:CollectionBase<WfProcessList,WfProcess>
    {
        public WfProcessList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfProcesses";
        }

        public static WfProcessList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<WfProcess>(string.Format(QUERY, "Sys_WfProcesses", query), dynamicObj).ToList();

                var list = new WfProcessList();
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
                return conn.Query<int>(string.Format(COUNT, "Sys_WfProcesses", query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
               return conn.Query<int>(string.Format(COUNT, "Sys_WfProcesses", query), dynamicObj).Single()>0;
            }
        }
    }
}


