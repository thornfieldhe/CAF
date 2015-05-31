#region using
using System;
using CAF.ObjectPool;
using CAF.Core;
#endregion
namespace CAF
{
    /// <summary>
    /// 基本的抽象可池化类型
    /// <remarks>
    ///     由于其实例可以被重用，因此设计上不支持上下文状态信息
    /// </remarks>
    /// </summary>
    public abstract class PoolableBase : IPoolable
    {
        #region Private Field
        private string guid;
        private Type type;
        private DateTime createTime;
        private IObjectPoolState state;
        #endregion

        #region Protected Field
        protected DateTime accessedTime;
        #endregion

        #region Constructor
        /// <summary>
        /// 构造池化对象的基本准备过程
        /// </summary>
        public PoolableBase()
        {
            this.guid = System.Guid.NewGuid().ToString();
            this.type = GetType();
            this.createTime = DateTime.Now;
            this.accessedTime = DateTime.Now;
            this.state = SingletonBase<ConstructedState>.Instance;
        }
        #endregion

        #region Public Property
        /// <summary>
        /// 当前实例在内存的唯一标识
        /// </summary>
        public virtual string Guid
        {
            get { return this.guid; ; }
        }

        /// <summary>
        /// 实例类型
        /// </summary>
        public virtual Type Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// 当前实例创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return this.createTime; }
        }

        /// <summary>
        /// 最近一次被访问时间
        /// </summary>
        public virtual DateTime AccessedTime
        {
            get { return this.accessedTime; }
        }

        /// <summary>
        /// 当前实例的运行状态
        /// </summary>
        public virtual IObjectPoolState State
        {
            get { return this.state; }
        }
        #endregion

        #region State Management

        /// <summary>
        /// 当前实例是否可以执行相应的功能
        /// </summary>
        public bool Executable
        {
            get { return state.Executable; }
        }

        /// <summary>
        /// 池化实例是否空闲着
        /// </summary>
        public bool Unoccupied
        {
            get { return state.Unoccupied; }
        }

        /// <summary>
        /// 修改当前的池化实例的状态
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IObjectPoolState newState)
        {
            this.state = newState;
        }

        /// <summary>
        /// 激活使用，此时排除其他对象对该实例的使用
        /// </summary>
        public virtual void Activate()
        {
            state.Activate(this);
        }

        /// <summary>
        /// 没有被客户程序占用，此时其他对象可以继续使用该实例
        /// </summary>
        public virtual void Deactivate()
        {
            state.Deactivate(this);
        }

        /// <summary>
        /// 释放, 此时其他对象不可以继续使用该实例
        /// </summary>
        public virtual void Dispose()
        {
            state.Dispose(this);
        }
        #endregion

        #region Helper method
        /// <summary>
        /// 更新最近被访问时间
        /// </summary>
        protected void PreProcess()
        {
            if (!Executable) throw new NotSupportedException();
            this.accessedTime = DateTime.Now;
        }
        #endregion
    }
}
