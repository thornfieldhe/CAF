using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CAF
{
    using CAF.Utility;

    [Serializable]
    public class BaseEntity<T> : IEqualityComparer<T>, IBusinessBase where T : class,IBusinessBase
    {
        protected Guid _id;
        private int _status;
        private DateTime _createdDate;
        private DateTime _changedDate;
        private string _note;
        [NonSerialized]
        private IDbConnection _connection;

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
        internal delegate void PropertyChangeHandler();
        internal event PropertyChangeHandler OnPropertyChange;

        public Guid Id { get { return _id; } set { SetProperty("Id", ref _id, value); } }
        public int Status { get { return _status; } set { SetProperty("Status", ref _status, value); } }
        public DateTime CreatedDate { get { return _createdDate; } protected set { SetProperty("CreatedDate", ref _createdDate, value); } }
        public DateTime ChangedDate { get { return _changedDate; } protected set { SetProperty("ChangedDate", ref _changedDate, value); } }
        public string Note { get { return _note; } set { SetProperty("Note", ref _note, value); } }


        public IDbConnection Connection { get { return _connection; } set { _connection = value; } }

        public BaseEntity(Guid id)
        {
            _id = id;
            _status = 1;
            _createdDate = DateTime.Now;
            _changedDate = DateTime.Now;

            IsChangeRelationship = false;//默认进行标识删除

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

        internal BaseEntity() : this(Guid.NewGuid()) { }


        protected bool SetProperty<K>(string propertyName, ref K oldValue, K newValue)
        {
            if ((oldValue == null && newValue == null)
                || (oldValue != null && oldValue.Equals(newValue)))
            { return false; }
            MarkDirty();
            var parameter = string.Format(", {0} =  @{0}", propertyName);
            if (!_updateParameters.Contains(parameter)) _updateParameters += parameter;
            oldValue = newValue;
            if (OnPropertyChange != null)
            {
                OnPropertyChange();
            }
            return true;
        }

        #region 属性验证
        [NonSerialized]
        protected bool _isValid;
        [NonSerialized]
        protected List<string> _errors;


        public List<string> Errors { get { return _errors; } protected set { _errors = value; } }

        [NonSerialized]
        Validator<T> customerValidator = ValidationFactory.CreateValidator<T>();
        ValidationResults v;


        public virtual bool IsValid
        {
            get
            {
                try
                {
                    var item = this as T;

                    customerValidator = ValidationFactory.CreateValidator<T>();
                    v = customerValidator.Validate(item);

                    (Errors == null).IfIsTrue(() => Errors = new List<string>());
                    for (var i = 0; i < v.Count; i++)
                    {
                        Errors.Add(v.ElementAt(i).Message);
                    }
                    _isValid = v.IsValid;
                    return _isValid;
                }
                catch (Exception rc)
                {
                    throw rc;
                }
            }
            protected set { _isValid = value; }
        }
        #endregion

        #region 基本状态
        internal bool _isNew = false;
        internal bool _isDirty = false;
        internal bool _isDelete = false;
        internal bool _isChild = false;
        internal bool _isRoot = false;

        internal bool IsNew { get { return _isNew; } set { _isNew = value; } }

        internal bool IsDelete { get { return _isDelete; } set { _isDelete = value; } }

        internal bool IsDirty { get { return _isDirty; } set { _isDirty = value; } }

        internal bool IsClean { get { return !_isDirty && !_isNew; } }

        /// <summary>
        /// true：只更新关系
        /// false：标记删除
        /// </summary>
        internal bool IsChangeRelationship { get; set; }
        internal virtual void MarkNew()
        {
            _isNew = true;
            MarkDirty();
        }

        internal virtual void MarkChild()
        {
            _isChild = true;
        }

        internal virtual void MarkRoot()
        {
            _isChild = true;
        }

        internal virtual void MarkOld()
        {
            _isNew = false;
            _updateParameters = "";
            MarkClean();
        }

        internal virtual void MarkClean()
        {
            _isDirty = false;
        }

        internal virtual void MarkDirty()
        {
            _isDirty = true;
        }

        /// <summary>
        /// 适用于作为子对象进行标记删除
        /// </summary>
        public virtual void MarkDelete()
        {
            _isDelete = true;
            MarkDirty();
            if (OnPropertyChange != null)
            {
                OnPropertyChange();
            }
        }

        #endregion

        #region 重载相等判断
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public static bool operator ==(BaseEntity<T> lhs, BaseEntity<T> rhs)
        {
            if ((lhs as object) != null && (rhs as object) != null)
            {
                return lhs.Id == rhs.Id;
            }
            if ((lhs as object) == null && (rhs as object) == null)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(BaseEntity<T> lhs, BaseEntity<T> rhs)
        {
            if ((lhs as object) != null && (rhs as object) != null)
            {
                return !(lhs.Id == rhs.Id);
            }
            if ((lhs as object) == null && (rhs as object) == null)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region  数据库操作方法

        public virtual int Create()
        {
            customerValidator = ValidationFactory.CreateValidator<T>();
            _changedRows = 0;
            using (IDbConnection conn = Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    PreInsert(conn, transaction);
                    if (IsValid)
                    {
                        _changedRows += Insert(conn, transaction);
                        PostInsert(conn, transaction);
                        transaction.Commit();
                        MarkOld();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return _changedRows;
        }

        public virtual int Save()
        {
            this._changedRows = 0;
            using (IDbConnection conn = Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    PreUpdate(conn, transaction);
                    if (IsDirty && IsValid)
                    {
                        _changedRows += Update(conn, transaction);
                        PostUpdate(conn, transaction);
                        transaction.Commit();
                        MarkOld();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return _changedRows;
        }

        public virtual int Delete()
        {
            using (IDbConnection conn = Connection)
            {
                this._changedRows = 0;
                var transaction = conn.BeginTransaction();
                try
                {
                    PreDelete(conn, transaction);
                    _changedRows += Delete(conn, transaction);
                    PostDelete(conn, transaction);
                    transaction.Commit();
                    MarkDelete();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return _changedRows;
        }

        /// <summary>
        /// 适用于子元素更新
        /// 和不需要知道元素状态的更新
        /// </summary>
        /// <returns></returns>
        public virtual int SubmitChange()
        {
            _changedRows = 0;
            using (IDbConnection conn = Connection)
            {
                var transaction = conn.BeginTransaction();
                try
                {
                    _changedRows += SaveChange(conn, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                return _changedRows;
            }
        }

        /// <summary>
        /// 适用于子元素更新
        /// 和不需要知道元素状态的更新
        /// </summary>
        /// <returns></returns>
        internal virtual int SaveChange(IDbConnection conn, IDbTransaction transaction)
        {
            if (this.IsDelete && !IsChangeRelationship)
            {
                PreDelete(conn, transaction);
                _changedRows += Delete(conn, transaction);
                PostDelete(conn, transaction);
                MarkOld();
                return _changedRows;
            }
            else if (this.IsNew)
            {
                PreInsert(conn, transaction);
                if (!IsValid)
                {
                    return _changedRows;
                }
                Insert(conn, transaction);
                PostInsert(conn, transaction);
                MarkOld();
                return _changedRows;
            }
            else if (this.IsDirty)
            {
                PreUpdate(conn, transaction);
                if (!IsValid)
                {
                    return _changedRows;
                }
                _changedRows += Update(conn, transaction);
                PostUpdate(conn, transaction);
                MarkOld();
            }
            return _changedRows;
        }

        internal virtual int Update(IDbConnection conn, IDbTransaction transaction) { return 0; }

        internal virtual int Insert(IDbConnection conn, IDbTransaction transaction) { return 0; }

        internal virtual int Delete(IDbConnection conn, IDbTransaction transaction) { return 0; }

        protected virtual void PreFetch(IDbConnection conn) {  }

        protected virtual void PreInsert(IDbConnection conn, IDbTransaction transaction) {  }

        protected virtual void PreUpdate(IDbConnection conn, IDbTransaction transaction) {  }

        protected virtual void PreDelete(IDbConnection conn, IDbTransaction transaction) { }

        protected virtual void PostFetch(IDbConnection conn) {  }

        protected virtual void PostUpdate(IDbConnection conn, IDbTransaction transaction) {  }

        protected virtual void PostDelete(IDbConnection conn, IDbTransaction transaction) { }

        protected virtual void PostInsert(IDbConnection conn, IDbTransaction transaction) {  }

        #endregion

        #region 克隆操作

        /// <summary>
        /// 创建浅表副本
        /// </summary>
        /// <returns></returns>
        public T GetShallowCopy()
        {
            return (T)this.MemberwiseClone();
        }

        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <returns></returns>
        public T Clone()
        {
            var graph = SerializationHelper.SerializeObjectToString(this);
            return SerializationHelper.DeserializeStringToObject<T>(graph);
        }

        #endregion

    }
}