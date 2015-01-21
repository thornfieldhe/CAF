using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAF;
using CAF.Core;

namespace TestCAF
{
    /// <summary>
    /// BuilderTest 的摘要说明
    /// </summary>
    [TestClass]
    public class BuilderTest
    {
        public BuilderTest()
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

        [TestMethod]
        public void TestBuilder()
        {
            Product product = new Product();
            
            Assert.AreEqual(product.Name, ",1,1,2,3");
            product.TearDown();
            Assert.AreEqual("", product.Name);
        }
    }

    public class Product : BuilderBase
    {
        public string Name { get; set; }

        [BuildStep("step1")]
        public void Do1() { Name += ",1"; }
        [BuildStep("step2")]
        public void Do2() { Name += ",2"; }
        [BuildStep(3,1)]
        public void Do3() { Name += ",3"; }

        public override void TearDown()
        {
            this.Name = "";
        }
    }
}
