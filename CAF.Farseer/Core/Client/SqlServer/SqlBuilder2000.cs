using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;
using System.Text;

namespace CAF.FS.Core.Client.SqlServer
{
    /// <summary>
    /// 针对SqlServer 2000 数据库 提供
    /// </summary>
    public class SqlBuilder2000 : SqlBuilder
    {
        public SqlBuilder2000(BaseQueueManger queueManger, Queue queue) : base(queueManger, queue) { }

        public override Queue ToList(int pageSize, int pageIndex, bool isDistinct = false)
        {
            // 不分页
            if (pageIndex == 1)
            {
                this.ToList(pageSize, isDistinct); return this.Queue;
            }

            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);
            var strDistinctSql = isDistinct ? "Distinct" : string.Empty;
            this.Queue.Sql = new StringBuilder();

            strOrderBySql = "ORDER BY " + (string.IsNullOrWhiteSpace(strOrderBySql) ? string.Format("{0} ASC", this.Queue.FieldMap.PrimaryState.Value.FieldAtt.Name) : strOrderBySql);
            var strOrderBySqlReverse = strOrderBySql.Replace(" DESC", " [倒序]").Replace("ASC", "DESC").Replace("[倒序]", "ASC");

            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }

            this.Queue.Sql.AppendFormat("SELECT {0} TOP {2} {1} FROM (SELECT TOP {3} {1} FROM {4} {5} {6}) a  {7};", strDistinctSql, strSelectSql, pageSize, pageSize * pageIndex, this.Queue.Name, strWhereSql, strOrderBySql, strOrderBySqlReverse);
            return this.Queue;
        }
    }
}