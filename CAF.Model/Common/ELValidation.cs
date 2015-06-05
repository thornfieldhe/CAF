using System.Collections.Generic;

namespace CAF.Model
{
    using CAF.Validations;

    using Microsoft.Practices.EnterpriseLibrary.Validation;

    internal class ELValidation : IValidation
    {
        public ELValidation() { }
        public ValidationResultCollection Validate(object target)
        {
            var validator = ValidationFactory.CreateValidator(target.GetType());
            var results = validator.Validate(target);
            return this.GetResult(results);

        }
        /// <summary>
        /// 获取验证结果
        /// </summary>
        private ValidationResultCollection GetResult(IEnumerable<ValidationResult> results)
        {
            var result = new ValidationResultCollection();
            results.ForEach(r => result.Add(new System.ComponentModel.DataAnnotations.ValidationResult(r.Message)));

            return result;
        }
    }
}
