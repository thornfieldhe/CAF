using System;
using System.Configuration;
namespace CAF.Configuration
{
    // 定义具有 name 和 description 属性的配置元素
    // name 属性作为 ConfigurationElementCollection中相应的 key
    public abstract class NamedConfigurationElementBase : ConfigurationElement
    {
        private const string NameProperty = "name";

        [ConfigurationProperty(NameProperty, IsKey = true, IsRequired = true)]
        public virtual string Name
        {
            get { return base[NameProperty] as string; }
        }
    }

    /// <summary>
    /// 创建者节构造顺序元素
    /// </summary>
    public class ObjectBuilderStepsConfrigurationElement : NamedConfigurationElementBase
    {
        private const string SequenceProperty = "seqence";
        private const string TimesProperty = "times";

        /// <summary>
        /// 构造次数
        /// </summary>
        [ConfigurationProperty(TimesProperty, IsRequired = false)]
        public int Times
        {
            get
            {
                var result = 1;
                int.TryParse(this[TimesProperty].ToString(), out result);
                return result;
            }
        }

        /// <summary>
        /// 构造顺序
        /// </summary>
        [ConfigurationProperty(SequenceProperty, IsRequired = true)]
        public virtual int Sequence
        {
            get
            {
                var result = 1;
                int.TryParse(base[SequenceProperty].ToString(), out result);
                return result;
            }
        }
    }

    /// <summary>
    /// 创建者节元素
    /// </summary>
    public class ObjectBuilderConfigurationElement : NamedConfigurationElementBase
    {
        private const string StepsItem = "steps";

        /// <summary>
        /// 依赖注入者
        /// </summary>
        [ConfigurationProperty(StepsItem, IsRequired = false)]
        public virtual BuilderStepsConfigurationElementCollection Steps
        {
            get
            {
                return base[StepsItem] as BuilderStepsConfigurationElementCollection;
            }
        }
    }

    /// <summary>
    /// 对象池元素
    /// </summary>
    public class PoolableConfigurationElement : NamedConfigurationElementBase
    {
        #region Private Field
        private const string MaxItem = "max";
        private const string TimeoutItem = "timeout";
        #endregion

        /// <summary>
        /// 对象池实例容量
        /// </summary>
        [ConfigurationProperty(MaxItem, IsRequired = true)]
        public int Max
        {
            get { return Convert.ToInt32(base[MaxItem]); }
        }

        /// <summary>
        /// 实例过期时间
        /// </summary>
        [ConfigurationProperty(TimeoutItem, IsRequired = true)]
        public int Timeout
        {
            get { return Convert.ToInt32(base[TimeoutItem]); }
        }
    }
}