namespace CAF
{
    public interface IDbAction
    {
        int Create();

        int Save();

        int Delete();

        void Remove();
    }
}