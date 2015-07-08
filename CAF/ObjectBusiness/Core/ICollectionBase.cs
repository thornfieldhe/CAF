namespace CAF
{
    using FS.Core.Infrastructure;

    public interface ICollectionBase<in TCollection, TMember>
        where TCollection : CollectionBase<TCollection, TMember>
        where TMember : class, IBusinessBase, IEntityBase, IEntityStatus
    {
        void Add(TMember member);
        void AddRange(System.Collections.Generic.List<TMember> members);
        void AddRange(TCollection container);
        void AddRange(TMember[] members);
        void Clear();
        int IndexOf(TMember member);
        void Insert(int index, TMember member);
        bool Remove(TMember member);
        void RemoveAt(int index);
    }
}
