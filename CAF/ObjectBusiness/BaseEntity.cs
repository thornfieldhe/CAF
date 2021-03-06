﻿using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CAF
{

    [Serializable]
    public partial class BaseEntity<T>
    {
        protected Guid _id;
        protected int _status;
        protected DateTime _createdDate;
        protected DateTime _changedDate;
        protected string _note;
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

        //属性改变事件，用于通知列表，修改状态为Dity
        public delegate void PropertyChangeHandler();
        public event PropertyChangeHandler OnPropertyChange;

        public Guid Id { get { return this._id; } set { this.SetProperty("Id", ref this._id, value); } }
        public int Status { get { return this._status; } set { this.SetProperty("Status", ref this._status, value); } }
        public DateTime CreatedDate { get { return this._createdDate; } protected set { this.SetProperty("CreatedDate", ref this._createdDate, value); } }
        public DateTime ChangedDate { get { return this._changedDate; } protected set { this.SetProperty("ChangedDate", ref this._changedDate, value); } }
        public string Note { get { return this._note; } set { this.SetProperty("Note", ref this._note, value); } }

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

        public BaseEntity(Guid id)
        {
            this._id = id;
            this._status = 1;
            this._createdDate = DateTime.Now;
            this._changedDate = DateTime.Now;

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

        public BaseEntity() : this(Guid.NewGuid()) { }


        protected bool SetProperty<K>(string propertyName, ref K oldValue, K newValue)
        {
            if ((oldValue == null && newValue == null)
                || (oldValue != null && oldValue.Equals(newValue)))
            { return false; }
            this.MarkDirty();
            var parameter = string.Format(", [{0}] =  @{0}", propertyName);
            if (!this._updateParameters.Contains(parameter))
                this._updateParameters += parameter;
            oldValue = newValue;
            if (this.OnPropertyChange != null)
            {
                this.OnPropertyChange();
            }
            return true;
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
            this.customerValidator = ValidationFactory.CreateValidator<T>();
            this._changedRows = 0;
            using (IDbConnection conn = this.Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    this.PreInsert(conn, transaction);
                    if (this.IsValid)
                    {
                        this._changedRows += this.Insert(conn, transaction);
                        this.PostInsert(conn, transaction);
                        transaction.Commit();
                        this.MarkOld();
                    }
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
                    if (this.IsDirty && this.IsValid)
                    {
                        this._changedRows += this.Update(conn, transaction);
                        this.PostUpdate(conn, transaction);
                        transaction.Commit();
                        this.MarkOld();
                    }
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
                this.PreInsert(conn, transaction);
                if (!this.IsValid)
                {
                    return this._changedRows;
                }
                this.Insert(conn, transaction);
                this.PostInsert(conn, transaction);
                this.MarkOld();
                return this._changedRows;
            }
            else if (this.IsDirty)
            {
                this.PreUpdate(conn, transaction);
                if (!this.IsValid)
                {
                    return this._changedRows;
                }
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

 
    }
}