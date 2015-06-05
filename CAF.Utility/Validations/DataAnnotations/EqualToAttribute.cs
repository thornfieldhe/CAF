using System.Globalization;
using System.Linq;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{
    using CAF.Utility.Validations.DataAnnotations;

    /// <summary>
    /// Validates that the property has the same value as the given 'otherProperty' 
    /// </summary>
    /// <remarks>
    /// From Mvc3 Futures
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EqualToAttribute : ValidationAttribute
    {
        public EqualToAttribute(string otherProperty)
        {
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }
            this.OtherProperty = otherProperty;
            this.OtherPropertyDisplayName = null;
        }

        public string OtherProperty { get; private set; }

        public string OtherPropertyDisplayName { get; set; }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = ValidatorResources.CompareAttribute_MustMatch;
            }

            var otherPropertyDisplayName = this.OtherPropertyDisplayName ?? this.OtherProperty;

            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, otherPropertyDisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var memberNames = new[] { validationContext.MemberName };

            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, ValidatorResources.EqualTo_UnknownProperty, this.OtherProperty), memberNames);
            }

            var displayAttribute =
                otherPropertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

            if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
            {
                this.OtherPropertyDisplayName = displayAttribute.Name;
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (!Equals(value, otherPropertyValue))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
            }
            return null;
        }
    }
}