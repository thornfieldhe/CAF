using System.Configuration;
namespace CAF.Configuration
{
    /// <summary>
    ///  基本配置节类型
    /// </summary>
    public abstract class ConfigurationSectionBase : ConfigurationSection { }

    /// <summary>
    /// 创建者配置节类型
    /// </summary>
    public class ObjectBuilderConfigurationSection : ConfigurationSectionBase
    {
        private const string ObjectBuilderItem = "builders";

        /// <summary>
        /// 创建者
        /// </summary>
        [ConfigurationProperty(ObjectBuilderItem, IsRequired = false)]
        public virtual ObjectBuilderConfigurationElementCollection Builders
        {
            get
            {
                return base[ObjectBuilderItem] as ObjectBuilderConfigurationElementCollection;
            }
        }

        private const string ObjectPoolItem = "objectPool";

        /// <summary>
        /// 对象池
        /// </summary>
        [ConfigurationProperty(ObjectPoolItem, IsRequired = false)]
        public virtual PoolableConfigurationElementCollection ObjectPool
        {
            get
            {
                return base[ObjectPoolItem] as PoolableConfigurationElementCollection;
            }
        }
    }
}
