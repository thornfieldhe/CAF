using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAF;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System;

namespace TestCAF
{
    /// <summary>
    /// TestComposite 的摘要说明
    /// </summary>
    [TestClass]
    public class CompositeTest
    {
        [TestMethod]
        public void TestIterating()
        {
            Department site = new Department();
            site.Name = "root"; site.Title = "level1";
            Department site1 = new Department();
            site1.Name = "dep1"; site1.Title = "level2";
            Department site2 = new Department();
            site2.Name = "dep2"; site2.Title = "level3";
            Department site3 = new Department();
            site3.Name = "dep3"; site3.Title = "level4";
            Employee leaf1 = new Employee();
            leaf1.Name = "stu1";
            Employee leaf2 = new Employee();
            leaf2.Name = "stu2"; leaf2.Age = 12;
            Employee leaf3 = new Employee();
            leaf3.Name = "stu3"; leaf3.Age = 15;
            Employee leaf4 = new Employee();
            leaf4.Name = "stu4";
            site.Add(leaf1);
            site.Add(leaf2);
            site.Add(site1);
            site1.Add(site2);
            site2.Add(leaf3);
            site2.Add(site3);
            site3.Add(leaf4);
            site2.Remove(site3);
            site.Save(@"c:\department.xml");

            List<Department> sites = site.Elements<Department>();
            List<Department> sites2 = site.Elements<Department>(ComponentSearchScope.OneLevel);
            List<Employee> emps = site.Elements<Employee>();
            Department d = Department.Load(@"c:\department.xml");
            
        }

        public class Employee : XmlLeaf
        {
            public Employee() : base("employee") { }
            public Employee(XElement xel) : base(xel) { }

            public int? Age
            {
                get
                {
                    return this.GetAttributeValueAsInt("age");
                }
                set
                {
                    SetAttributeValue("age", value);
                }
            }
        }

        public class Department : XmlComposite
        {
            public Department() : base("dep") { }
            public Department(XElement xel) : base(xel) { }
            public string Title
            {
                get
                {
                    return this.GetAttributeValueAsString("title");
                }
                set
                {
                    SetAttributeValue("title", value);
                }
            }

            protected override void BindTypies()
            {
                this.componentTypes.Add(typeof(Department));
                this.componentTypes.Add(typeof(Employee));
            }

            public static Department Load(string path)
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.Open);
                    XmlReader reader = new XmlTextReader(fs);
                    XDocument doc = XDocument.Load(reader);
                    return new Department(doc.Element(XName.Get("dep")));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void Save(string filePath)
            {
                try
                {
                    XDocument d = new XDocument();
                    d.Add(this.WElement);
                    d.Save(filePath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
