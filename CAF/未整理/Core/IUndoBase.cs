using System;

namespace CAF
{
    public interface IUndoBase
    {
        void Redo();

        int Save();

        void Submit();

        void Undo();

        event EventHandler PropertyChanged;
    }
}