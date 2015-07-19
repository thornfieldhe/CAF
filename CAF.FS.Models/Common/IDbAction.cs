namespace CAF.Models
{
    using System;

    public interface IDbAction<T> where T : new()
    {
        int Create();

        int Save();

        int Delete();

        void Remove();
        T Find(Guid id);
    }
}