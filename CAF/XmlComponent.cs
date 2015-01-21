using System;
using System.Collections.Generic;
using System.Xml.Linq;
namespace CAF
{
    /// <summary>
    /// 组合模式
    /// </summary>
    public abstract class XmlComponent
    {
        public XmlComponent(string name)
            : base()
        {
            this.WElement = new XElement(XName.Get(name));
        }

        public XmlComponent(XElement xelement)
            : base()
        {
            BindTypies();
            this.WElement = xelement;
            BindChildren(xelement);
        }

        /// <summary>
        /// 保存子节点
        /// </summary>
        public List<XmlComponent> Children { get; private set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public virtual string TagName
        {
            get { return this.WElement.Name.ToString(); }
        }

        /// <summary>
        /// 节点Name属性
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetAttributeValueAsString("name");
            }
            set { this.SetAttributeValue("name", value.ToString()); }
        }

        /// <summary>
        /// 基本序列化后的xml元素
        /// </summary>
        public XElement WElement;

        /// <summary>
        /// 该组件中应该包含的子类型
        /// </summary>
        protected List<Type> componentTypes = new List<Type>();

        protected virtual void BindTypies() { }

        /// <summary>
        /// 其实只有Composite类型才需要真正实现的功能
        /// </summary>
        /// <param name="child"></param>
        public virtual void Add(XmlComponent child)
        {
            if (Children == null)
            {
                Children = new List<XmlComponent>();
            }
            Children.Add(child);
            this.WElement.Add(child.WElement);
        }

        public virtual string Value { get { return this.WElement.Value; } set { this.WElement.Value = value; } }

        public virtual void Remove(XmlComponent child)
        {
            Children.Remove(child);
            foreach (XElement item in this.WElement.Elements(XName.Get(child.TagName)))
            {
                if (!string.IsNullOrEmpty(child.TagName) && item.Name == child.TagName)
                {
                    item.Remove();
                }
            }
        }

        public virtual XmlComponent this[int index] { get { return Children[index]; } }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        public virtual List<T> Elements<T>() where T : XmlComponent
        {
            IEnumerable<T> lists = Enumerate<T>();
            return new List<T>(lists);
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="searchScope">搜索范围</param>
        public virtual List<T> Elements<T>(ComponentSearchScope searchScope) where T : XmlComponent
        {
            if (searchScope == ComponentSearchScope.Subtree)
            {
                return Elements<T>();
            }
            else
            {
                IEnumerable<T> lists = GetOneLevelMember<T>();
                return new List<T>(lists);
            }
        }

        /// <summary>
        /// 获取指定名称对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        ///<param name="name">对象Name属性名称</param>
        public virtual T Element<T>(string name) where T : XmlComponent
        {
            IEnumerable<T> lists = GetComponentByName<T>(name);
            return new List<T>(lists)[0];
        }

        public override string ToString()
        {
            return WElement.ToString();
        }

        #region 私有方法

        /// <summary>
        /// 获取类型为string的基本属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected string GetAttributeValueAsString(string attributeName)
        {
            string result = null;
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                result = this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value;
            }
            return result;
        }

        /// <summary>
        /// 获取类型为int的基本属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected int? GetAttributeValueAsInt(string attributeName)
        {
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                return int.Parse(this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取类型为guid的基本属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected Guid? GetAttributeValueAsGuid(string attributeName)
        {
            Guid result = new Guid();
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                try
                {
                    result = new Guid(this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value);
                }
                catch
                {
                    result = new Guid();
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取类型为bool的基本属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected bool? GetAttributeValueAsBool(string attributeName)
        {
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                return bool.Parse(this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取类型为datetime的基本属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected DateTime? GetAttributeValueAsDateTime(string attributeName)
        {
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                return DateTime.Parse(this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取类型为double的基本属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected double? GetAttributeValueAsDouble(string attributeName)
        {
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                return double.Parse(this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取类型为decimal的基本属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected decimal? GetAttributeValueAsDecimal(string attributeName)
        {
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                return decimal.Parse(this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value);
            }
            else
            {
                return null;
            }
        }

        protected object GetAttributeValueAsDouble(string attributeName, Type enumType)
        {
            if (this.WElement.Attribute(attributeName.ToLower()) != null)
            {
                string value = this.WElement.Attribute(XName.Get(attributeName.ToLower())).Value;
                return Enum.Parse(enumType, value);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="attributeName">基本属性名称</param>
        /// <returns></returns>
        protected void SetAttributeValue(string attributeName, object value)
        {
            this.WElement.SetAttributeValue(attributeName.ToLower(), value.ToString());
        }

        /// <summary>
        /// 实现迭代器，并且对容器对象实现隐性递归
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<T> Enumerate<T>() where T : XmlComponent
        {
            if (IsMatch<T>(this))
                yield return this as T;
            if ((Children != null) && (Children.Count > 0))
                foreach (XmlComponent child in Children)
                    foreach (XmlComponent item in child.Enumerate<T>())
                        if (IsMatch<T>(item))
                            yield return item as T;
        }

        /// <summary>
        /// 遍历直接子对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        private IEnumerable<T> GetOneLevelMember<T>() where T : XmlComponent
        {
            if (IsMatch<T>(this))
                yield return this as T;
            if ((Children != null) && (Children.Count > 0))
                foreach (XmlComponent child in Children)
                    if (IsMatch<T>(child))
                        yield return child as T;
        }

        /// <summary>
        /// 遍历直接子对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">component名称</param>
        /// <returns></returns>
        private IEnumerable<T> GetComponentByName<T>(string name) where T : XmlComponent
        {
            if ((Children != null) && (Children.Count > 0))
                foreach (XmlComponent child in Children)
                    if (IsMatch<T>(child) && child.Name == name)
                        yield return child as T;
        }

        /// <summary>
        /// 判断对象是否是指定类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="target">比较对象</param>
        private bool IsMatch<T>(XmlComponent target) where T : XmlComponent
        {
            if (target == null) return false;
            return (target.GetType() == typeof(T)) ? true : false;
        }

        /// <summary>
        /// 绑定子成员
        /// </summary>
        /// <param name="xel"></param>
        private void BindChildren(XElement xel)
        {
            foreach (Type item in componentTypes)
            {
                if (item.IsSubclassOf(typeof(XmlComponent)))
                {
                    XmlComponent component = Activator.CreateInstance(item) as XmlComponent;

                    List<XElement> eles = new List<XElement>(xel.Elements(component.TagName));
                    foreach (XElement xe in eles)
                    {
                        try
                        {
                            if (Children == null)
                            {
                                Children = new List<XmlComponent>();
                            }
                            this.Children.Add(Activator.CreateInstance(item, xe) as XmlComponent);
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// 叶
    /// </summary>
    public class XmlLeaf : XmlComponent
    {
        public XmlLeaf(string name) : base(name) { }

        public XmlLeaf(XElement xelement) : base(xelement) { }

        /// <summary>
        /// 明确声明不支持此类操作
        /// </summary>
        /// <param name="child"></param>
        public override void Add(XmlComponent child) { throw new NotSupportedException(); }
        public override void Remove(XmlComponent child) { throw new NotSupportedException(); }
        public override XmlComponent this[int index] { get { throw new NotSupportedException(); } }
        public override T Element<T>(string name) { throw new NotSupportedException(); }
        public override List<T> Elements<T>() { throw new NotSupportedException(); }
        public override List<T> Elements<T>(ComponentSearchScope searchScope) { throw new NotSupportedException(); }

    }

    /// <summary>
    /// 干
    /// </summary>
    public class XmlComposite : XmlComponent
    {
        public XmlComposite(string name) : base(name) { }

        public XmlComposite(XElement xelement) : base(xelement) { }
    }

    /// <summary>
    ///  指定使用 Component 对象执行的目录搜索的可能范围。
    /// </summary>
    public enum ComponentSearchScope
    {
        /// <summary>
        /// 搜索基对象的直接子对象，但不搜索基对象。
        /// </summary>
        OneLevel = 0,

        /// <summary>
        /// 搜索整个子树，包括基对象及其所有子对象。
        /// </summary>
        Subtree = 1,
    }
}
