using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CAF.Core;
namespace CAF
{
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
            IList<MethodInfo> methods = AttributeHelper.GetMethodsWithCustomAttribute<BuildStepAttribute>(this.GetType());
            if (methods != null && methods.Count > 0)
            {
                BuildStepAttribute[] attributes = new BuildStepAttribute[methods.Count];
                Dictionary<Type, IList<BuildStepAttribute>> buildSetupAttributes = SingletonBase<CAFConfiguration>.Instance.ObjectBuilders;
                for (int i = 0; i < methods.Count; i++)
                {
                    BuildStepAttribute attribute = AttributeHelper.GetMethodCustomAttribute<BuildStepAttribute>(methods[i]);
                    IList<BuildStepAttribute> steps = new List<BuildStepAttribute>();
                    buildSetupAttributes.TryGetValue(this.GetType(), out steps);
                    foreach (BuildStepAttribute item in steps)
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
                    foreach (BuildStepAttribute attribute in attributes)
                    {
                        for (int i = 0; i < attribute.Times; i++)
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
