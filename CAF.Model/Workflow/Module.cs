using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.Data;

    public partial class Module : BaseEntity<Module>
    {
        public static Module Get(string key)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                const string QUERY_GETBYKEY = "SELECT Top 1 * FROM Sys_Modules WHERE Key = @Key  AND Status!=-1";
                var item = conn.Query<Module>(QUERY_GETBYKEY, new { Key = key }).SingleOrDefault<Module>();
                if (item == null)
                {
                    return null;
                }
                item.Connection = SqlService.Instance.Connection;
                item.MarkOld();
                return item;
            }
        }
    }
}


