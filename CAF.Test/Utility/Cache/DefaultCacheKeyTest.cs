
namespace CAF.Tests.Cache
{
    using CAF.Caches;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DefaultCacheKeyTest
    {
        private DefaultCacheKey _key;
        [TestInitialize]
        public void TestInit() { this._key = new DefaultCacheKey(); }
        [TestMethod]
        public void TestKey()
        {
            Assert.AreEqual("CacheKey_a", this._key.GetKey("a"));
        }
    }
}
