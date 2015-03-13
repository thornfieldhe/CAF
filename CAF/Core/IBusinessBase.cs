namespace CAF
{
    using System;
    using System.Collections.Generic;

    public interface IBusinessBase
    {
        Guid Id { get; set; }

        int Create();

        int Save();

        int Delete();

        int SubmitChange();

        List<string> Errors { get; }
    }
}