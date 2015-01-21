using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
namespace CAF.Configuration
{
    // 整个配置节组的对象, 包括 <delegating> 和 <generics> 两个配置节
    //  <sectionGroup name="marvellousWorks.practicalPattern.concept"/>
    internal class NamedConfigurationSectionGroup : ConfigurationSectionGroup
    {
        private const string UnityItem = "unity";
        private const string BuilderItem = "objectBuilder";
        private const string ChainItem = "chain";

        internal NamedConfigurationSectionGroup() : base() { }

        /// <summary>
        /// 依赖注入配置模块配置节
        /// </summary>
        [ConfigurationProperty(UnityItem, IsRequired = false)]
        public UnityConfigurationSection Unity
        {
            get { return this.Sections[UnityItem] as UnityConfigurationSection; }
        }

        /// <summary>
        /// 创建者模块配置节
        /// </summary>
        [ConfigurationProperty(BuilderItem, IsRequired = false)]
        internal virtual ObjectBuilderConfigurationSection ObjectBuilder
        {
            get { return base.Sections[BuilderItem] as ObjectBuilderConfigurationSection; }
        }
    }
}
