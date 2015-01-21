using System.Configuration;
namespace CAF.Configuration
{
    // ������� NamedConfigurationElementBase �� ConfiugrationElementCollection
    [ConfigurationCollection(typeof(NamedConfigurationElementBase), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public abstract class NamedConfigurationElementCollectionBase<T> : ConfigurationElementCollection
        where T : NamedConfigurationElementBase, new()
    {
        // �ⲿͨ�� index ��ȡ�������ض� configurationelement 
        public T this[int index]
        {
            get
            {
                return (T)base.BaseGet(index);
            }
        }
        public new T this[string name]
        {
            get
            {
                return (T)base.BaseGet(name);
            }
        }

        // ����һ���µ� NamedConfiugrationElement ʵ��
        protected override ConfigurationElement CreateNewElement() { return new T(); }

        // ��ȡ������ĳ���ض� NamedConfiugrationElement �� key (Name����)
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as T).Name;
        }

    }

    /// <summary>
    /// �����߽ڲ�������
    /// </summary>
    public class ObjectBuilderConfigurationElementCollection :
        NamedConfigurationElementCollectionBase<ObjectBuilderConfigurationElement> { }

    /// <summary>
    /// �����߽ڹ���˳�򼯺�
    /// </summary>
    public class BuilderStepsConfigurationElementCollection :
        NamedConfigurationElementCollectionBase<ObjectBuilderStepsConfrigurationElement> { }

    /// <summary>
    /// ����ؽڼ���
    /// </summary>
    public class PoolableConfigurationElementCollection :
        NamedConfigurationElementCollectionBase<PoolableConfigurationElement> { }
}
