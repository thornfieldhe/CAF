namespace CAF
{
    using System;

    public delegate void PropertyChangeHandler();


    public interface IBusinessBase
    {

        event PropertyChangeHandler OnPropertyChanged;
        bool IsChangeRelationship { get; set; }
    }

    /// <summary>
    /// 确保是业务类
    /// </summary>
    public interface IEntityBase
    {
        Guid Id { get; }
    }
}