
namespace System.ComponentModel.DataAnnotations
{
    using CAF;
    using CAF.Validations.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GuidRequiredAttribute : DataTypeAttribute
    {
        public GuidRequiredAttribute()
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
            return value == null || Convert.ToString(value).ToGuid().IsEmpty();
        }
    }
}