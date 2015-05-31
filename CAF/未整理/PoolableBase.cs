#region using
using System;
using CAF.ObjectPool;
using CAF.Core;
#endregion
namespace CAF
{
    /// <summary>
    /// �����ĳ���ɳػ�����
    /// <remarks>
    ///     ������ʵ�����Ա����ã��������ϲ�֧��������״̬��Ϣ
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
        /// ����ػ�����Ļ���׼������
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
        /// ��ǰʵ�����ڴ��Ψһ��ʶ
        /// </summary>
        public virtual string Guid
        {
            get { return this.guid; ; }
        }

        /// <summary>
        /// ʵ������
        /// </summary>
        public virtual Type Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// ��ǰʵ������ʱ��
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return this.createTime; }
        }

        /// <summary>
        /// ���һ�α�����ʱ��
        /// </summary>
        public virtual DateTime AccessedTime
        {
            get { return this.accessedTime; }
        }

        /// <summary>
        /// ��ǰʵ��������״̬
        /// </summary>
        public virtual IObjectPoolState State
        {
            get { return this.state; }
        }
        #endregion

        #region State Management

        /// <summary>
        /// ��ǰʵ���Ƿ����ִ����Ӧ�Ĺ���
        /// </summary>
        public bool Executable
        {
            get { return state.Executable; }
        }

        /// <summary>
        /// �ػ�ʵ���Ƿ������
        /// </summary>
        public bool Unoccupied
        {
            get { return state.Unoccupied; }
        }

        /// <summary>
        /// �޸ĵ�ǰ�ĳػ�ʵ����״̬
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IObjectPoolState newState)
        {
            this.state = newState;
        }

        /// <summary>
        /// ����ʹ�ã���ʱ�ų���������Ը�ʵ����ʹ��
        /// </summary>
        public virtual void Activate()
        {
            state.Activate(this);
        }

        /// <summary>
        /// û�б��ͻ�����ռ�ã���ʱ����������Լ���ʹ�ø�ʵ��
        /// </summary>
        public virtual void Deactivate()
        {
            state.Deactivate(this);
        }

        /// <summary>
        /// �ͷ�, ��ʱ�������󲻿��Լ���ʹ�ø�ʵ��
        /// </summary>
        public virtual void Dispose()
        {
            state.Dispose(this);
        }
        #endregion

        #region Helper method
        /// <summary>
        /// �������������ʱ��
        /// </summary>
        protected void PreProcess()
        {
            if (!Executable) throw new NotSupportedException();
            this.accessedTime = DateTime.Now;
        }
        #endregion
    }
}
