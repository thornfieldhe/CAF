namespace CAF.ObjectBusiness
{
    public interface IValidationEntity
    {
        void AddValidationRule(CAF.Validations.IValidationRule rule);
        void Validate();

        bool IsValidated();
    }
}
