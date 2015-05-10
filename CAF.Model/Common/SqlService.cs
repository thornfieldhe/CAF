using System.Data.SqlClient;

namespace CAF.Model
{

    public class SqlService : SingletonBase<SqlService>
    {
        public string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CAFConnectionString"].ToString();
            }
        }

        public SqlService() { }


        public SqlConnection Connection
        {
            get
            {
                var connection = new SqlConnection(this.ConnectionString);
                connection.Open();
                return connection;
            }
        }
    }
}
