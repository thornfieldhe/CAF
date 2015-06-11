using CAF.FS.Core.Infrastructure;
using CAF.FS.Mapping.Context;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CAF.FS.Core.Data.Table
{
    /// <summary>
    /// 队列管理
    /// </summary>
    public class TableQueueManger : BaseQueueManger
    {
        /// <summary>
        /// 当前所有持久化列表
        /// </summary>
        private readonly List<Queue> _groupQueueList;
        /// <summary>
        /// 所有队列的参数
        /// </summary>
        public override List<DbParameter> Param
        {
            get
            {
                var lst = new List<DbParameter>();
                this._groupQueueList.Where(o => o.Param != null).Select(o => o.Param).ToList().ForEach(o => o.ForEach(oo =>
                {
                    if (!lst.Exists(x => oo.ParameterName == x.ParameterName)) { lst.Add(oo); }
                }));
                return lst;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database">数据库操作</param>
        /// <param name="contextMap">映射关系</param>
        public TableQueueManger(DbExecutor database, ContextMap contextMap)
            : base(database, contextMap)
        {
            this._groupQueueList = new List<Queue>();
        }

        /// <summary>
        /// 获取当前队列（不存在，则创建）
        /// </summary>
        /// <param name="map">字段映射</param>
        /// <param name="name">表名称</param>
        public override Queue CreateQueue(string name, FieldMap map)
        {
            return this.Queue ?? (this.Queue = new Queue(this._groupQueueList.Count, name, map, this));
        }

        /// <summary>
        /// 延迟执行数据库交互，并提交到队列
        /// </summary>
        /// <param name="act">要延迟操作的委托</param>
        /// <param name="map">字段映射</param>
        /// <param name="name">表名称</param>
        /// <param name="isExecute">是否立即执行</param>
        public override void Append(string name, FieldMap map, Action<Queue> act, bool isExecute)
        {
            try
            {
                this.CreateQueue(name, map);
                if (isExecute) { act(this.Queue); return; }
                this.Queue.LazyAct = act;
                if (this.Queue != null) { this._groupQueueList.Add(this.Queue); }
            }
            finally
            {
                this.Clear();
            }
        }

        /// <summary>
        /// 提交所有GetQueue，完成数据库交互
        /// </summary>
        public int Commit()
        {
            try
            {
                foreach (var queryQueue in this._groupQueueList)
                {
                    // 查看是否延迟执行
                    if (queryQueue.LazyAct != null) { queryQueue.LazyAct(queryQueue); }
                    // 清除队列
                    queryQueue.Dispose();
                }
            }
            finally
            {
                // 清除队列
                this._groupQueueList.Clear();
                this.Clear();
            }
            return 0;
        }
    }
}