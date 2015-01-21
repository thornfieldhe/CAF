#region using
using System;
using System.Collections.Generic;
using System.Threading;
using CAF.Core;
using CAF.Configuration;
#endregion
namespace CAF.ObjectPool
{
    /// <summary>
    /// �����������Ƶĳػ����󻺳� List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SizeRestrictedList<T>
        where T : PoolableBase, new()
    {
        /// <summary>
        /// ��ػ�������
        /// </summary>
        private PoolableConfigurationElement configuration;

        /// <summary>
        /// �Զ�����ʱ�Ӷ���
        /// </summary>
        private Timer timer;

        /// <summary>
        /// ����ʵ������ List
        /// </summary>
        private IList<T> cache;

        /// <summary>
        /// ����������������ض����͵Ļ��� List
        /// </summary>
        /// <param name="typeName">ʵ�ʻ���Ķ�����������</param>
        internal SizeRestrictedList()
        {
            configuration =CAFConfiguration.Instance.ObjectPoolCache[typeof(T)];
            cache = new List<T>();
            TimerCallback callback = new TimerCallback(ClearUp);
            timer = new Timer(callback, null, 0, configuration.Timeout);
        }

        /// <summary>
        /// ��ȡһ���µ�ʵ��
        /// </summary>
        /// <param name="instance">���õĶ���ʵ��</param>
        /// <param name="increasable">�Ƿ���Լ������</param>
        /// <returns>�Ƿ�ɹ���ÿ��õ�ʵ��</returns>
        internal bool Acquire(out T item, out bool increasable)
        {
            increasable = cache.Count+1 >= configuration.Max ? false : true;
            item = null;

            // �޷���ȡ�������
            if (cache.Count <= 0)
                return false;
            

            // ���ü���ʵ��
            foreach (T cachedItem in cache)
                if ((cachedItem != null) && (cachedItem.Unoccupied))
                {
                    item = cachedItem;
                    return true;
                }


            return false;
        }

        /// <summary>
        /// �����µĻ���ʵ��
        /// </summary>
        /// <param name="item"></param>
        internal void Add(T item)
        {
            if ((cache.Count < configuration.Max) || (item != null))  // �Ѿ��ﵽ��������
                cache.Add(item);
        }

        /// <summary>
        /// ������δʹ�ö���
        /// </summary>
        private void ClearUp(object sender)
        {
            if (cache.Count <= 0) return;

            // ����
            List<int> toDeleteList = new List<int>();
            for(int i=0; i<cache.Count; i++)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(cache[i].AccessedTime);
                if (timeSpan.TotalMilliseconds > configuration.Timeout)
                    toDeleteList.Add(i);
            }

            // ����
            if (toDeleteList.Count <= 0) return;
            for (int i = toDeleteList.Count - 1; i >= 0; i--)
            {
                T item = cache[toDeleteList[i]];
                cache.Remove(item);
                item.Dispose();
                item = null;
            }
        }
    }
}
