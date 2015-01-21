using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAF.Core;
using CAF;

namespace TestCAF
{
    /// <summary>
    /// MementoTest 的摘要说明
    /// </summary>
    [TestClass]
    public class MementoTest
    {
        public MementoTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试属性
        //
        // 编写测试时，还可使用以下附加属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void TestMemento()
        //{
        //    Orginator or = new Orginator();
        //    or.SaveCheckpoint();
        //    or.NewFruit();
        //    or.SaveCheckpoint();
        //    Assert.AreEqual(1, or.Current.Count);
        //    or.NewFruit2();
        //    Assert.AreEqual("Banana", or.Current.Type);

        //    or.Undo();
        //    Assert.AreEqual("Apple", or.Current.Type);
        //}
    }

    [Serializable]
    public class Fruit
    {
        public int Count { get; set; }
        public string Type { get; set; }
    }

    public class Orginator : OriginatorBase<Fruit>
    {
        public Orginator() { }
        public void NewFruit() 
        {
            State.Count = 1;
            State.Type = "Apple";
        }

        public void NewFruit2()
        {
            State.Count = 10;
            State.Type = "Banana";
        }

        public Fruit Current { get { return State; } }
    }
}
