namespace CAF
{
    using System;
    using System.Collections.Generic;

    public interface IBusinessBase
    {
        string TableName { get; }
        Guid Id { get; set; }

        int Create();

        int Save();

        int Delete();

        int SubmitChange();

        List<string> Errors { get; }
    }
}