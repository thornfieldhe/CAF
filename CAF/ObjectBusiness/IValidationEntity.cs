using System;
namespace CAF.ObjectBusiness
{
    interface IValidationEntity
    {
        void AddValidationRule(CAF.Validations.IValidationRule rule);
        void Validate();
    }
}
