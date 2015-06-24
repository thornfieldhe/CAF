
namespace System.ComponentModel.DataAnnotations
{
    using CAF.Utility;
    using CAF.Utility.Validations.DataAnnotations;

    /// <summary>
    /// 手机号验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MobilePhoneAttribute : BaseRegValidateAttribute
    {
        protected override string Pattern
        {
            get { return StringRegExpression.Mobile; }
        }
    }

    /// <summary>
    /// 手机号验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : BaseRegValidateAttribute
    {
        protected override string Pattern
        {
            get { return StringRegExpression.Email; }
        }
    }

    /// <summary>
    /// 整数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InteAttribute : BaseRegValidateAttribute
    {
        protected override string Pattern
        {
            get { return StringRegExpression.Integer; }
        }
    }

    /// <summary>
    /// 数字和字母
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LetterAndNumbersAttribute : BaseRegValidateAttribute
    {
        protected override string Pattern
        {
            get { return StringRegExpression.LetterAndNumbers; }
        }
    }
}
