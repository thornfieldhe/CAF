using System.Web;

namespace CAF
{
    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBase<T> where T : new()
    {
        protected SingletonBase() { }

        /// <summary>
        /// 默认实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (GenericContex.CheckWhetherIsWeb())
                {
                    var instance = (T)HttpContext.Current.Items[typeof(T).FullName];
                    if (instance == null)
                    {
                        instance = SinglentonCreator.instance;
                        HttpContext.Current.Items[typeof(T).FullName] = instance;
                    }
                    return instance;
                }
                else
                {
                    return SinglentonCreator.instance;
                }
            }
        }

        /// <summary>
        /// 创建一个实例
        /// </summary>
        private class SinglentonCreator
        {
            static SinglentonCreator() { }

            internal static readonly T instance = new T();
        }
    }
}