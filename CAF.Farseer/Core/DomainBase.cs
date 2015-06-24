﻿using System;
using System.Collections.Generic;

namespace CAF.Farseer
{
    using CAF.Logs;
    using CAF.ObjectBusiness;
    using CAF.Validations;
    using FS.Mapping.Context.Attribute;

    public abstract class DomainBase<T> : StatusDescription, IEqualityComparer<T>, IComparable<IEntityBase>,
       IBaseStatus, IValidationEntity where T : class,IEntityBase
    {
        #region 初始化

        protected DomainBase() : this(Guid.NewGuid()) { }

        protected DomainBase(Guid id)
        {
            this._id = id;
            this._status = 1;
            this._createdDate = DateTime.Now;
            this._changedDate = DateTime.Now;

            this._rules = new List<IValidationRule>();
            this._handler = TypeCreater.IocBuildUp<IValidationHandler>();
            this.Init();
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        protected virtual void Init() { }

        #endregion

        #region 基本属性

        protected Guid _id;
        protected int _status;
        protected DateTime _createdDate;
        protected DateTime _changedDate;
        protected string _note;
        protected byte[] _version;


        //属性改变事件，用于通知列表，修改状态为Dity
        public delegate void PropertyChangeHandler();
        public event PropertyChangeHandler OnPropertyChanged;

        [Field(UpdateStatusType = StatusType.ReadCondition)]
        public Guid Id { get { return this._id; } set { this.SetProperty("Id", ref this._id, value); } }
        public int Status { get { return this._status; } set { this.SetProperty("Status", ref this._status, value); } }
        public DateTime CreatedDate { get { return this._createdDate; } protected set { this.SetProperty("CreatedDate", ref this._createdDate, value); } }
        public DateTime ChangedDate { get { return this._changedDate; } protected set { this.SetProperty("ChangedDate", ref this._changedDate, value); } }
        public string Note { get { return this._note; } set { this.SetProperty("Note", ref this._note, value); } }
        /// <summary>
        /// 版本号(乐观锁)
        /// </summary>
        [Field(UpdateStatusType = StatusType.ReadCondition)]
        public byte[] Version { get { return this._version; } set { this.SetProperty("Version", ref this._version, value); } }



        protected bool SetProperty<K>(string propertyName, ref K oldValue, K newValue)
        {
            if ((oldValue == null && newValue == null)
                || (oldValue != null && oldValue.Equals(newValue)))
            { return false; }
            this.MarkDirty();
            oldValue = newValue;
            if (this.OnPropertyChanged != null)
            {
                this.OnPropertyChanged();
            }
            return true;
        }

        #endregion

        #region Log(日志操作)

        /// <summary>
        /// 日志操作
        /// </summary>
        protected ILog Log { get; set; }
        #endregion

        #region 属性验证

        #region 字段

        /// <summary>
        /// 验证规则集合
        /// </summary>
        private readonly List<IValidationRule> _rules;
        /// <summary>
        /// 验证处理器
        /// </summary>
        private IValidationHandler _handler;

        #endregion

        #region SetValidationHandler(设置验证处理器)

        /// <summary>
        /// 设置验证处理器
        /// </summary>
        /// <param name="handler">验证处理器</param>
        public void SetValidationHandler(IValidationHandler handler)
        {
            if (handler == null)
                return;
            this._handler = handler;
        }

        #endregion

        #region AddValidationRule(添加验证规则)

        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rule">验证规则</param>
        public virtual void AddValidationRule(IValidationRule rule)
        {
            if (rule == null)
                return;
            this._rules.Add(rule);
        }

        #endregion

        #region Validate(验证)

        /// <summary>
        /// 验证
        /// </summary>
        public virtual void Validate()
        {
            var result = this.GetValidationResult();
            this.HandleValidationResult(result);

        }

        public bool IsValidated()
        {
            return this.GetValidationResult().IsValid;
        }

        /// <summary>
        /// 获取验证结果
        /// </summary>
        private ValidationResultCollection GetValidationResult()
        {
            var result = TypeCreater.IocBuildUp<IValidation>().Validate(this);
            this.Validate(result);
            foreach (var rule in this._rules)
                result.Add(rule.Validate());
            return result;
        }

        /// <summary>
        /// 验证并添加到验证结果集合
        /// </summary>
        /// <param name="results">验证结果集合</param>
        protected virtual void Validate(ValidationResultCollection results)
        {
        }

        /// <summary>
        /// 处理验证结果
        /// </summary>
        private void HandleValidationResult(ValidationResultCollection results)
        {
            if (results.IsValid)
                return;
            this._handler.Handle(results);
        }

        #endregion


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
            if (this.OnPropertyChanged != null)
            {
                this.OnPropertyChanged();
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

        public static bool operator ==(DomainBase<T> lhs, DomainBase<T> rhs)
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

        public static bool operator !=(DomainBase<T> lhs, DomainBase<T> rhs)
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

        #region IComparable<IEntityBase> 成员

        public int CompareTo(IEntityBase other)
        {
            return this.Id.CompareTo(other.Id);
        }

        #endregion

    }
}