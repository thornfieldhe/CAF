using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;
using System.Text;

namespace CAF.FS.Core.Client.Common
{
    using CAF.FS.Utils;

    public class SqlBuilder : ISqlBuilder
    {
        /// <summary>
        /// 队列管理模块
        /// </summary>
        protected readonly BaseQueueManger QueueManger;
        /// <summary>
        /// 包含数据库SQL操作的队列
        /// </summary>
        protected readonly Queue Queue;
        /// <summary>
        /// 数据库字段解析器总入口，根据要解析的类型，再分散到各自负责的解析器
        /// </summary>
        protected readonly ExpressionVisit Visit;

        /// <summary>
        /// 查询支持的SQL方法
        /// </summary>
        /// <param name="queueManger">队列管理模块</param>
        /// <param name="queue">包含数据库SQL操作的队列</param>
        public SqlBuilder(BaseQueueManger queueManger, Queue queue)
        {
            this.QueueManger = queueManger;
            this.Queue = queue;
            this.Visit = new ExpressionVisit(queueManger, this.Queue);
        }

        public virtual Queue ToEntity()
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (!string.IsNullOrWhiteSpace(strOrderBySql)) { strOrderBySql = "ORDER BY " + strOrderBySql; }

            this.Queue.Sql.AppendFormat("SELECT TOP 1 {0} FROM {1} {2} {3}", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
            return this.Queue;
        }

        public virtual Queue ToList(int top = 0, bool isDistinct = false, bool isRand = false)
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);
            var strTopSql = top > 0 ? string.Format("TOP {0}", top) : string.Empty;
            var strDistinctSql = isDistinct ? "Distinct" : string.Empty;

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (!string.IsNullOrWhiteSpace(strOrderBySql)) { strOrderBySql = "ORDER BY " + strOrderBySql; }

            if (!isRand)
            {
                this.Queue.Sql.AppendFormat("SELECT {0} {1} {2} FROM {3} {4} {5}", strDistinctSql, strTopSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
            }
            else if (string.IsNullOrWhiteSpace(strOrderBySql))
            {
                this.Queue.Sql.AppendFormat("SELECT {0} {1} {2}{5} FROM {3} {4} ORDER BY NEWID()", strDistinctSql, strTopSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, isDistinct ? ",NEWID() as newid" : "");
            }
            else
            {
                this.Queue.Sql.AppendFormat("SELECT {2} FROM (SELECT {0} {1} *{6} FROM {3} {4} ORDER BY NEWID()) a {5}", strDistinctSql, strTopSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql, isDistinct ? ",NEWID() as newid" : "");
            }
            return this.Queue;
        }

        public virtual Queue ToList(int pageSize, int pageIndex, bool isDistinct = false)
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

            this.Queue.Sql.AppendFormat("SELECT {0} TOP {2} {1} FROM (SELECT TOP {3} * FROM {4} {5} {6}) a  {7};", strDistinctSql, strSelectSql, pageSize, pageSize * pageIndex, this.Queue.Name, strWhereSql, strOrderBySql, strOrderBySqlReverse);
            return this.Queue;
        }

        public virtual Queue Count(bool isDistinct = false)
        {
            this.Queue.Sql = new StringBuilder();
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strDistinctSql = isDistinct ? "Distinct" : string.Empty;

            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }

            this.Queue.Sql.AppendFormat("SELECT {0} Count(0) FROM {1} {2}", strDistinctSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql);
            return this.Queue;
        }

        public virtual Queue Sum()
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "0"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }

            this.Queue.Sql.AppendFormat("SELECT SUM({0}) FROM {1} {2}", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql);
            return this.Queue;
        }

        public virtual Queue Max()
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "0"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }

            this.Queue.Sql.AppendFormat("SELECT MAX({0}) FROM {1} {2}", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql);
            return this.Queue;
        }

        public virtual Queue Min()
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "0"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }

            this.Queue.Sql.AppendFormat("SELECT MIN({0}) FROM {1} {2}", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql);
            return this.Queue;
        }

        public virtual Queue GetValue()
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);

            if (string.IsNullOrWhiteSpace(strSelectSql)) { strSelectSql = "*"; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }
            if (!string.IsNullOrWhiteSpace(strOrderBySql)) { strOrderBySql = "ORDER BY " + strOrderBySql; }

            this.Queue.Sql.AppendFormat("SELECT TOP 1 {0} FROM {1} {2} {3}", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
            return this.Queue;
        }

        public virtual Queue Delete()
        {
            this.Queue.Sql = new StringBuilder();
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);

            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }

            this.Queue.Sql.AppendFormat("DELETE FROM {0} {1}", this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql);
            return this.Queue;
        }

        public virtual Queue Insert<TEntity>(TEntity entity) where TEntity : class,new()
        {
            this.Queue.Sql = new StringBuilder();
            var strinsertAssemble = this.Visit.Insert(entity);

            this.Queue.Sql.AppendFormat("INSERT INTO {0} {1}", this.Queue.Name, strinsertAssemble);
            return this.Queue;
        }

        public virtual Queue InsertIdentity<TEntity>(TEntity entity) where TEntity : class,new()
        {
            this.Queue.Sql = new StringBuilder();
            var strinsertAssemble = this.Visit.Insert(entity);
            this.Queue.Sql.AppendFormat("INSERT INTO {0} {1}", this.Queue.Name, strinsertAssemble);
            return this.Queue;
        }

        public virtual Queue Update<TEntity>(TEntity entity) where TEntity : class,new()
        {
            this.Queue.Sql = new StringBuilder();
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strAssemble = this.Visit.Assign(entity);
            var readCondition = this.Visit.ReadCondition(entity);

            Check.NotEmpty(strAssemble, "更新操作时，当前实体没有要更新的字段。" + typeof(TEntity));

            // 主键如果有值、或者设置成只读条件，则自动转成条件
            if (!string.IsNullOrWhiteSpace(readCondition)) { strWhereSql += string.IsNullOrWhiteSpace(strWhereSql) ? readCondition : " AND " + readCondition; }
            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }

            this.Queue.Sql.AppendFormat("UPDATE {0} SET {1} {2}", this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strAssemble, strWhereSql);
            return this.Queue;
        }

        public virtual Queue AddUp()
        {
            Check.IsTure(this.Queue.ExpAssign == null || this.Queue.ExpAssign.Count == 0, "赋值的参数不能为空！");

            this.Queue.Sql = new StringBuilder();
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);

            if (!string.IsNullOrWhiteSpace(strWhereSql)) { strWhereSql = "WHERE " + strWhereSql; }

            #region 字段赋值
            var sqlAssign = new StringBuilder();
            foreach (var keyValue in this.Queue.ExpAssign)
            {
                var strAssemble = this.Visit.Assign(keyValue.Key);
                var strs = strAssemble.Split(',');
                foreach (var s in strs) { sqlAssign.AppendFormat("{0} = {0} + {1},", s, keyValue.Value); }
            }
            if (sqlAssign.Length > 0) { sqlAssign = sqlAssign.Remove(sqlAssign.Length - 1, 1); }
            #endregion

            this.Queue.Sql.AppendFormat("UPDATE {0} SET {1} {2}", this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), sqlAssign, strWhereSql);
            return this.Queue;
        }
    }
}
