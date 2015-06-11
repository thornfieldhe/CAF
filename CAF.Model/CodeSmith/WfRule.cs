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
	public partial class WfRule :  BaseEntity<WfRule>,IEntityBase
	{   
        public WfRule()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfRules";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private Guid _wfProcessId = Guid.Empty;
        private string _name = String.Empty;
        private Guid? _beginActivityID = Guid.Empty;
        private Guid? _endActivityID = Guid.Empty;
        private string _type = String.Empty;
        private string _condition = String.Empty;
        private Guid _ruleId = Guid.Empty;
        private byte[] _version;
        
        [GuidRequired(ErrorMessage="WfProcess不允许为空")]
		public Guid WfProcessId
		{
			get {return this._wfProcessId;} 
            set {this.SetProperty("WfProcessId",ref this._wfProcessId, value);}           	
		}
        
        public WfProcess WfProcess
		{
			get
			{ 
				return WfProcess.Get(this.WfProcessId);
			}        	
		}

        
        /// <summary>
        /// 规则名称
        /// </summary>
        [StringLength(50,ErrorMessage="规则名称长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// 起始活动Id
        /// </summary>
		public Guid? BeginActivityID
		{
			get {return this._beginActivityID;} 
            set {this.SetProperty("BeginActivityID",ref this._beginActivityID, value);}           	
		}
        
        /// <summary>
        /// 截止活动Id
        /// </summary>
		public Guid? EndActivityID
		{
			get {return this._endActivityID;} 
            set {this.SetProperty("EndActivityID",ref this._endActivityID, value);}           	
		}
        
        /// <summary>
        /// 规则类型
        /// </summary>
        [StringLength(50,ErrorMessage="规则类型长度不能超过50")]
		public string Type
		{
			get {return this._type;} 
            set {this.SetProperty("Type",ref this._type, value);}           	
		}
        
        /// <summary>
        /// 规则路由条件
        /// </summary>
		public string Condition
		{
			get {return this._condition;} 
            set {this.SetProperty("Condition",ref this._condition, value);}           	
		}
        
        /// <summary>
        /// 规则Id
        /// </summary>
        [GuidRequired(ErrorMessage="规则不允许为空")]
		public Guid RuleId
		{
			get {return this._ruleId;} 
            set {this.SetProperty("RuleId",ref this._ruleId, value);}           	
		}
        
 
        
        [Required(ErrorMessage="Version不允许为空")]
		public byte[] Version
		{
			get {return this._version;} 
            set {this.SetProperty("Version",ref this._version, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_WfRules Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WfRules WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WfRules WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WfRules SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WfRules WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYWFPROCESSID = "SELECT * FROM Sys_WfRules WHERE  Status!=-1 And WfProcessId=@WfProcessId";
        const string QUERY_INSERT="INSERT INTO Sys_WfRules ([Id], [WfProcessId], [Name], [BeginActivityID], [EndActivityID], [Type], [Condition], [CreatedDate], [ChangedDate], [Status], [Note], [RuleId], [Version]) VALUES (@Id, @WfProcessId, @Name, @BeginActivityID, @EndActivityID, @Type, @Condition, @CreatedDate, @ChangedDate, @Status, @Note, @RuleId, @Version)";
        const string QUERY_UPDATE = "UPDATE Sys_WfRules SET {0} WHERE  Id = @Id  AND Version=@Version";
                
        #endregion
        		
        #region 静态方法
        
		public static WfRule Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<WfRule>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<WfRule>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static WfRuleList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<WfRule>(QUERY_GETAll, null).ToList();                
                var list=new WfRuleList();
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
		
       public static WfRuleList GetAllByWfProcessId(Guid wfProcessId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<WfRule>(QUERY_GETALLBYWFPROCESSID, new { WfProcessId = wfProcessId }).ToList();
              	var list=new WfRuleList();
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
        public static WfRuleList Query(Expression<Func<IQueryable<WfRule>, IQueryable<WfRule>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<WfRule>();
                expc.Add(exp);
                var items = conn.Query<WfRule>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new WfRuleList();
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
        /// 表达式查询
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static WfRuleList Query(Expression<Func<IQueryable<WfRule>, IQueryable<WfRule>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<WfRule>();
            expc.Add(exp);
            var items = conn.Query<WfRule>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new WfRuleList();
            foreach (var item in items)
            {
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
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
        public static int QueryCount(Expression<Func<IQueryable<WfRule>, IQueryable<WfRule>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfRule>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<WfRule>, IQueryable<WfRule>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfRule>();
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
            return this._changedRows;
		}
		
		public override int Insert(IDbConnection conn, IDbTransaction transaction)
		{
            this._changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return this._changedRows;
		}
		
		#region 私有方法
		
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
		    this.AddDescription( "WfProcessId:"+ this.WfProcessId + "," );        
		    this.AddDescription( "Id:"+ this.Id + "," );        
		    this.AddDescription( "Name:"+ this.Name + "," );        
		    this.AddDescription( "BeginActivityID:"+ this.BeginActivityID + "," );        
		    this.AddDescription( "EndActivityID:"+ this.EndActivityID + "," );        
		    this.AddDescription( "Type:"+ this.Type + "," );        
		    this.AddDescription( "Condition:"+ this.Condition + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "RuleId:"+ this.RuleId + "," );        
		    this.AddDescription( "Version:"+ this.Version + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class WfRuleList:CollectionBase<WfRuleList,WfRule>
    {
        public WfRuleList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfRules";
        }
    }
}


