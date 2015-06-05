using System;
using System.Collections.Generic;

namespace CAF.Tests.Samples
{
    using CAF.Validations;
    using System.ComponentModel.DataAnnotations;

    public class Customer
    {
        /// <summary>
        /// 客户
        /// </summary>

        /// <summary>
        /// 初始化客户
        /// </summary>
        public Customer()
        {
            this._rules = new List<IValidationRule>();
            this._handler = new ValidationHandler();
        }

        /// <summary>
        /// 初始化客户
        /// </summary>
        /// <param name="customerId">客户编号</param>
        public Customer(int customerId)
        {
        }

        /// <summary>
        /// 验证规则集合
        /// </summary>
        private readonly List<IValidationRule> _rules;
        /// <summary>
        /// 验证处理器
        /// </summary>
        private IValidationHandler _handler;

        /// <summary>
        /// 验证
        /// </summary>
        protected void Validate(ValidationResultCollection results)
        {
            if (Equals(Name, default(string)))
                results.Add(new ValidationResult("Name不能为空"));
        }

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


        #region AddValidationRule(添加验证规则)

        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rule">验证规则</param>
        public void AddValidationRule(IValidationRule rule)
        {
            if (rule == null)
                return;
            this._rules.Add(rule);
        }

        #endregion


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
            var result = ValidationFactory.Instance.Validation.Validate(this);
            this.Validate(result);
            foreach (var rule in this._rules)
                result.Add(rule.Validate());
            return result;
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

        /// <summary>
        /// 员工编号,重点：必须是可空类型，否则不能使用HasOptional映射
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "客户名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        [Required(ErrorMessage = "英文名不能为空")]
        public string EnglishName { get; set; }


        /// <summary>
        /// 获取客户实例
        /// </summary>
        public static Customer GetCustomer()
        {
            return new Customer()
            {
                Name = "张三",
                EnglishName = "Zs"
            };
        }

        /// <summary>
        /// 获取客户A
        /// </summary>
        public static Customer GetCustomerA()
        {
            var customer = GetCustomer();
            customer.Name = "A";
            customer.EnglishName = "A";
            return customer;
        }

        /// <summary>
        /// 日期1
        /// </summary>
        public static DateTime Date1
        {
            get { return ("2000-6-1 10:10:10").ToDate(); }
        }

        /// <summary>
        /// 获取客户B
        /// </summary>
        public static Customer GetCustomerB()
        {
            var customer = GetCustomer();
            customer.Name = "B";
            customer.EnglishName = "B";
            return customer;
        }

        /// <summary>
        /// 日期2
        /// </summary>
        public static DateTime Date2
        {
            get { return ("2005-1-1 10:10:10").ToDate(); }
        }

        /// <summary>
        /// 获取客户C
        /// </summary>
        public static Customer GetCustomerC()
        {
            var customer = GetCustomer();
            customer.Name = "C";
            customer.EnglishName = "C";
            return customer;
        }

        /// <summary>
        /// 日期3
        /// </summary>
        public static DateTime Date3
        {
            get { return ("2010-3-1 10:10:10").ToDate(); }
        }

        /// <summary>
        /// 获取客户集合
        /// </summary>
        public static List<Customer> GetCustomers()
        {
            return new List<Customer>() {
                GetCustomerA(),
                GetCustomerB(),
                GetCustomerC()
            };
        }
    }

    public class Customer2 : BaseEntity<Customer2>
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
    /// 客户英文名验证规则
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
