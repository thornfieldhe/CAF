using System.Data.SqlClient;

namespace CAF.Model
{

    public class SqlService : SingletonBase<SqlService>
    {
        private readonly string sqlconnection = System.Configuration.ConfigurationManager.ConnectionStrings["CAFConnectionString"].ToString();

        public SqlService() { }


        public SqlConnection Connection
        {
            get
            {
                var connection = new SqlConnection(sqlconnection);
                connection.Open();
                return connection;
            }
        }
    }
}
