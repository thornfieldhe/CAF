using System;
using System.Data;

namespace CAF
{

    using CAF.Exceptions;


    [Serializable]
    public abstract class BaseEntity<T> : DomainBase<T>, IBusinessBase, IEntityBase where T : class,IEntityBase
    {

        [NonSerialized]
        protected IDbConnection _connection;

        protected int _changedRows = 0;//影响行
        protected string _updateParameters = "";// 用于拼接更新方法所需字段


        //方法委托，可以实现Insert，Update，Delete方法扩展
        //        protected InsertDelegate _insertDelegate;
        //        public delegate int InsertDelegate(IDbConnection conn, IDbTransaction transaction);
        //        protected UpdateDelegate _updateDelegate;
        //        public delegate int UpdateDelegate(IDbConnection conn, IDbTransaction transaction);
        //        protected DeleteDelegate _deleteDelegate;
        //        public delegate int DeleteDelegate(IDbConnection conn, IDbTransaction transaction);



        public override void MarkOld()
        {
            this._isNew = false;
            this._updateParameters = "";

            this.MarkClean();
        }

        public string TableName { get; protected set; }

        public string[] SkipedProperties { get; private set; }

        public IDbConnection Connection
        {
            get
            {
                return this._connection;
            }
            set
            {
                this._connection = value;

            }
        }

        /// <summary>
        /// true：只更新关系
        /// false：标记删除
        /// </summary>
        public bool IsChangeRelationship { get; set; }

        protected BaseEntity() : this(new Guid()) { }

        protected BaseEntity(Guid id)
            : base(id)
        {
            this.IsChangeRelationship = false;//默认进行标识删除


            //初始化方法注册
            //            _insertDelegate = PreInsert;
            //            _insertDelegate += Insert;
            //            _insertDelegate += PostInsert;
            //            _updateDelegate = PreUpdate;
            //            _updateDelegate += Update;
            //            _updateDelegate += PostUpdate;
            //            _deleteDelegate = PreDelete;
            //            _deleteDelegate += Delete;
            //            _deleteDelegate += PostDelete;
        }


        #region  数据库操作方法

        /// <summary>
        /// 插入或更新中过滤不需要的字段，
        /// properties：A,B,C
        /// </summary>
        /// <param name="properties"></param>
        public void SkipProperties(string properties)
        {
            var propertyList = properties.Split(',');
            this.SkipedProperties = propertyList;
            foreach (var property in propertyList)
            {
                this._updateParameters = this._updateParameters.Replace(string.Format(", {0} =  @{0}", property), "");
            }

        }

        public virtual int Create()
        {
            this._changedRows = 0;
            using (IDbConnection conn = this.Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    this.PreInsert(conn, transaction);
                    this.Validate();
                    this._changedRows += this.Insert(conn, transaction);
                    this.PostInsert(conn, transaction);
                    transaction.Commit();
                    this.MarkOld();
                }
                catch (Warning ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return this._changedRows;
        }

        public virtual int Save()
        {
            this._changedRows = 0;
            using (IDbConnection conn = this.Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    this.PreUpdate(conn, transaction);
                    if (this.IsDirty)
                    {
                        this.Validate();
                        this._changedRows += this.Update(conn, transaction);
                        this.PostUpdate(conn, transaction);
                        transaction.Commit();
                        this.MarkOld();
                    }
                }
                catch (Warning ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return this._changedRows;
        }

        public virtual int Delete()
        {
            using (IDbConnection conn = this.Connection)
            {
                this._changedRows = 0;
                var transaction = conn.BeginTransaction();
                try
                {
                    this.PreDelete(conn, transaction);
                    this._changedRows += this.Delete(conn, transaction);
                    this.PostDelete(conn, transaction);
                    transaction.Commit();
                    this.MarkDelete();
                }
                catch (Warning ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return this._changedRows;
        }

        /// <summary>
        /// 适用于子元素更新
        /// 和不需要知道元素状态的更新
        /// </summary>
        /// <returns></returns>
        public virtual int SubmitChange()
        {
            this._changedRows = 0;
            using (IDbConnection conn = this.Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    this._changedRows += this.SaveChange(conn, transaction);
                    transaction.Commit();
                }
                catch (Warning ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                return this._changedRows;
            }
        }

        /// <summary>
        /// 适用于子元素更新
        /// 和不需要知道元素状态的更新
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChange(IDbConnection conn, IDbTransaction transaction)
        {
            if (this.IsDelete && !this.IsChangeRelationship)
            {
                this.PreDelete(conn, transaction);
                this._changedRows += this.Delete(conn, transaction);
                this.PostDelete(conn, transaction);
                this.MarkOld();
                return this._changedRows;
            }
            else if (this.IsNew)
            {
                this.Validate();
                this.PreInsert(conn, transaction);
                this.Insert(conn, transaction);
                this.PostInsert(conn, transaction);
                this.MarkOld();
                return this._changedRows;
            }
            else if (this.IsDirty)
            {
                this.Validate();
                this.PreUpdate(conn, transaction);
                this._changedRows += this.Update(conn, transaction);
                this.PostUpdate(conn, transaction);
                this.MarkOld();
            }
            return this._changedRows;
        }

        public virtual int Update(IDbConnection conn, IDbTransaction transaction) { return 0; }

        public virtual int Insert(IDbConnection conn, IDbTransaction transaction) { return 0; }

        public virtual int Delete(IDbConnection conn, IDbTransaction transaction) { return 0; }

        protected virtual void PreFetch(IDbConnection conn) { }

        protected virtual void PreInsert(IDbConnection conn, IDbTransaction transaction) { }

        protected virtual void PreUpdate(IDbConnection conn, IDbTransaction transaction) { }

        protected virtual void PreDelete(IDbConnection conn, IDbTransaction transaction) { }

        protected virtual void PostFetch(IDbConnection conn) { }

        protected virtual void PostUpdate(IDbConnection conn, IDbTransaction transaction) { }

        protected virtual void PostDelete(IDbConnection conn, IDbTransaction transaction) { }

        protected virtual void PostInsert(IDbConnection conn, IDbTransaction transaction) { }

        #endregion


        protected override bool SetProperty<K>(string propertyName, ref K oldValue, K newValue)
        {
            var parameter = string.Format(", [{0}] =  @{0}", propertyName);
            if (!this._updateParameters.Contains(parameter))
                this._updateParameters += parameter;
            return base.SetProperty(propertyName, ref oldValue, newValue);
        }


    }
}