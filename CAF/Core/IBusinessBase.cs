namespace CAF
{
    using System;
    using System.Collections.Generic;

    public interface IBusinessBase : IEntityBase, ITableName
    {

        string[] SkipedProperties { get; }
        Guid Id { get; set; }

        int Create();

        int Save();

        int Delete();

        int SubmitChange();

    }
}