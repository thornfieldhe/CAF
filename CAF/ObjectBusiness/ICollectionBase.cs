namespace CAF
{
    public interface ICollectionBase<in TCollection, TMember>
        where TCollection : CollectionBase<TCollection, TMember>
        where TMember : BaseEntity<TMember>,IEntityBase
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
        int Save();
        int SaveChanges(System.Data.IDbConnection conn, System.Data.IDbTransaction transaction);
    }
}
