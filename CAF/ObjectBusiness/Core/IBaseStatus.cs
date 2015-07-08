namespace CAF
{
    public interface IBaseStatus
    {
        bool IsClean { get; }
        bool IsDirty { get; }
        bool IsNew { get; }

        void MarkClean();
        void MarkDirty();
        void MarkNew();
    }

    public interface IEntityStatus : IBaseStatus
    {
        bool IsDelete { get; }
        void MarkDelete();
    }


}
