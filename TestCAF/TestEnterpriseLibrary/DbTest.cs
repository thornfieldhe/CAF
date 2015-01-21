using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAF;
using CAF.Data;
using System.Data;
namespace TestCAF.TestEnterpriseLibrary
{
    /// <summary>
    /// DbTest 的摘要说明
    /// </summary>
    [TestClass]
    public class DbTest
    {
        public DbTest() { }

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

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    BussinessProduct p = BussinessProduct.GetTop1Product();
        //    Assert.AreEqual("Chai", p.ProductName);
        //}
    }

    public class BussinessProduct
    {
        [DbProperty("ProductID")]
        public int ProductID { get; set; }

        [DbProperty("ProductName")]
        public string ProductName { get; set; }

        public BussinessProduct() { }

        public static BussinessProduct GetTop1Product()
        {
            return SingletonBase<EntityProvider>.Instance.GetTop1Product();
        }
    }

    public class EntityProvider : EntityMapBase
    {
        private SqlDataPortal portal = new SqlDataPortal();
        public BussinessProduct GetTop1Product()
        {
            BussinessProduct p = new BussinessProduct();
            return base.Load<BussinessProduct>(portal.GetReaderBySP("CustOrdersDetail",10248))[0];
        }
    }
}
