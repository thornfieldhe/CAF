
namespace System.ComponentModel.DataAnnotations
{
    using CAF.Validations.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeRequiredAttribute : DataTypeAttribute
    {
        public DateTimeRequiredAttribute()
            : base(DataType.Date)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = ValidatorResources.DateAttribute_Invalid;
            }

            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            DateTime retDate;

            return DateTime.TryParse(Convert.ToString(value), out retDate);
        }
    }
}