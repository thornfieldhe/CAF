using System.Configuration;
namespace CAF.Configuration
{
    // 定义包括 NamedConfigurationElementBase 的 ConfiugrationElementCollection
    [ConfigurationCollection(typeof(NamedConfigurationElementBase), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public abstract class NamedConfigurationElementCollectionBase<T> : ConfigurationElementCollection
        where T : NamedConfigurationElementBase, new()
    {
        // 外部通过 index 获取集合中特定 configurationelement 
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

        // 创建一个新的 NamedConfiugrationElement 实例
        protected override ConfigurationElement CreateNewElement() { return new T(); }

        // 获取集合中某个特定 NamedConfiugrationElement 的 key (Name属性)
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as T).Name;
        }

    }

    /// <summary>
    /// 创建者节参数集合
    /// </summary>
    public class ObjectBuilderConfigurationElementCollection :
        NamedConfigurationElementCollectionBase<ObjectBuilderConfigurationElement> { }

    /// <summary>
    /// 创建者节构造顺序集合
    /// </summary>
    public class BuilderStepsConfigurationElementCollection :
        NamedConfigurationElementCollectionBase<ObjectBuilderStepsConfrigurationElement> { }

    /// <summary>
    /// 对象池节集合
    /// </summary>
    public class PoolableConfigurationElementCollection :
        NamedConfigurationElementCollectionBase<PoolableConfigurationElement> { }
}
