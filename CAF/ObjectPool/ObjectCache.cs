#region using
using System;
using System.Collections.Generic;
#endregion
namespace CAF.ObjectPool
{
    /// <summary>
    /// �ػ����󻺳�
    /// </summary>
    internal class ObjectCache:SingletonBase<ObjectCache>
    {
        /// <summary>
        /// ������ͻ����б�ע����
        /// </summary>
        private IDictionary<Type, object> registry;

        /// <summary>
        /// ��̬���������б�ע���
        /// </summary>
        public ObjectCache()
        {
            registry = new Dictionary<Type, object>();
        }

        /// <summary>
        /// ��ȡһ�����õĻ������
        /// </summary>
        /// <typeparam name="T">ָ���Ķ�������</typeparam>
        /// <param name="item">���õĶ���ʵ��</param>
        /// <param name="increasable">�Ƿ���Լ������</param>
        /// <returns>�Ƿ�ɹ���ÿ��õ�ʵ��</returns>
        internal bool TryToAcquire<T>(out T item, out bool increasable)
            where T : PoolableBase, new()
        {
            PrepareTypeList<T>();
            return (registry[typeof(T)] as SizeRestrictedList<T>).Acquire(out item, out increasable);
        }

        /// <summary>
        /// �����µ�ʵ��
        /// </summary>
        /// <param name="item"></param>
        internal void Cache<T>(T item)
            where T : PoolableBase, new()
        {
            PrepareTypeList<T>();
            (registry[typeof(T)] as SizeRestrictedList<T>).Add(item);
        }

        /// <summary>
        /// ׼���ض����ͻ����б�
        /// </summary>
        /// <param name="type"></param>
        private void PrepareTypeList<T>()
            where T : PoolableBase, new()
        {
            if (!registry.ContainsKey(typeof(T)))
                registry.Add(typeof(T), new SizeRestrictedList<T>());
        }
    }
}
