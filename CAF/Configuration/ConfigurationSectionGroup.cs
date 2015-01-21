using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
namespace CAF.Configuration
{
    // �������ý���Ķ���, ���� <delegating> �� <generics> �������ý�
    //  <sectionGroup name="marvellousWorks.practicalPattern.concept"/>
    internal class NamedConfigurationSectionGroup : ConfigurationSectionGroup
    {
        private const string UnityItem = "unity";
        private const string BuilderItem = "objectBuilder";
        private const string ChainItem = "chain";

        internal NamedConfigurationSectionGroup() : base() { }

        /// <summary>
        /// ����ע������ģ�����ý�
        /// </summary>
        [ConfigurationProperty(UnityItem, IsRequired = false)]
        public UnityConfigurationSection Unity
        {
            get { return this.Sections[UnityItem] as UnityConfigurationSection; }
        }

        /// <summary>
        /// ������ģ�����ý�
        /// </summary>
        [ConfigurationProperty(BuilderItem, IsRequired = false)]
        internal virtual ObjectBuilderConfigurationSection ObjectBuilder
        {
            get { return base.Sections[BuilderItem] as ObjectBuilderConfigurationSection; }
        }
    }
}
