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
	public partial class WfAuditOption :  BaseEntity<WfAuditOption>,IEntityBase
	{   
        public WfAuditOption()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfAuditOptions";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private Guid _wfProcessId = Guid.Empty;
        private Guid _wfActivityId = Guid.Empty;
        private string _postName = String.Empty;
        private string _auditOpinion = String.Empty;
        private string _note = String.Empty;
        private string _auditName = String.Empty;
        
        /// <summary>
        /// 工作流Id
        /// </summary>
        [GuidRequired(ErrorMessage="工作流不允许为空")]
		public Guid WfProcessId
		{
			get {return this._wfProcessId;} 
            set {this.SetProperty("WfProcessId",ref this._wfProcessId, value);}           	
		}
        
        /// <summary>
        /// 工作流
        /// </summary>
        public WfProcess WfProcess
		{
			get
			{ 
				return WfProcess.Get(this.WfProcessId);
			}        	
		}

        
        /// <summary>
        /// 活动Id
        /// </summary>
        [GuidRequired(ErrorMessage="活动不允许为空")]
		public Guid WfActivityId
		{
			get {return this._wfActivityId;} 
            set {this.SetProperty("WfActivityId",ref this._wfActivityId, value);}           	
		}
        
        /// <summary>
        /// 活动
        /// </summary>
        public WfActivity WfActivity
		{
			get
			{ 
				return WfActivity.Get(this.WfActivityId);
			}        	
		}

        
        /// <summary>
        /// 岗位
        /// </summary>
        [Required(ErrorMessage="岗位不允许为空")]
        [StringLength(50,ErrorMessage="岗位长度不能超过50")]
		public string PostName
		{
			get {return this._postName;} 
            set {this.SetProperty("PostName",ref this._postName, value);}           	
		}
        
        /// <summary>
        /// 审核意见
        /// </summary>
        [StringLength(500,ErrorMessage="审核意见长度不能超过500")]
		public string AuditOpinion
		{
			get {return this._auditOpinion;} 
            set {this.SetProperty("AuditOpinion",ref this._auditOpinion, value);}           	
		}
        
        [StringLength(500,ErrorMessage="Note长度不能超过500")]
		public string Note
		{
			get {return this._note;} 
            set {this.SetProperty("Note",ref this._note, value);}           	
		}
        
        /// <summary>
        /// 审核者
        /// </summary>
        [StringLength(50,ErrorMessage="审核者长度不能超过50")]
		public string AuditName
		{
			get {return this._auditName;} 
            set {this.SetProperty("AuditName",ref this._auditName, value);}           	
		}
        
        
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_WfAuditOptions Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WfAuditOptions WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WfAuditOptions WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WfAuditOptions SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WfAuditOptions WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Sys_WfAuditOptions ([Id], [WfProcessId], [WfActivityId], [PostName], [AuditOpinion], [CreatedDate], [ChangedDate], [Status], [Note], [AuditName]) VALUES (@Id, @WfProcessId, @WfActivityId, @PostName, @AuditOpinion, @CreatedDate, @ChangedDate, @Status, @Note, @AuditName)";
        const string QUERY_UPDATE = "UPDATE Sys_WfAuditOptions SET {0} WHERE  Id = @Id  AND Version=@Version";
                
        #endregion
        		
        #region 静态方法
        
		public static WfAuditOption Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<WfAuditOption>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<WfAuditOption>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static WfAuditOptionList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<WfAuditOption>(QUERY_GETAll, null).ToList();                
                var list=new WfAuditOptionList();
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
        public static WfAuditOptionList Query(Expression<Func<IQueryable<WfAuditOption>, IQueryable<WfAuditOption>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<WfAuditOption>();
                expc.Add(exp);
                var items = conn.Query<WfAuditOption>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new WfAuditOptionList();
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
        public static WfAuditOptionList Query(Expression<Func<IQueryable<WfAuditOption>, IQueryable<WfAuditOption>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<WfAuditOption>();
            expc.Add(exp);
            var items = conn.Query<WfAuditOption>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new WfAuditOptionList();
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
        public static int QueryCount(Expression<Func<IQueryable<WfAuditOption>, IQueryable<WfAuditOption>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfAuditOption>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<WfAuditOption>, IQueryable<WfAuditOption>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfAuditOption>();
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
		    this.AddDescription( "Id:"+ this.Id + "," );        
		    this.AddDescription( "WfProcessId:"+ this.WfProcessId + "," );        
		    this.AddDescription( "WfActivityId:"+ this.WfActivityId + "," );        
		    this.AddDescription( "PostName:"+ this.PostName + "," );        
		    this.AddDescription( "AuditOpinion:"+ this.AuditOpinion + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Status:"+ this.Status + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "AuditName:"+ this.AuditName + "," );        
		    this.AddDescription( "Version:"+ this.Version + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class WfAuditOptionList:CollectionBase<WfAuditOptionList,WfAuditOption>
    {
        public WfAuditOptionList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfAuditOptions";
        }
    }
}


