using CAF.FS.Core.Infrastructure;
using CAF.FS.Extends;
using CAF.FS.Mapping.Context;
using FS.Configs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace CAF.FS.Core.Data
{
    /// <summary>
    /// 每一次的数据库查询，将生成一个新的实例
    /// </summary>
    public class Queue
    {
        /// <summary>
        /// 当前队列的ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 当前组索引
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// 表名/视图名/存储过程名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 当前生成的参数
        /// </summary>
        public List<DbParameter> Param { get; set; }
        /// <summary>
        /// 实体类映射
        /// </summary>
        public FieldMap FieldMap { get; private set; }
        /// <summary>
        /// 当前生成的SQL语句
        /// </summary>
        public StringBuilder Sql { get; set; }
        /// <summary>
        /// SQL生成器
        /// </summary>
        public ISqlBuilder SqlBuilder { get; private set; }
        /// <summary>
        /// 延迟执行的委托
        /// </summary>
        public Action<Queue> LazyAct { get; set; }
        /// <summary>
        /// 排序表达式树
        /// </summary>
        public Dictionary<Expression, bool> ExpOrderBy { get; private set; }
        /// <summary>
        /// 字段筛选表达式树
        /// </summary>
        public List<Expression> ExpSelect { get; private set; }
        /// <summary>
        /// 条件表达式树
        /// </summary>
        public Expression ExpWhere { get; private set; }
        /// <summary>
        /// 赋值表达式树
        /// </summary>
        public Dictionary<Expression, object> ExpAssign { get; private set; }
        /// <summary>
        /// 队列管理模块
        /// </summary>
        public readonly BaseQueueManger QueueManger;
        public Queue(int index, string name, FieldMap map, BaseQueueManger queueManger)
        {
            this.ID = Guid.NewGuid();
            this.Index = index;
            this.Name = name;
            this.Param = new List<DbParameter>();
            this.FieldMap = map;
            this.QueueManger = queueManger;
            this.SqlBuilder = queueManger.DbProvider.CreateSqlBuilder(queueManger, this);
            this.ExecuteQuery = SystemConfigs.ConfigEntity.IsWriteDbLog ? new ExecuteQueryProxy(this) : new ExecuteQuery(this);
        }

        /// <summary>
        /// 添加筛选
        /// </summary>
        /// <param name="select"></param>
        public void AddSelect(Expression select)
        {
            if (this.ExpSelect == null) { this.ExpSelect = new List<Expression>(); }
            if (select != null) { this.ExpSelect.Add(select); }
        }
        /// <summary>
        ///     添加条件
        /// </summary>
        /// <param name="where">查询条件</param>
        public void AddWhere<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            this.ExpWhere = this.ExpWhere == null ? this.ExpWhere = where : ((Expression<Func<TEntity, bool>>)this.ExpWhere).AndAlso(where);
        }

        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isAsc"></param>
        public void AddOrderBy(Expression exp, bool isAsc)
        {
            if (this.ExpOrderBy == null) { this.ExpOrderBy = new Dictionary<Expression, bool>(); }
            if (exp != null) { this.ExpOrderBy.Add(exp, isAsc); }
        }
        /// <summary>
        /// 字段累加（字段 = 字段 + 值）
        /// </summary>
        /// <param name="fieldName">字段选择器</param>
        /// <param name="fieldValue">值</param>
        public void AddAssign(Expression fieldName, object fieldValue)
        {
            if (this.ExpAssign == null) { this.ExpAssign = new Dictionary<Expression, object>(); }
            if (fieldName != null) { this.ExpAssign.Add(fieldName, fieldValue); }
        }
        /// <summary>
        /// 复制条件
        /// </summary>
        /// <param name="queue">队列</param>
        public void Copy(Queue queue)
        {
            this.ExpOrderBy = queue.ExpOrderBy;
            this.ExpSelect = queue.ExpSelect;
            this.ExpWhere = queue.ExpWhere;
        }
        /// <summary>
        /// 执行数据库操作
        /// </summary>
        public ExecuteQuery ExecuteQuery { get; private set; }

        #region 释放
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        private void Dispose(bool disposing)
        {
            //释放托管资源
            if (disposing)
            {
                if (this.Sql != null)
                {
                    this.Sql.Clear();
                    this.Sql = null;
                }

                this.ExpOrderBy = null;
                this.ExpSelect = null;
                this.ExpWhere = null;
                if (this.Param != null) { this.Param.Clear(); }
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
