
namespace CAF.Tests.Samples
{
    using CAF.Validations;
    using System.ComponentModel.DataAnnotations;


    using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;


    public class Customer2 : BaseEntity<Customer2>,IEntityBase
    {
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }


        protected override void Validate(ValidationResultCollection results)
        {
            results.Add(new MaxLengthValidationRule(this.Name).Validate());
            base.Validate(results);
        }

        protected override void AddDescriptions() { this.AddDescription("Name:" + this.Name + ","); }
    }

    /// <summary>
    /// 最大长度验证规则
    /// </summary>
    public class MaxLengthValidationRule : IValidationRule
    {
        private string txt;
        /// <summary>
        /// 初始化客户英文名验证规则
        /// </summary>
        /// <param name="customer">客户</param>
        public MaxLengthValidationRule(string text) { this.txt = text; }

        /// <summary>
        /// 验证
        /// </summary>
        public ValidationResult Validate()
        {
            if (!string.IsNullOrWhiteSpace(this.txt) && this.txt.Length > 3)
                return new ValidationResult("姓名长度不能大于3");
            return ValidationResult.Success;
        }

    }
    /// <summary>
    /// 最小长度验证规则
    /// </summary>
    public class MinLengthValidationRule : IValidationRule
    {
        private string txt;
        /// <summary>
        /// 初始化客户英文名验证规则
        /// </summary>
        /// <param name="customer">客户</param>
        public MinLengthValidationRule(string text) { this.txt = text; }

        /// <summary>
        /// 验证
        /// </summary>
        public ValidationResult Validate()
        {
            if (this.txt.Length < 2)
                return new ValidationResult("姓名长度不能小于2");
            return ValidationResult.Success;
        }

    }
}
