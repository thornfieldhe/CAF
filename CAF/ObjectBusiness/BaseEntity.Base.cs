using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CAF
{

    public partial class BaseEntity<T> : IEqualityComparer<T>, IBusinessBase, IComparable<IBusinessBase>,
        IBaseStatus where T : class,IBusinessBase
    {
        #region 属性验证
        [NonSerialized]
        protected bool _isValid;
        [NonSerialized]
        protected List<string> _errors;


        public List<string> Errors
        {
            get { return this._errors.SafeValue(); }
            protected set { this._errors = value; }
        }

        [NonSerialized]
        Validator<T> customerValidator = ValidationFactory.CreateValidator<T>();
        ValidationResults v;


        public virtual bool IsValid
        {
            get
            {

                var item = this as T;

                this.customerValidator = ValidationFactory.CreateValidator<T>();
                this.v = this.customerValidator.Validate(item);

                for (var i = 0; i < this.v.Count; i++)
                {
                    this.Errors.Add(this.v.ElementAt(i).Message);
                }
                this._isValid = this.v.IsValid;
                return this._isValid;
            }
            protected set { this._isValid = value; }
        }
        #endregion

        #region 基本状态
        protected bool _isNew = false;
        protected bool _isDirty = false;
        protected bool _isDelete = false;
        protected bool _isChild = false;
        protected bool _isRoot = false;

        public bool IsNew { get { return this._isNew; } set { this._isNew = value; } }

        public bool IsDelete { get { return this._isDelete; } set { this._isDelete = value; } }

        public bool IsDirty { get { return this._isDirty; } set { this._isDirty = value; } }

        public bool IsClean { get { return !this._isDirty && !this._isNew; } }


        public virtual void MarkNew()
        {
            this._isNew = true;
            this.MarkDirty();
        }

        public virtual void MarkChild()
        {
            this._isChild = true;
            this._isRoot = false;
        }

        public virtual void MarkRoot()
        {
            this._isChild = false;
            this._isRoot = true;
        }

        public virtual void MarkOld()
        {
            this._isNew = false;
            this._updateParameters = "";
            this.MarkClean();
        }

        public virtual void MarkClean()
        {
            this._isDirty = false;
        }

        public virtual void MarkDirty()
        {
            this._isDirty = true;
        }

        /// <summary>
        /// 适用于作为子对象进行标记删除
        /// </summary>
        public virtual void MarkDelete()
        {
            this._isDelete = true;
            this.MarkDirty();
            if (this.OnPropertyChange != null)
            {
                this.OnPropertyChange();
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
            return this.Id.ToString();
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
            var graph = this.SerializeObjectToString();
            return graph.DeserializeStringToObject<T>();
        }

        #endregion

        #region IComparable<IBusinessBase> 成员

        public int CompareTo(IBusinessBase other)
        {
            return this.Id.CompareTo(other.Id);
        }

        #endregion
    }
}