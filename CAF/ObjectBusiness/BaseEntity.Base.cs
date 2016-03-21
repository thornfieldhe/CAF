using System;
using System.Collections.Generic;

namespace CAF
{
    using CAF.ObjectBusiness;
    using CAF.Validations;


    public abstract partial class BaseBusiness<T> : StatusDescription, IEqualityComparer<T>, IBusinessBase, IComparable<IBusinessBase>,
         IValidationEntity where T : class,IBusinessBase
    {
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

        public static bool operator ==(BaseBusiness<T> lhs, BaseBusiness<T> rhs)
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

        public static bool operator !=(BaseBusiness<T> lhs, BaseBusiness<T> rhs)
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