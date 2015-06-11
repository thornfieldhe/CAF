using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;
using System;
using System.Text;

namespace CAF.FS.Core.Client.SqlServer
{
    public class SqlBuilder : Common.SqlBuilder
    {
        /// <summary>
        /// 查询支持的SQL方法
        /// </summary>
        /// <param name="queueManger">队列管理模块</param>
        /// <param name="queue">包含数据库SQL操作的队列</param>
        public SqlBuilder(BaseQueueManger queueManger, Queue queue) : base(queueManger, queue) { }

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
            if (string.IsNullOrWhiteSpace(strOrderBySql) && string.IsNullOrWhiteSpace(this.Queue.FieldMap.PrimaryState.Value.FieldAtt.Name)) { throw new Exception("当未指定排序方式时，必须要指定 主键字段"); }

            strOrderBySql = "ORDER BY " + (string.IsNullOrWhiteSpace(strOrderBySql) ? string.Format("{0} ASC", this.Queue.FieldMap.PrimaryState.Value.FieldAtt.Name) : strOrderBySql);

            this.Queue.Sql.AppendFormat("SELECT {1} FROM (SELECT {0} {1},ROW_NUMBER() OVER({2}) as Row FROM {3} {4}) a WHERE Row BETWEEN {5} AND {6};", strDistinctSql, strSelectSql, strOrderBySql, this.Queue.Name, strWhereSql, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            return this.Queue;
        }

        public override Queue Insert<TEntity>(TEntity entity)
        {
            base.Insert(entity);

            // 主键如果有值，则需要 SET IDENTITY_INSERT ON
            var indexHaveValue = this.Queue.FieldMap.PrimaryState.Key != null && this.Queue.FieldMap.PrimaryState.Key.GetValue(entity, null) != null;
            if (indexHaveValue && !string.IsNullOrWhiteSpace(this.Queue.FieldMap.PrimaryState.Value.FieldAtt.Name))
            {
                this.Queue.Sql = new StringBuilder(string.Format("SET IDENTITY_INSERT {0} ON ; {1} ; SET IDENTITY_INSERT {0} OFF;", this.Queue.Name, this.Queue.Sql));
            }
            return this.Queue;
        }

        public override Queue InsertIdentity<TEntity>(TEntity entity)
        {
            this.Insert(entity);
            this.Queue.Sql.AppendFormat("SELECT @@IDENTITY;");
            return this.Queue;
        }
    }
}