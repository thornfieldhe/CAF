using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;
using System.Text;

namespace CAF.FS.Core.Client.MySql
{
    public class SqlBuilder : Common.SqlBuilder
    {
        /// <summary>
        /// 查询支持的SQL方法
        /// </summary>
        /// <param name="queueManger">队列管理模块</param>
        /// <param name="queue">包含数据库SQL操作的队列</param>
        public SqlBuilder(BaseQueueManger queueManger, Queue queue) : base(queueManger, queue) { }

        public override Queue ToEntity()
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (!string.IsNullOrWhiteSpace(strOrderBySql)) { strOrderBySql = "ORDER BY " + strOrderBySql; }

            this.Queue.Sql.AppendFormat("SELECT {0} FROM {1} {2} {3} LIMIT 1", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
            return this.Queue;
        }

        public override Queue ToList(int top = 0, bool isDistinct = false, bool isRand = false)
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);
            var strTopSql = top > 0 ? string.Format("LIMIT {0}", top) : string.Empty;
            var strDistinctSql = isDistinct ? "Distinct" : string.Empty;

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (!string.IsNullOrWhiteSpace(strOrderBySql)) { strOrderBySql = "ORDER BY " + strOrderBySql; }

            if (!isRand)
            {
                this.Queue.Sql.AppendFormat("SELECT {0} {1} FROM {2} {3} {4} {5}", strDistinctSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql, strTopSql);
            }
            else if (string.IsNullOrWhiteSpace(strOrderBySql))
            {
                this.Queue.Sql.AppendFormat("SELECT {0} {1}{5} FROM {2} {3} ORDER BY Rand() {4}", strDistinctSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strTopSql, isDistinct ? ",Rand() as newid" : "");
            }
            else
            {
                this.Queue.Sql.AppendFormat("SELECT {1} FROM (SELECT {0} *{6} FROM {2} {3} ORDER BY Rand() {5}) a {4}", strDistinctSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql, strTopSql, isDistinct ? ",Rand() as newid" : "");
            }
            return this.Queue;
        }

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

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (!string.IsNullOrWhiteSpace(strOrderBySql)) { strOrderBySql = "ORDER BY " + strOrderBySql; }

            this.Queue.Sql.AppendFormat("SELECT {0} {1} FROM {2} {3} {4} LIMIT {5},{6}", strDistinctSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql, pageSize * (pageIndex - 1), pageSize);
            return this.Queue;
        }

        public override Queue GetValue()
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (!string.IsNullOrWhiteSpace(strOrderBySql)) { strOrderBySql = "ORDER BY " + strOrderBySql; }

            this.Queue.Sql.AppendFormat("SELECT {0} FROM {1} {2} {3} LIMIT 1", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
            return this.Queue;
        }

        public override Queue InsertIdentity<TEntity>(TEntity entity)
        {
            base.InsertIdentity(entity);
            this.Queue.Sql.AppendFormat("SELECT @@IDENTITY;");
            return this.Queue;
        }
    }
}
