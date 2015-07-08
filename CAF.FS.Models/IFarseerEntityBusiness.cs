namespace CAF.FSModels
{
    internal interface IFarseerEntityBusiness
    {
        void Validate();

        int SubmitChange(Context contex);
    }
}