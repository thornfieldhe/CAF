namespace CAF.ObjectPool
{
    /// <summary>
    /// ����ص�����
    /// </summary>
    internal static class ObjectDispatch
    {
        /// <summary>
        /// �Ƿ���Զ������
        /// </summary>
        private static bool available = true;

        /// <summary>
        /// ����ÿ������ʵ����ʹ���������ȡָ�����͵Ķ���ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T Acquire<T>()
            where T : PoolableBase, new()
        {
            available = false;
            T item = null;
            bool increasable;

            var cache = ObjectCache.Instance;
            if (!(cache.TryToAcquire<T>(out item, out increasable)))
            {
                if (increasable)
                {
                    item = TypeCreater.BuildUp<T>();
                    cache.Cache<T>(item);
                }
            }

            available = true;
            return item;
        }

        /// <summary>
        /// �Ƿ���Զ������
        /// </summary>
        internal static bool Available
        {
            get { return available; }
        }
    }
}
