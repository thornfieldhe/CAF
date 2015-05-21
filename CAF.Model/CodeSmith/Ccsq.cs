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
	public partial class Ccsq :  BaseEntity<Ccsq>
	{   
        public Ccsq()
		{
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Wf_Ccsq";
            base.MarkNew();
		}
		
            
		#region 公共属性

        private DateTime _ccsjq;
        private DateTime _ccsjz;
        private string _ccdd = String.Empty;
        private int? _jtgj;
        private Guid _createdBy = Guid.Empty;
        private Guid _modifyBy = Guid.Empty;
        private Guid _organizeId = Guid.Empty;
        
        /// <summary>
        /// 出差起始时间
        /// </summary>
        [Required(ErrorMessage="出差起始时间不允许为空")]
        [DateTimeRequired(ErrorMessage="出差起始时间不允许为空")]
		public DateTime Ccsjq
		{
			get {return this._ccsjq;} 
            set {this.SetProperty("Ccsjq",ref this._ccsjq, value);}           	
		}
        
        /// <summary>
        /// 出差截止时间
        /// </summary>
        [Required(ErrorMessage="出差截止时间不允许为空")]
        [DateTimeRequired(ErrorMessage="出差截止时间不允许为空")]
		public DateTime Ccsjz
		{
			get {return this._ccsjz;} 
            set {this.SetProperty("Ccsjz",ref this._ccsjz, value);}           	
		}
        
        /// <summary>
        /// 目的地
        /// </summary>
        [Required(ErrorMessage="目的地不允许为空")]
        [StringLength(50,ErrorMessage="目的地长度不能超过50")]
		public string Ccdd
		{
			get {return this._ccdd;} 
            set {this.SetProperty("Ccdd",ref this._ccdd, value);}           	
		}
        
        /// <summary>
        /// 交通工具
        /// </summary>
		public int? jtgj
		{
			get {return this._jtgj;} 
            set {this.SetProperty("jtgj",ref this._jtgj, value);}           	
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
        
        /// <summary>
        /// 更新者
        /// </summary>
        [GuidRequired(ErrorMessage="更新者不允许为空")]
		public Guid ModifyBy
		{
			get {return this._modifyBy;} 
            set {this.SetProperty("ModifyBy",ref this._modifyBy, value);}           	
		}
        
        /// <summary>
        /// 创建者所属部门
        /// </summary>
        [GuidRequired(ErrorMessage="创建者所属部门不允许为空")]
		public Guid OrganizeId
		{
			get {return this._organizeId;} 
            set {this.SetProperty("OrganizeId",ref this._organizeId, value);}           	
		}
        
        /// <summary>
        /// 创建者所属部门
        /// </summary>
        public Organize Organize
		{
			get
			{ 
				return Organize.Get(this.OrganizeId);
			}        	
		}

        
        
		#endregion
        
        #region 常量定义
        
        const string QUERY_GETBYID = "SELECT Top 1 * FROM Wf_Ccsq WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Wf_Ccsq WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Wf_Ccsq SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Wf_Ccsq WHERE Id = @Id AND Status!=-1";
        const string QUERY_INSERT="INSERT INTO Wf_Ccsq ([Id], [Ccsjq], [Ccsjz], [Ccdd], [jtgj], [Note], [CreatedDate], [ChangedDate], [Status], [CreatedBy], [ModifyBy], [OrganizeId]) VALUES (@Id, @Ccsjq, @Ccsjz, @Ccdd, @jtgj, @Note, @CreatedDate, @ChangedDate, @Status, @CreatedBy, @ModifyBy, @OrganizeId)";
        const string QUERY_UPDATE = "UPDATE Wf_Ccsq SET {0} WHERE  Id = @Id";
                
        #endregion
        		
        #region 静态方法
        
		public static Ccsq Get(Guid id)
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item= conn.Query<Ccsq>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Ccsq>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
		}
		 
		public static CcsqList GetAll()
		{
			using (IDbConnection conn = SqlService.Instance.Connection)
            {               
                var items = conn.Query<Ccsq>(QUERY_GETAll, null).ToList();                
                var list=new CcsqList();
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
    public class CcsqList:CollectionBase<CcsqList,Ccsq>
    {
        public CcsqList() 
        {
            this.Connection = SqlService.Instance.Connection;
            this.TableName = "Wf_Ccsq";
        }

        public static CcsqList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Ccsq>(string.Format(QUERY, "Wf_Ccsq", query), dynamicObj).ToList();

                var list = new CcsqList();
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
                return conn.Query<int>(string.Format(COUNT, "Wf_Ccsq", query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
               return conn.Query<int>(string.Format(COUNT, "Wf_Ccsq", query), dynamicObj).Single()>0;
            }
        }
    }
}


