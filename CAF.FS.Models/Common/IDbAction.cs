namespace CAF.FSModels
{
    public interface IDbAction
    {
        int Create();

        int Save();

        int Delete();

        void Remove();
    }
}