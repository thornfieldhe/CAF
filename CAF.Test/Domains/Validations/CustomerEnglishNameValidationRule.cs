using System.ComponentModel.DataAnnotations;


namespace CAF.Domains.Tests.Validations {
    using CAF.Tests.Samples;
    using CAF.Validations;

    /// <summary>
    /// 客户英文名验证规则
    /// </summary>
    public class CustomerEnglishNameValidationRule : IValidationRule {
        /// <summary>
        /// 初始化客户英文名验证规则
        /// </summary>
        /// <param name="customer">客户</param>
        public CustomerEnglishNameValidationRule( Customer customer ) {
            _customer = customer;
        }

        /// <summary>
        /// 客户
        /// </summary>
        private readonly Customer _customer;

        /// <summary>
        /// 验证
        /// </summary>
        public ValidationResult Validate() {
            if( _customer.EnglishName.Length > 1 )
                return new ValidationResult( "客户英文名长度不能大于1" );
            return ValidationResult.Success;
        }
    }
}
