using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Utility
{
    using CAF.Validations;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationFactory = Microsoft.Practices.EnterpriseLibrary.Validation.ValidationFactory;
    using ValidationResult = Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult;

    [TestClass]
    public class ValidateionTest
    {
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    public class A
    {
        [Required]
        public string Name { get; set; }


    }
    public class ELValidation : IValidation
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
