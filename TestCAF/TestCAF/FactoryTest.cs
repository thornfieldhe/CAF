using CAF;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCAF
{
    public class ProductA : IProduct
    {
        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        public IPlace Place { get; set; }

        public ProductA() { }

        public ProductA(string productName)
        {
            this.ProductName = productName;
        }
    }

    public class ProductB : IProduct
    {
        private string _productName;

        public string ProductName
        {
            get { return _productName = "this is B"; }
            set { _productName = value; }
        }

        public IPlace Place { get; set; }
    }

    public interface IProduct
    {
        string ProductName { get; }
    }

    public interface IPlace
    {
        string PlaceName { get; set; }
    }

    public class PlaceA : IPlace
    {
        private string _placeName;

        public string PlaceName { get { return "Hello"; } set { _placeName = value; } }
    }

    /// <summary>
    /// FactoryTest 的摘要说明
    /// </summary>
    [TestClass]
    public class FactoryTest
    {
        [TestMethod]
        public void TestFactory()
        {
            IProduct p = TypeCreater.BuildUp<IProduct>("a");
            Assert.AreEqual(p.ProductName, "zzzz");
            IProduct p2 = TypeCreater.BuildUp<IProduct>("p2", "b");
            Assert.AreEqual(p2.ProductName, "this is B");
        }
    }
}