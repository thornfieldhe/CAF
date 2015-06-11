using CAF.FS.Mapping.Context;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace CAF.FS.Core.Infrastructure
{
    using CAF.FS.Core.Data;

    /// <summary>
    /// 队列管理模块
    /// </summary>
    public abstract class BaseQueueManger : IDisposable
    {
        /// <summary>
        /// 数据库提供者（不同数据库的特性）
        /// </summary>
        public DbProvider DbProvider { get; private set; }

        /// <summary>
        /// 返回所有队列的参数Param
        /// </summary>
        public virtual List<DbParameter> Param
        {
            get
            {
                return this.Queue.Param;
            }
        }

        /// <summary>
        /// 数据库操作
        /// </summary>
        public DbExecutor DataBase { get; private set; }
        /// <summary>
        /// 映射关系
        /// </summary>
        public ContextMap ContextMap { get; private set; }

        /// <summary>
        /// 当前组查询队列（支持批量提交SQL）
        /// </summary>
        protected Data.Queue Queue;

        protected BaseQueueManger(DbExecutor database, ContextMap contextMap)
        {
            this.DataBase = database;
            this.ContextMap = contextMap;
            this.DbProvider = DbProvider.CreateInstance(database.DataType);
        }

        /// <summary>
        /// 清除当前队列
        /// </summary>
        public void Clear()
        {
            this.Queue = null;
        }

        /// <summary>
        /// 获取当前队列（不存在，则创建）
        /// </summary>
        /// <param name="map">字段映射</param>
        /// <param name="name">表名称</param>
        public virtual Data.Queue CreateQueue(string name, FieldMap map)
        {
            return this.Queue ?? (this.Queue = new Data.Queue(0, name, map, this));
        }

        /// <summary>
        /// 延迟执行数据库交互，并提交到队列
        /// </summary>
        /// <param name="act">要延迟操作的委托</param>
        /// <param name="map">字段映射</param>
        /// <param name="name">表名称</param>
        /// <param name="isExecute">是否立即执行</param>
        public virtual void Append(string name, FieldMap map, Action<Data.Queue> act, bool isExecute)
        {
            try
            {
                if (isExecute) { act(this.CreateQueue(name, map)); return; }
                this.Queue.LazyAct = act;
            }
            finally
            {
                this.Clear();
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        private void Dispose(bool disposing)
        {
            //释放托管资源
            if (disposing)
            {
                this.DataBase.Dispose();
                this.DataBase = null;
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
    }
}