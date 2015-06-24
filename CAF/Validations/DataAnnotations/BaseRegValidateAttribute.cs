using System;

namespace CAF.Utility.Validations.DataAnnotations
{
    using CAF.Validations.DataAnnotations;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public abstract class BaseRegValidateAttribute : ValidationAttribute
    {
        protected abstract string Pattern { get; }
        /// <summary>
        /// 格式化错误消息
        /// </summary>
        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
                this.ErrorMessage = ValidatorResources.InvalidMobilePhone;
            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString);
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.ToStr().IsEmpty())
                return null;

            if (Regex.IsMatch(value.ToStr(), this.Pattern))
                return null;
            return new ValidationResult(this.FormatErrorMessage(string.Empty));
        }


    }
}
