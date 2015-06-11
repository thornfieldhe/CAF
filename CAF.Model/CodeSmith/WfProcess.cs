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
	public partial class WfProcess :  BaseEntity<WfProcess>,IEntityBase
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

        private string _name = String.Empty;
        private Guid _modulelId = Guid.Empty;
        private string _document = String.Empty;
        private Guid _organizeId = Guid.Empty;
        private Guid _createdBy = Guid.Empty;
        private Guid _modifyBy = Guid.Empty;
        private byte[] _version;
        private WfActivityList  _wfActivityList;
        private Lazy<WfActivityList>  _wfActivityListInitalizer;       
        private WfRuleList  _wfRuleList;
        private Lazy<WfRuleList>  _wfRuleListInitalizer;       
        
        /// <summary>
        /// 模块名称
        /// </summary>
        [Required(ErrorMessage="模块名称不允许为空")]
        [StringLength(50,ErrorMessage="模块名称长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// 模块Id
        /// </summary>
        [GuidRequired(ErrorMessage="模块不允许为空")]
		public Guid ModulelId
		{
			get {return this._modulelId;} 
            set {this.SetProperty("ModulelId",ref this._modulelId, value);}           	
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
        [GuidRequired(ErrorMessage="创建者不允许为空")]
		public Guid CreatedBy
		{
			get {return this._createdBy;} 
            set {this.SetProperty("CreatedBy",ref this._createdBy, value);}           	
		}
        
        [GuidRequired(ErrorMessage="ModifyBy不允许为空")]
		public Guid ModifyBy
		{
			get {return this._modifyBy;} 
            set {this.SetProperty("ModifyBy",ref this._modifyBy, value);}           	
		}
        
        [Required(ErrorMessage="Version不允许为空")]
		public byte[] Version
		{
			get {return this._version;} 
            set {this.SetProperty("Version",ref this._version, value);}           	
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
        
        public override void Validate()
        {
            foreach (var item in this.WfActivitys)
            {
                item.Validate();
            }
            foreach (var item in this.WfRules)
            {
                item.Validate();
            }
           base.Validate();
        }
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_WfProcesses Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WfProcesses WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WfProcesses WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WfProcesses SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WfProcesses WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_WfProcesses ([ID], [Name], [ModulelId], [Document], [OrganizeId], [Status], [CreatedBy], [CreatedDate], [ChangedDate], [Note], [ModifyBy], [Version]) VALUES (@ID, @Name, @ModulelId, @Document, @OrganizeId, @Status, @CreatedBy, @CreatedDate, @ChangedDate, @Note, @ModifyBy, @Version)";
        const string QUERY_UPDATE = "UPDATE Sys_WfProcesses SET {0} WHERE  ID = @ID  AND Version=@Version";
                
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
        public static WfProcessList Query(Expression<Func<IQueryable<WfProcess>, IQueryable<WfProcess>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<WfProcess>();
                expc.Add(exp);
                var items = conn.Query<WfProcess>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
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
        /// 表达式查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static WfProcessList Query(Expression<Func<IQueryable<WfProcess>, IQueryable<WfProcess>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<WfProcess>();
            expc.Add(exp);
            var items = conn.Query<WfProcess>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
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

        /// <summary>
        /// 数量查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static int QueryCount(Expression<Func<IQueryable<WfProcess>, IQueryable<WfProcess>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfProcess>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<WfProcess>, IQueryable<WfProcess>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfProcess>();
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
    		this. _wfActivityListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.WfActivitys.SaveChanges(conn,transaction);
            });
    		this. _wfRuleListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.WfRules.SaveChanges(conn,transaction);
            });
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
    		this. _wfActivityListInitalizer.IsValueCreated.IfTrue(
			() =>
            {
 				this._changedRows+=this.WfActivitys.SaveChanges(conn,transaction);
            });
    		this. _wfRuleListInitalizer.IsValueCreated.IfTrue(
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
		
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
		    this.AddDescription( "Id:"+ this.Id + "," );        
		    this.AddDescription( "Name:"+ this.Name + "," );        
		    this.AddDescription( "ModulelId:"+ this.ModulelId + "," );        
		    this.AddDescription( "Document:"+ this.Document + "," );        
		    this.AddDescription( "OrganizeId:"+ this.OrganizeId + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "CreatedBy:"+ this.CreatedBy + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "ModifyBy:"+ this.ModifyBy + "," );        
		    this.AddDescription( "Version:"+ this.Version + "," );        
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
    }
}


