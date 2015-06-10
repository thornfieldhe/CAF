using CAF.Core;
using System.Collections.Generic;
namespace CAF
{
    using CAF.Utility;

    public class BuilderBase
    {
        public BuilderBase()
        {
            DiscoveryBuildSteps();
        }
        /// <summary>
        /// 装配部分
        /// </summary>
        private void DiscoveryBuildSteps()
        {
            var methods = Reflection.GetMethodsWithCustomAttribute<BuildStepAttribute>(this.GetType());
            if (methods != null && methods.Count > 0)
            {
                var attributes = new BuildStepAttribute[methods.Count];
                var buildSetupAttributes = SingletonBase<CAFConfiguration>.Instance.ObjectBuilders;
                for (var i = 0; i < methods.Count; i++)
                {
                    var attribute = Reflection.GetMethodCustomAttribute<BuildStepAttribute>(methods[i]);
                    IList<BuildStepAttribute> steps = new List<BuildStepAttribute>();
                    buildSetupAttributes.TryGetValue(this.GetType(), out steps);
                    foreach (var item in steps)
                    {
                        if (item.Name == attribute.Name)
                        {
                            attribute = item;
                        }
                    }
                    attribute.Handler = methods[i];
                    attributes[i] = attribute;
                }
                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        for (var i = 0; i < attribute.Times; i++)
                        {
                            attribute.Handler.Invoke(this, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 卸载部分
        /// </summary>
        public virtual void TearDown() { }
    }
}
