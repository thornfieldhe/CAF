﻿using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;
using System.Text;

namespace CAF.FS.Core.Client.Oracle
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

            this.Queue.Sql.AppendFormat("SELECT {0} FROM {1} {2} {3} rownum <=1", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
            return this.Queue;
        }

        public override Queue ToList(int top = 0, bool isDistinct = false, bool isRand = false)
        {
            this.Queue.Sql = new StringBuilder();
            var strSelectSql = this.Visit.Select(this.Queue.ExpSelect);
            var strWhereSql = this.Visit.Where(this.Queue.ExpWhere);
            var strOrderBySql = this.Visit.OrderBy(this.Queue.ExpOrderBy);
            var strTopSql = top > 0 ? string.Format("rownum <={0}", top) : string.Empty;
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
                this.Queue.Sql.AppendFormat("SELECT {0} {1}{5} FROM {2} {3} ORDER BY dbms_random.value {4}", strDistinctSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strTopSql, isDistinct ? ",dbms_random.value as newid" : "");
            }
            else
            {
                this.Queue.Sql.AppendFormat("SELECT {1} FROM (SELECT {0} *{6} FROM {2} {3} ORDER BY dbms_random.value {5}) a {4}", strDistinctSql, strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql, strTopSql, isDistinct ? ",dbms_random.value as newid" : "");
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

            this.Queue.Sql.AppendFormat("SELECT * FROM ( SELECT A.*, ROWNUM RN FROM (SELECT {0} {1} FROM {4} {5} {6}) A WHERE ROWNUM <= {3} ) WHERE RN > {2}", strDistinctSql, strSelectSql, pageSize * (pageIndex - 1), pageSize * pageIndex, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
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

            this.Queue.Sql.AppendFormat("SELECT {0} FROM {1} {2} {3} rownum <=1", strSelectSql, this.QueueManger.DbProvider.KeywordAegis(this.Queue.Name), strWhereSql, strOrderBySql);
            return this.Queue;
        }
        public override Queue InsertIdentity<TEntity>(TEntity entity)
        {
            base.InsertIdentity(entity);
            this.Queue.Sql.AppendFormat("SELECT @@IDENTITY ");
            return this.Queue;
        }
    }
}
