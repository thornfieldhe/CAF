namespace CAF
{
    public interface IBaseStatus
    {
        bool IsClean { get; }
        bool IsDirty { get; set; }
        bool IsValid { get; }
        void MarkClean();
        void MarkDirty();
        void MarkNew();
        void MarkOld();
    }
}
