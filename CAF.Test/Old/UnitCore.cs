﻿
namespace CAF.Test
{
    using CAF.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitCore
    {
        /// <summary>
        /// 创建者模式
        /// </summary>
        [TestMethod]
        public void TestBuilder()
        {
            var product = new Product();

            Assert.AreEqual(product.Name, ",1,1,2,3");
            product.TearDown();
            Assert.AreEqual("", product.Name);
        }

        /// <summary>
        /// 工厂模式
        /// </summary>
        [TestMethod]
        public void TestFactory()
        {
            var p = TypeCreater.BuildUp<IProduct>("a");
            Assert.AreEqual(p.ProductName, "zzzz");
            var p2 = TypeCreater.BuildUp<IProduct>("p2", "b");
            Assert.AreEqual(p2.ProductName, "this is B");
        }
    }

    public class Product : BuilderBase
    {
        public string Name { get; set; }

        [BuildStep("step1")]
        public void Do1() { Name += ",1"; }
        [BuildStep("step2")]
        public void Do2() { Name += ",2"; }
        [BuildStep(3, 1)]
        public void Do3() { Name += ",3"; }

        public override void TearDown()
        {
            this.Name = "";
        }
    }

    #region 工厂模式
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
    #endregion
}
