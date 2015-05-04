using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.Data;

    public partial class Post
    {
        public static List<KeyValueItem<Guid, string>> GetSimpleRoleList()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                const string query = "Select Id as [Key],Name as [Value] From Sys_Posts Where Status<>-1";
                return conn.Query<KeyValueItem<Guid, string>>(query, null).ToList();
            }
        }
    }
}
