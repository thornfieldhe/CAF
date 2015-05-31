namespace CAF.ObjectPool
{
    /// <summary>
    /// ����س���״̬�ӿ�
    /// </summary>
    public interface IObjectPoolState
    {
        /// <summary>
        /// ��ǰ״̬�£��ػ�ʵ���Ƿ����ִ��
        /// </summary>
        bool Executable { get;}

        /// <summary>
        /// ��ǰ״̬�£��ػ�ʵ���Ƿ������
        /// </summary>
        bool Unoccupied { get;}

        /// <summary>
        /// ����ʹ�ã���ʱ�ų���������Ը�ʵ����ʹ��
        /// </summary>
        void Activate(IPoolable item);

        /// <summary>
        /// û�б��ͻ�����ռ�ã���ʱ����������Լ���ʹ�ø�ʵ��
        /// </summary>
        void Deactivate(IPoolable item);

        /// <summary>
        /// �ͷ�, ��ʱ�������󲻿��Լ���ʹ�ø�ʵ��
        /// </summary>
        void Dispose(IPoolable item);
    }
}
