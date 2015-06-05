using System.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    using CAF.Utility.Validations.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinAttribute : DataTypeAttribute
    {
        public object Min { get { return this._min; } }

        private readonly double _min;

        public MinAttribute(int min)
            : base("min")
        {
            this._min = min;
        }

        public MinAttribute(double min)
            : base("min")
        {
            this._min = min;
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = ValidatorResources.MinAttribute_Invalid;
            }

            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this._min);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            double valueAsDouble;

            var isDouble = double.TryParse(Convert.ToString(value), out valueAsDouble);

            return isDouble && valueAsDouble >= this._min;
        }
    }
}
