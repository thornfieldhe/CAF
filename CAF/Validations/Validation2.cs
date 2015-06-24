using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CAF.Validations
{
    using CAF.Utility;

    /// <summary>
    /// 验证操作
    /// </summary>
    public class Validation2 : IValidation
    {
        /// <summary>
        /// 初始化验证操作
        /// </summary>
        public Validation2()
        {
            this._result = new ValidationResultCollection();
        }

        /// <summary>
        /// 验证目标
        /// </summary>
        private object _target;
        /// <summary>
        /// 结果
        /// </summary>
        private readonly ValidationResultCollection _result;

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="target">验证目标</param>
        public ValidationResultCollection Validate(object target)
        {
            target.CheckNull("target");
            this._target = target;
            var type = target.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
                this.ValidateProperty(property);
            return this._result;
        }

        public bool IsValidate(object target)
        {
            var isValidated = true;
            target.CheckNull("target");
            this._target = target;
            var type = target.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                this.ValidateProperty(property).IfFalse(() => isValidated = false);
            }
            return isValidated;
        }

        /// <summary>
        /// 验证属性
        /// </summary>
        private bool ValidateProperty(PropertyInfo property)
        {
            var isValidated = true;
            var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
            foreach (var attribute in attributes)
            {
                var validationAttribute = attribute as ValidationAttribute;
                if (validationAttribute == null)
                    continue;
                this.ValidateAttribute(property, validationAttribute).IfFalse(() => isValidated = false);
            }
            return isValidated;
        }

        /// <summary>
        /// 验证特性
        /// </summary>
        private bool ValidateAttribute(PropertyInfo property, ValidationAttribute attribute)
        {
            var isValid = attribute.IsValid(property.GetValue(this._target));
            if (isValid)
                return true;
            this._result.Add(new ValidationResult(this.GetErrorMessage(attribute)));
            return false;
        }

        /// <summary>
        /// 获取错误消息
        /// </summary>
        private string GetErrorMessage(ValidationAttribute attribute)
        {
            if (!string.IsNullOrEmpty(attribute.ErrorMessage))
                return attribute.ErrorMessage;
            return ResourceHelper.GetString(attribute.ErrorMessageResourceType.FullName, attribute.ErrorMessageResourceName, attribute.ErrorMessageResourceType.Assembly);
        }
    }
}
