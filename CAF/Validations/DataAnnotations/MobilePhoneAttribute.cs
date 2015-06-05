using System.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    using CAF;
    using CAF.Validations.DataAnnotations;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 手机号验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MobilePhoneAttribute : ValidationAttribute
    {
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
            const string pattern = "^1[3|4|5|7|8|][0-9]{9}$";
            if (Regex.IsMatch(value.ToStr(), pattern))
                return null;
            return new ValidationResult(this.FormatErrorMessage(string.Empty));
        }
    }
}
