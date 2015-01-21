using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using CAF.Core;

namespace CAF
{

    /// <summary>
    /// 命令调用者
    /// </summary>
    public class CommandInvoker<T> 
    {
        protected List<T> _plugins;

        public CommandInvoker()
        {
            _plugins = new List<T>();
        }

        public List<T> GetPlugins(string containerName)
        {
            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection section = SingletonBase<CAFConfiguration>.Instance.Unity;
            section.Configure(container,containerName);

            foreach (ContainerElement type in section.Containers)
            {
                T item = container.Resolve<T>(type.Name);
                _plugins.Add(item);
            }
            return _plugins;
        }
    }
}
