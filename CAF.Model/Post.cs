using System;
using System.Collections.Generic;
using System.Linq;
using CAF.Utility;
namespace CAF.Model
{
    using CAF.Data;
    using System.Data;

    public partial class Post
    {
        public static List<Kuple<Guid, string>> GetSimpleRoleList()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                const string query = "Select Id as [Key],Name as [Value] From Sys_Posts Where Status<>-1";
                return conn.Query<Kuple<Guid, string>>(query).ToList();
            }
        }
    }
}
