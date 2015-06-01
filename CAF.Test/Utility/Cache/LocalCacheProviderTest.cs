using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CAF.Tests.Cache
{
    using CAF.Caches;
    using CAF.Model;
    using CAF.Utility;

    /// <summary>
    /// 本地缓存提供程序测试
    /// </summary>
    [TestClass]
    public class LocalCacheProviderTest
    {
        /// <summary>
        /// 本地缓存提供程序
        /// </summary>
        private LocalCacheProvider _cache;
        /// <summary>
        /// 测试对象
        /// </summary>
        private User _user;
        /// <summary>
        /// 键
        /// </summary>
        private string _key;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._cache = new LocalCacheProvider();
            this._user = new User { Name = "A"};
            this._key = "a";
        }

        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void TestClear()
        {
            this._cache.Clear();
        }

        /// <summary>
        /// 测试添加缓存，验证key
        /// </summary>
        [TestMethod]
        public void TestValidateKey()
        {
            this._cache.Remove(null);
            this._cache.Add(null, this._user, 10);
            Assert.IsNull(this._cache.Get<User>(null), "null");
            this._cache.Add("", this._user, 10);
            Assert.IsNull(this._cache.Get<User>(""));
        }

        /// <summary>
        /// 测试添加缓存
        /// </summary>
        [TestMethod]
        public void TestAdd()
        {
            Assert.IsNull(this._cache.Get<User>(this._key));
            this._cache.Add(this._key, this._user, 10);
            Assert.IsNotNull(this._cache.Get<User>(this._key));
            Assert.AreEqual("A", this._cache.Get<User>(this._key).Name);
        }

        /// <summary>
        /// 测试添加缓存，对key进行过滤
        /// </summary>
        [TestMethod]
        public void TestAdd_FilterKey()
        {
            const string key2 = "A ";
            this._cache.Add(key2, this._user, 10);
            Assert.IsNotNull(this._cache.Get<User>(key2));
            Assert.AreEqual("A", this._cache.Get<User>(this._key).Name);
        }

        /// <summary>
        /// 测试修改
        /// </summary>
        [TestMethod]
        public void TestUpdate()
        {
            const string key2 = "A ";
            this._cache.Add(this._key, this._user, 10);
            var user = new User { Name = "B" };
            this._cache.Update(null, user, 10);
            this._cache.Update(key2, user, 10);
            Assert.AreEqual("B", this._cache.Get<User>(this._key).Name);
        }

        /// <summary>
        /// 测试修改时间
        /// </summary>
        [TestMethod]
        [Ignore]
        public void TestUpdate_Time()
        {
            Assert.IsNull(this._cache.Get<User>(this._key));
            this._cache.Add(this._key, this._user, 1);
            Thread.Sleep(500);
            Assert.IsNotNull(this._cache.Get<User>(this._key));
            this._cache.Update(this._key, new User { Name = "C" }, 1);
            Thread.Sleep(800);
            Assert.IsNotNull(this._cache.Get<User>(this._key));
            Assert.AreEqual("C", this._cache.Get<User>(this._key).Name);
            this._cache.Update(this._key, new User { Name = "C" }, 1);
            Thread.Sleep(1200);
            Assert.IsNull(this._cache.Get<User>(this._key));
        }

        /// <summary>
        /// 测试移除
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            const string keyB = "B";
            this._cache.Add(this._key, this._user, 10);
            this._cache.Add(keyB, this._user, 10);
            Assert.IsNotNull(this._cache.Get<User>(this._key));
            Assert.IsNotNull(this._cache.Get<User>(keyB));

            this._cache.Remove("A ");
            Assert.IsNull(this._cache.Get<User>(this._key));
            Assert.IsNotNull(this._cache.Get<User>(keyB));
        }
    }
}
