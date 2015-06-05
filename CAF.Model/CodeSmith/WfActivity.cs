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
	public partial class WfActivity :  BaseEntity<WfActivity>
	{   
        public WfActivity()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfActivities";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private Guid _wfProcessId = Guid.Empty;
        private string _name = String.Empty;
        private string _type = String.Empty;
        private Guid? _post = Guid.Empty;
        private int _statuse;
        private Guid _activityId = Guid.Empty;
        
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
        /// 活动名称
        /// </summary>
        [StringLength(50,ErrorMessage="活动名称长度不能超过50")]
		public string Name
		{
			get {return this._name;} 
            set {this.SetProperty("Name",ref this._name, value);}           	
		}
        
        /// <summary>
        /// 活动类型
        /// </summary>
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
        
        [Required(ErrorMessage="Statuse不允许为空")]
		public int Statuse
		{
			get {return this._statuse;} 
            set {this.SetProperty("Statuse",ref this._statuse, value);}           	
		}
        
        /// <summary>
        /// 活动Id
        /// </summary>
        [GuidRequired(ErrorMessage="活动不允许为空")]
		public Guid ActivityId
		{
			get {return this._activityId;} 
            set {this.SetProperty("ActivityId",ref this._activityId, value);}           	
		}
    
		#endregion
        
        #region 常量定义
        protected const string QUERY_COUNT = "SELECT COUNT(*) AS COUNT FROM Sys_WfActivities Where Status!=-1 ";
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_WfActivities WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_WfActivities WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_WfActivities SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_WfActivities WHERE Id = @Id AND Status!=-1";
        const string QUERY_GETALLBYWFPROCESSID = "SELECT * FROM Sys_WfActivities WHERE  Status!=-1 And WfProcessId=@WfProcessId";
        const string QUERY_INSERT="INSERT INTO Sys_WfActivities ([Id], [WfProcessId], [Name], [Type], [Post], [Statuse], [CreatedDate], [ChangedDate], [Note], [ActivityId]) VALUES (@Id, @WfProcessId, @Name, @Type, @Post, @Statuse, @CreatedDate, @ChangedDate, @Note, @ActivityId)";
        const string QUERY_UPDATE = "UPDATE Sys_WfActivities SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static WfActivity Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<WfActivity>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<WfActivity>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static WfActivityList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<WfActivity>(QUERY_GETAll, null).ToList();                
                var list=new WfActivityList();
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
		
       public static WfActivityList GetAllByWfProcessId(Guid wfProcessId)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var items = conn.Query<WfActivity>(QUERY_GETALLBYWFPROCESSID, new { WfProcessId = wfProcessId }).ToList();
              	var list=new WfActivityList();
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
        public static WfActivityList Query(Expression<Func<IQueryable<WfActivity>, IQueryable<WfActivity>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {                
                var expc = new ExpConditions<WfActivity>();
                expc.Add(exp);
                var items = conn.Query<WfActivity>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
                
                var list=new WfActivityList();
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
        public static WfActivityList Query(Expression<Func<IQueryable<WfActivity>, IQueryable<WfActivity>>> exp,
        IDbConnection conn, IDbTransaction transaction)
        {
            var expc = new ExpConditions<WfActivity>();
            expc.Add(exp);
            var items = conn.Query<WfActivity>(string.Format("{0} {1} {2}", QUERY_GETAll, expc.Where(), expc.OrderBy())).ToList();
            
            var list=new WfActivityList();
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
        public static int QueryCount(Expression<Func<IQueryable<WfActivity>, IQueryable<WfActivity>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfActivity>();
                expc.Add(exp);
                return conn.Query<int>(string.Format(string.Format("{0} {1}", QUERY_COUNT, expc.Where()))).Single();
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static bool Exists(Expression<Func<IQueryable<WfActivity>, IQueryable<WfActivity>>> exp)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var expc = new ExpConditions<WfActivity>();
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
		    this.AddDescription( "Type:"+ this.Type + "," );        
		    this.AddDescription( "Post:"+ this.Post + "," );        
		    this.AddDescription( "Statuse:"+ this.Statuse + "," );        
		    this.AddDescription( "CreatedDate:"+ this.CreatedDate + "," );        
		    this.AddDescription( "ChangedDate:"+ this.ChangedDate + "," );        
		    this.AddDescription( "Note:"+ this.Note + "," );        
		    this.AddDescription( "ActivityId:"+ this.ActivityId + "," );        
        }
		#endregion
				
	}
    
	[Serializable]
    public class WfActivityList:CollectionBase<WfActivityList,WfActivity>
    {
        public WfActivityList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Sys_WfActivities";
        }
    }
}


