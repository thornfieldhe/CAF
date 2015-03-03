
namespace CAF.Model
{
    using CAF.Data;
    using System.Data;
    using System.Linq;

    public partial class User
    {
        public static bool Exists(string name)
        {
            const string query = "SELECT Count(*) FROM Sys_Users WHERE Name Like '%'+@Name+'%' Or Abb Like '%'+@Name+'%'";

            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(query, new { Name = name }).Single() >= 1;
            }
        }
    }

}
