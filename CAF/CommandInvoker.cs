using Microsoft.Practices.Unity;
using System.Collections.Generic;

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
            var section = SingletonBase<CAFConfiguration>.Instance.Unity;
            section.Configure(container, containerName);

            foreach (var type in section.Containers)
            {
                var item = container.Resolve<T>(type.Name);
                _plugins.Add(item);
            }
            return _plugins;
        }
    }
}
