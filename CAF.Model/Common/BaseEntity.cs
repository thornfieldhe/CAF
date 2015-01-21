using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using CAF;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace SweetDessert.Model
{
    [DataContract]
    public class BaseEntity<T> : IEqualityComparer<T>, IBusinessBase where T : class,IBusinessBase
    {
        protected Guid _id;
        protected bool _isValid;
        protected List<string> _errors;

        public bool IsNew { get { return _isNew; } protected set { _isNew = value; } }

        public bool IsDelete { get { return _isDelete; } protected set { _isDelete = value; } }

        public bool IsDirty { get { return _isDirty; } protected set { _isDirty = value; } }

        #region 存储初始状态

        protected bool _isNew = false;
        protected bool _isDirty = false;
        protected bool _isDelete = false;

        #endregion 存储初始状态

        [DataMember]
        public Guid Id { get { return _id; } protected set { _id = value; } }

        public List<string> Errors { get { return _errors; } protected set { _errors = value; } }

        [DataMember]
        public virtual bool IsValid
        {
            get
            {
                try
                {
                    T item = this as T;
                    if (v == null)
                    {
                        customerValidator = ValidationFactory.CreateValidator<T>();
                        v = customerValidator.Validate(item);
                    }
                    Errors = new List<string>();
                    for (int i = 0; i < v.Count; i++)
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

        public delegate void PropertyChangedHandler(object sender, EventArgs e);
        protected event PropertyChangedHandler PropertyChanged;

        internal virtual void MarkNew()
        {
            _isNew = true;
            MarkDirty();
        }

        internal virtual void MarkOld()
        {
            _isNew = false;
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

        internal virtual void MarkDelete()
        {
            _isDelete = true;
            MarkDirty();
        }

        Validator<T> customerValidator = ValidationFactory.CreateValidator<T>();
        ValidationResults v;

        public bool Equals(T x, T y)
        {
            return x.ToString() == y.ToString();
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public virtual int Create()
        {
            customerValidator = ValidationFactory.CreateValidator<T>();
            if (IsValid)
            {
                using (IDbConnection conn = SqlService.OpenConnection)
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        PreInsert(conn, transaction);
                        int i = Insert(conn, transaction);
                        PostInsert(conn, transaction);
                        transaction.Commit();
                        return i;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            else
            {
                for (int i = 0; i < v.Count; i++)
                {
                    Errors.Add(v.ElementAt(i).Message);
                }
                return 0;
            }
        }

        public virtual int Save()
        {
            if (IsValid)
            {
                using (IDbConnection conn = SqlService.OpenConnection)
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        PreUpdate(conn, transaction);
                        int i = Update(conn, transaction);
                        PostUpdate(conn, transaction);
                        transaction.Commit();
                        return i;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        public virtual int Delete()
        {
            using (IDbConnection conn = SqlService.OpenConnection)
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    PreDelete(conn, transaction);
                    int i = Delete(conn, transaction);
                    PostDelete(conn, transaction);
                    transaction.Commit();
                    return i;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        internal virtual int Update(IDbConnection conn, IDbTransaction transaction) { return 0; }

        internal virtual int Insert(IDbConnection conn, IDbTransaction transaction) { return 0; }

        internal virtual int Delete(IDbConnection conn, IDbTransaction transaction) { return 0; }

        internal virtual void PreFetch(IDbConnection conn) { }

        internal virtual void PreInsert(IDbConnection conn, IDbTransaction transaction) { }

        internal virtual void PreUpdate(IDbConnection conn, IDbTransaction transaction) { }

        internal virtual void PreDelete(IDbConnection conn, IDbTransaction transaction) { }

        internal virtual void PostFetch(IDbConnection conn) { }

        internal virtual void PostUpdate(IDbConnection conn, IDbTransaction transaction) { }

        internal virtual void PostDelete(IDbConnection conn, IDbTransaction transaction) { }

        internal virtual void PostInsert(IDbConnection conn, IDbTransaction transaction) { }

        public void OnPropertyChanged(EventArgs e)
        {
            if (PropertyChanged != null)
            {
                MarkDirty();
            }
        }
    }
}