using System;
using System.Linq;

namespace CAF
{
    using CAF.Validations;
    using System.Collections.Generic;

    using CAF.ObjectBusiness;

    /// <summary>
    /// 值对象
    /// </summary>
    /// <typeparam name="TValueObject">值对象类型</typeparam>
    public abstract class ValueObjectBase<TValueObject> :StatusDescription, IEquatable<TValueObject> where TValueObject : ValueObjectBase<TValueObject>
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

        #region 相等

        #region Equals(相等性比较)

        /// <summary>
        /// 相等性比较
        /// </summary>
        public bool Equals(TValueObject other)
        {
            return this == other;
        }

        /// <summary>
        /// 相等性比较
        /// </summary>
        public override bool Equals(object other)
        {
            return Equals(other as TValueObject);
        }

        #endregion

        #region ==(相等性比较)

        /// <summary>
        /// 相等性比较
        /// </summary>
        public static bool operator ==(
            ValueObjectBase<TValueObject> valueObject1, ValueObjectBase<TValueObject> valueObject2)
        {
            if ((object)valueObject1 == null && (object)valueObject2 == null)
                return true;
            if ((object)valueObject1 == null || (object)valueObject2 == null)
                return false;
            if (valueObject1.GetType() != valueObject2.GetType())
                return false;
            var properties = valueObject1.GetType().GetProperties();
            return properties.All(property => property.GetValue(valueObject1) == property.GetValue(valueObject2));
        }

        #endregion

        #region !=(不相等比较)

        /// <summary>
        /// 不相等比较
        /// </summary>
        public static bool operator !=(
            ValueObjectBase<TValueObject> valueObject1, ValueObjectBase<TValueObject> valueObject2)
        {
            return !(valueObject1 == valueObject2);
        }

        #endregion

        #region GetHashCode(获取哈希)

        /// <summary>
        /// 获取哈希
        /// </summary>
        public override int GetHashCode()
        {
            var properties = GetType().GetProperties();
            return properties.Select(property => property.GetValue(this))
                .Where(value => value != null)
                .Aggregate(0, (current, value) => current ^ value.GetHashCode());
        }

        #endregion

        #endregion

        #region Clone(克隆副本)

        /// <summary>
        /// 克隆副本
        /// </summary>
        public virtual TValueObject Clone()
        {
            return this.MemberwiseClone().To<TValueObject>();
        }

        #endregion
    }
}
