#region using
using System;

#endregion
namespace CAF.ObjectPool
{
    /// <summary>
    /// �����ɳػ����������ӿ�
    /// </summary>S
    public interface IPoolable : IDisposable
    {
        /// <summary>
        /// Ψһ��ʶ
        /// </summary>
        string Guid { get;}             

        /// <summary>
        /// ʵ������
        /// </summary>
        Type Type { get;}
    
        /// <summary>
        /// ���δ���ʱ��
        /// </summary>
        DateTime CreateTime { get;}     

        /// <summary>
        /// �������ʱ��
        /// </summary>
        DateTime AccessedTime { get;}

        /// <summary>
        /// ��ǰʵ���Ƿ����ִ����Ӧ�Ĺ���
        /// </summary>
        bool Executable { get;}

        /// <summary>
        /// �ػ�ʵ���Ƿ������
        /// </summary>
        bool Unoccupied { get;}

        /// <summary>
        /// ����ʹ�ã���ʱ�ų���������Ը�ʵ����ʹ��
        /// </summary>
        void Activate();

        /// <summary>
        /// �ͷţ���ʱ����������Լ���ʹ�ø�ʵ��
        /// </summary>
        void Deactivate();

        /// <summary>
        /// �޸ĵ�ǰ�ĳػ�ʵ����״̬
        /// </summary>
        /// <param name="newState"></param>
        void ChangeState(IObjectPoolState newState);
    }
}
