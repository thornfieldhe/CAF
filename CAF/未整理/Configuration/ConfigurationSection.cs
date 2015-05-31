using System.Configuration;
namespace CAF.Configuration
{
    /// <summary>
    ///  �������ý�����
    /// </summary>
    public abstract class ConfigurationSectionBase : ConfigurationSection { }

    /// <summary>
    /// ���������ý�����
    /// </summary>
    public class ObjectBuilderConfigurationSection : ConfigurationSectionBase
    {
        private const string ObjectBuilderItem = "builders";

        /// <summary>
        /// ������
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
        /// �����
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
