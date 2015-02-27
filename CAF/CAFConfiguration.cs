using CAF.Configuration;
using CAF.Core;
using CAF.ObjectPool;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CAF
{
    public class CAFConfiguration : SingletonBase<CAFConfiguration>
    {
        private const string GroupName = "CAF";

        /// <summary>
        /// 缓冲的池化类型配置信息
        /// </summary>
        public IDictionary<Type, PoolableConfigurationElement> ObjectPoolCache { get; private set; }

        /// <summary>
        /// 依赖注入配置信息
        /// </summary>
        public UnityConfigurationSection Unity { get; private set; }

        /// <summary>
        /// 创建者配置信息
        /// </summary>
        public Dictionary<Type, IList<BuildStepAttribute>> ObjectBuilders { get; private set; }

        private ObjectBuilderConfigurationSection _objectBuilderElement;

        public CAFConfiguration()
        {
            System.Configuration.Configuration config;
            if (GenericContex.CheckWhetherIsWeb())
            {
                config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(@"~\web.config");
            }
            else
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            var group = (NamedConfigurationSectionGroup)config.GetSectionGroup(GroupName);

            //Unity
            Unity = group.Unity;

            //Builder
            _objectBuilderElement = group.ObjectBuilder;
            ObjectBuilders = new Dictionary<Type, IList<BuildStepAttribute>>();
            BindBuilder();

            //ObjectPool
            ObjectPoolCache = new Dictionary<Type, PoolableConfigurationElement>();
            foreach (PoolableConfigurationElement element in group.ObjectBuilder.ObjectPool)
            {
                var type = Type.GetType(Unity.TypeAliases[element.Name]);

                if (typeof(IPoolable).IsAssignableFrom(type))
                    ObjectPoolCache.Add(type, _objectBuilderElement.ObjectPool[element.Name]);
                else
                    throw new TypeInitializationException(typeof(IPoolable).FullName, null);
            }
        }

        private void BindBuilder()
        {
            foreach (ObjectBuilderConfigurationElement builder in _objectBuilderElement.Builders)
            {
                var type = Type.GetType(Unity.TypeAliases[builder.Name]);
                var attributes = new BuildStepAttribute[builder.Steps.Count];
                for (var i = 0; i < builder.Steps.Count; i++)
                {
                    attributes[i] = new BuildStepAttribute() { Sequence = builder.Steps[i].Sequence, Times = builder.Steps[i].Times, Name = builder.Steps[i].Name };
                }
                Array.Sort<BuildStepAttribute>(attributes);
                ObjectBuilders.Add(type, attributes);
            }
        }
    }
}