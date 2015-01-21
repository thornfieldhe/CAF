using System;
using System.Collections.Generic;
using System.Xml.Linq;
namespace CAF
{
    /// <summary>
    /// ���ģʽ
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
        /// �����ӽڵ�
        /// </summary>
        public List<XmlComponent> Children { get; private set; }

        /// <summary>
        /// �ڵ�����
        /// </summary>
        public virtual string TagName
        {
            get { return this.WElement.Name.ToString(); }
        }

        /// <summary>
        /// �ڵ�Name����
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
        /// �������л����xmlԪ��
        /// </summary>
        public XElement WElement;

        /// <summary>
        /// �������Ӧ�ð�����������
        /// </summary>
        protected List<Type> componentTypes = new List<Type>();

        protected virtual void BindTypies() { }

        /// <summary>
        /// ��ʵֻ��Composite���Ͳ���Ҫ����ʵ�ֵĹ���
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
        /// ��ȡ���󼯺�
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        public virtual List<T> Elements<T>() where T : XmlComponent
        {
            IEnumerable<T> lists = Enumerate<T>();
            return new List<T>(lists);
        }

        /// <summary>
        /// ��ȡ���󼯺�
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="searchScope">������Χ</param>
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
        /// ��ȡָ�����ƶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        ///<param name="name">����Name��������</param>
        public virtual T Element<T>(string name) where T : XmlComponent
        {
            IEnumerable<T> lists = GetComponentByName<T>(name);
            return new List<T>(lists)[0];
        }

        public override string ToString()
        {
            return WElement.ToString();
        }

        #region ˽�з���

        /// <summary>
        /// ��ȡ����Ϊstring�Ļ�������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
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
        /// ��ȡ����Ϊint�Ļ�������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
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
        /// ��ȡ����Ϊguid�Ļ�������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
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
        /// ��ȡ����Ϊbool�Ļ�������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
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
        /// ��ȡ����Ϊdatetime�Ļ�������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
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
        /// ��ȡ����Ϊdouble�Ļ�������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
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
        /// ��ȡ����Ϊdecimal�Ļ�������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
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
        /// ��������ֵ
        /// </summary>
        /// <param name="attributeName">������������</param>
        /// <returns></returns>
        protected void SetAttributeValue(string attributeName, object value)
        {
            this.WElement.SetAttributeValue(attributeName.ToLower(), value.ToString());
        }

        /// <summary>
        /// ʵ�ֵ����������Ҷ���������ʵ�����Եݹ�
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
        /// ����ֱ���Ӷ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
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
        /// ����ֱ���Ӷ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="name">component����</param>
        /// <returns></returns>
        private IEnumerable<T> GetComponentByName<T>(string name) where T : XmlComponent
        {
            if ((Children != null) && (Children.Count > 0))
                foreach (XmlComponent child in Children)
                    if (IsMatch<T>(child) && child.Name == name)
                        yield return child as T;
        }

        /// <summary>
        /// �ж϶����Ƿ���ָ������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="target">�Ƚ϶���</param>
        private bool IsMatch<T>(XmlComponent target) where T : XmlComponent
        {
            if (target == null) return false;
            return (target.GetType() == typeof(T)) ? true : false;
        }

        /// <summary>
        /// ���ӳ�Ա
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
    /// Ҷ
    /// </summary>
    public class XmlLeaf : XmlComponent
    {
        public XmlLeaf(string name) : base(name) { }

        public XmlLeaf(XElement xelement) : base(xelement) { }

        /// <summary>
        /// ��ȷ������֧�ִ������
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
    /// ��
    /// </summary>
    public class XmlComposite : XmlComponent
    {
        public XmlComposite(string name) : base(name) { }

        public XmlComposite(XElement xelement) : base(xelement) { }
    }

    /// <summary>
    ///  ָ��ʹ�� Component ����ִ�е�Ŀ¼�����Ŀ��ܷ�Χ��
    /// </summary>
    public enum ComponentSearchScope
    {
        /// <summary>
        /// �����������ֱ���Ӷ��󣬵�������������
        /// </summary>
        OneLevel = 0,

        /// <summary>
        /// �������������������������������Ӷ���
        /// </summary>
        Subtree = 1,
    }
}
