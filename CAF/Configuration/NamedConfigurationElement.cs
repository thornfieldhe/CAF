using System;
using System.Configuration;
namespace CAF.Configuration
{
    // ������� name �� description ���Ե�����Ԫ��
    // name ������Ϊ ConfigurationElementCollection����Ӧ�� key
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
    /// �����߽ڹ���˳��Ԫ��
    /// </summary>
    public class ObjectBuilderStepsConfrigurationElement : NamedConfigurationElementBase
    {
        private const string SequenceProperty = "seqence";
        private const string TimesProperty = "times";

        /// <summary>
        /// �������
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
        /// ����˳��
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
    /// �����߽�Ԫ��
    /// </summary>
    public class ObjectBuilderConfigurationElement : NamedConfigurationElementBase
    {
        private const string StepsItem = "steps";

        /// <summary>
        /// ����ע����
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
    /// �����Ԫ��
    /// </summary>
    public class PoolableConfigurationElement : NamedConfigurationElementBase
    {
        #region Private Field
        private const string MaxItem = "max";
        private const string TimeoutItem = "timeout";
        #endregion

        /// <summary>
        /// �����ʵ������
        /// </summary>
        [ConfigurationProperty(MaxItem, IsRequired = true)]
        public int Max
        {
            get { return Convert.ToInt32(base[MaxItem]); }
        }

        /// <summary>
        /// ʵ������ʱ��
        /// </summary>
        [ConfigurationProperty(TimeoutItem, IsRequired = true)]
        public int Timeout
        {
            get { return Convert.ToInt32(base[TimeoutItem]); }
        }
    }
}