using System.Data.SqlClient;

namespace CAF.Model
{
    using System.Data;

    public class SqlService : SingletonBase<SqlService>
    {
        private  readonly string sqlconnection = System.Configuration.ConfigurationManager.ConnectionStrings["CAFConnectionString"].ToString();

        public SqlService() { _sqlConnection = new SqlConnection(sqlconnection); _sqlConnection.Open(); }

        private readonly SqlConnection _sqlConnection;
        public SqlConnection Connection
        {
            get
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.ConnectionString = sqlconnection;
                    _sqlConnection.Open();
                }
                return _sqlConnection;
            }
        }
    }
}
