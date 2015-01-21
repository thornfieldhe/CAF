using System;
using System.ComponentModel.DataAnnotations;

namespace CAF.Validation
{
    /// <summary>
    /// GUID不能为空
    /// </summary>
    public class GuidRequired : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (Guid)value != new Guid();
        }
    }

    /// <summary>
    /// decimal不能大于
    /// </summary>
    public class MaxDecimalAttribute : ValidationAttribute
    {
        private decimal Max { get; set; }

        public MaxDecimalAttribute(double min, string ErrorMessage = "数值不得大于0")
        {
            this.Max = Convert.ToDecimal(min);
            this.ErrorMessage = ErrorMessage;
        }

        public override bool IsValid(object value)
        {
            return (decimal)value < Max;
        }
    }

    /// <summary>
    /// decimal不能小于
    /// </summary>
    public class MinDecimalAttribute : ValidationAttribute
    {
        private decimal Min { get; set; }

        public MinDecimalAttribute(double min, string ErrorMessage = "数值不得大于0")
        {
            this.Min = Convert.ToDecimal(min);
            this.ErrorMessage = ErrorMessage;
        }

        public override bool IsValid(object value)
        {
            return (decimal)value > Min;
        }
    }

    /// <summary>
    /// 日期不能为空
    /// </summary>
    public class DateTimeRequired : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (DateTime)value >new DateTime(1753, 1, 1, 12, 0, 0);
        }
    }
}