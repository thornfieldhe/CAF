using System.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    using CAF.Validations.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxAttribute : DataTypeAttribute
    {
        public object Max { get { return this._max; } }

        private readonly double _max;

        public MaxAttribute(int max)
            : base("max")
        {
            this._max = max;
        }

        public MaxAttribute(double max)
            : base("max")
        {
            this._max = max;
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = ValidatorResources.MaxAttribute_Invalid;
            }

            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this._max);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            double valueAsDouble;

            var isDouble = double.TryParse(Convert.ToString(value), out valueAsDouble);

            return isDouble && valueAsDouble <= this._max;
        }
    }
}