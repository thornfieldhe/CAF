namespace CAF
{
    using System;
    using System.Collections.Generic;

    public interface IBusinessBase :  ITableName
    {

        string[] SkipedProperties { get; }
     

        int Create();

        int Save();

        int Delete();

        int SubmitChange();

    }
}