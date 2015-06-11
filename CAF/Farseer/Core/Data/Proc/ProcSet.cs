using CAF.FS.Mapping.Context;
using System.Collections.Generic;
using System.Reflection;

namespace CAF.FS.Core.Data.Proc
{

    /// <summary>
    /// 存储过程操作
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public sealed class ProcSet<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly ProcContext _context;

        private ProcQueueManger QueueManger { get { return this._context.QueueManger; } }
        private Queue Queue { get { return this.QueueManger.CreateQueue(this._name, this._map); } }

        /// <summary>
        /// 存储过程名
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// 实体类映射
        /// </summary>
        private readonly FieldMap _map;

        /// <summary>
        /// 禁止外部实例化
        /// </summary>
        private ProcSet() { }

        /// <summary>
        /// 使用属性类型的创建
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="pInfo">属性类型</param>
        public ProcSet(ProcContext context, PropertyInfo pInfo) : this(context, pInfo.Name) { }

        /// <summary>
        /// 使用属性名称的创建
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="propertyName">属性名称</param>
        public ProcSet(ProcContext context, string propertyName)
        {
            this._context = context;
            this._map = typeof(TEntity);
            var contextState = this._context.ContextMap.GetState(this.GetType(), propertyName);
            this._name = contextState.Value.SetAtt.Name;
        }

        /// <summary>
        /// 返回查询的值
        /// </summary>
        public T GetValue<T>(TEntity entity = null, T t = default(T))
        {
            // 加入委托
            this.QueueManger.Append(this._name, this._map, (queryQueue) => t = queryQueue.ExecuteQuery.ToValue(entity, t), true);
            return t;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        public void Execute(TEntity entity = null)
        {
            // 加入委托
            this.QueueManger.Append(this._name, this._map, (queryQueue) => queryQueue.ExecuteQuery.Execute(entity), !this._context.IsMergeCommand);
        }

        /// <summary>
        /// 返回单条记录
        /// </summary>
        public TEntity ToEntity(TEntity entity = null)
        {
            // 加入委托
            this.QueueManger.Append(this._name, this._map, (queryQueue) => entity = queryQueue.ExecuteQuery.ToEntity(entity), true);
            return entity;
        }

        /// <summary>
        /// 返回多条记录
        /// </summary>
        public List<TEntity> ToList(TEntity entity = null)
        {
            List<TEntity> lst = null;
            // 加入委托
            this.QueueManger.Append(this._name, this._map, (queryQueue) => lst = queryQueue.ExecuteQuery.ToList(entity), true);
            return lst;
        }
    }
}