namespace CAF
{
    using System;
    using System.Collections.Generic;

    public interface IBusinessBase
    {
        string TableName { get; }

        string[] SkipedProperties { get; }
        Guid Id { get; set; }

        int Create();

        int Save();

        int Delete();

        int SubmitChange();

        List<string> Errors { get; }
    }
}