using System.ComponentModel.DataAnnotations;

namespace CAF.Validations
{
    /// <summary>
    /// 空白验证规则，只需要传入需要返回的错误信息
    /// </summary>
    public class EmptyErrorValidateionRule : IValidationRule
    {
        private readonly string errorMessage;
        public EmptyErrorValidateionRule(string errorMessage) { this.errorMessage = errorMessage; }
        public ValidationResult Validate()
        {
            return new ValidationResult(this.errorMessage);

        }
    }
}
