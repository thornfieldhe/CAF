namespace CAF
{

    public delegate void PropertyChangeHandler();


    public interface IBusinessBase
    {

        event PropertyChangeHandler OnPropertyChanged;
        bool IsChangeRelationship { get; set; }
    }
}