using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.Data;

    public partial class Organize
    {
        /// <summary>
        /// 获取部门及子部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SortLevelItem> GetChildrenOrganizes(Guid id)
        {
            var item = Get(id);
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                const string query = "Select Id,Name,[Level] From Sys_Organize Where Level Like '%'+@Level";
                return conn.Query<SortLevelItem>(query, new { Level = item == null ? "00" : item.Level })
                    .Select(d => new SortLevelItem { Id = d.Id, Level = d.Level, Name = (new string('-', d.Level.Length * 3)) + d.Name })
                    .OrderBy(d => d.Level).ToList();
            }
        }
    }
}
