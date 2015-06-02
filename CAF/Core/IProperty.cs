using System;

namespace CAF.Core
{
    public interface ITrackStatus
    {
        event EventHandler PropertyChanged;

        bool IsNew { get; }

        bool IsDirty { get; }
    }
}