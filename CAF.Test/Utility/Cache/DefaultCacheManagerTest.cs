using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Cache
{
    using CAF.Caches;
    using CAF.Model;
    using CAF.Test.Samples;
    using CAF.Utility;

    using NSubstitute;

    [TestClass]
    public class DefaultCacheManagerTest
    {
        #region 测试初始化

        /// <summary>
        /// 默认缓存管理器
        /// </summary>
        private DefaultCacheManager _manager;
        /// <summary>
        /// 模拟测试仓储1
        /// </summary>
        private ITest3Repository _mockRepository;
        /// <summary>
        /// 模拟缓存提供程序
        /// </summary>
        private ICacheProvider _mockCacheProvider;

        /// <summary>
        /// 键
        /// </summary>
        private string _key;

        /// <summary>
        /// 实际缓存键
        /// </summary>
        private string _cacheKey;

        /// <summary>
        /// 缓存过期标记键
        /// </summary>
        private string _signKey;

        /// <summary>
        /// 测试对象
        /// </summary>
        private User _testA;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._mockCacheProvider = Substitute.For<ICacheProvider>();
            this._manager = new DefaultCacheManager(this._mockCacheProvider, new DefaultCacheKey());
            this._mockRepository = Substitute.For<ITest3Repository>();
            this._key = "a";
            this._cacheKey = "CacheKey_a";
            this._signKey = "CacheKey_Sign_a";
            this._testA = new User(){Id=Guid.NewGuid(),Name = "A"};

        }

        #endregion

        #region Get(获取缓存)

        /// <summary>
        /// 测试首次获取缓存，从仓储加载，并添加到缓存
        /// </summary>
        [TestMethod]
        public void TestGet_First()
        {
            //设置仓储操作
            this._mockRepository.GetTest3().Returns(this._testA);
            //设置仅当调用了添加缓存方法后，才能获取
            this._mockCacheProvider.When(t => t.Add(this._cacheKey, this._testA, 20))
                .Do(invocation => this._mockCacheProvider.Get<User>(this._cacheKey).Returns(this._testA));
            this._mockCacheProvider.When(t => t.Add(this._signKey, "a", 10))
                .Do(invocation => this._mockCacheProvider.Get<string>(this._signKey).Returns("a"));

            //首次读取缓存
            var result0=this._manager.Get(this._key, () => this._mockRepository.GetTest3(), 10);
            //第二次读取缓存
            var result = this._manager.Get(this._key, () => this._mockRepository.GetTest3(), 10);

            //验证仓储只被调用一次，说明第二次从缓存中读取
            this._mockRepository.Received(1).GetTest3();
            //验证缓存只被添加一次
            this._mockCacheProvider.Received(1).Add(this._cacheKey, this._testA, 20);
            //验证结果
            Assert.AreEqual(result,result0);
            Assert.IsNotNull(result);
            Assert.AreEqual("A", result.Name);
        }

        /// <summary>
        /// 测试第二次获取缓存，不再读取仓储
        /// </summary>
        [TestMethod]
        public void TestGet_Second()
        {
            //设置仓储操作
            this._mockRepository.GetTest3().Returns(this._testA);
            //设置仅当调用了添加缓存方法后，才能获取
            this._mockCacheProvider.When(t => t.Add(this._cacheKey, this._testA, 20))
                .Do(invocation => this._mockCacheProvider.Get<User>(this._cacheKey).Returns(this._testA));
            this._mockCacheProvider.When(t => t.Add(this._signKey, "a", 10))
                .Do(invocation => this._mockCacheProvider.Get<string>(this._signKey).Returns("a"));

            //首次读取缓存
            this._manager.Get(this._key, () => this._mockRepository.GetTest3(), 10);
            //第二次读取缓存
            var result = this._manager.Get(this._key, () => this._mockRepository.GetTest3(), 10);

            //验证仓储只被调用一次，说明第二次从缓存中读取
            this._mockRepository.Received(1).GetTest3();
            //验证缓存只被添加一次
            this._mockCacheProvider.Received(1).Add(this._cacheKey, this._testA, 20);
            //验证结果
            Assert.IsNotNull(result);
            Assert.AreEqual("A", result.Name);
        }

        /// <summary>
        /// 测试并发更新
        /// </summary>
        [TestMethod]
        public void TestGet_Concurrency()
        {
            //设置仓储操作
            this._mockRepository.GetTest3().Returns(this._testA);
            //设置仅当调用了添加缓存方法后，才能获取
            this._mockCacheProvider.When(t => t.Add(this._cacheKey, this._testA, 20))
                .Do(invocation => this._mockCacheProvider.Get<User>(this._cacheKey).Returns(this._testA));
            this._mockCacheProvider.When(t => t.Add(this._signKey, "a", 10))
                .Do(invocation => this._mockCacheProvider.Get<string>(this._signKey).Returns("a"));

            //并发读取缓存
            UnitTest.TestConcurrency(() =>
            {
                var manager = new DefaultCacheManager(this._mockCacheProvider, new DefaultCacheKey());
                manager.Get(this._key, () => this._mockRepository.GetTest3(), 10);
            }, 5);

            //验证仓储只被调用一次，说明同时只有一个线程能更新缓存
            this._mockRepository.Received(1).GetTest3();
            //验证缓存只被添加一次
            this._mockCacheProvider.Received(1).Add(this._cacheKey, this._testA, 20);
        }

        /// <summary>
        /// 已缓存,当缓存标记过期，重新更新缓存
        /// </summary>
        [TestMethod]
        public void TestGet_Concurrency2()
        {
            //模拟已加载缓存
            this._mockCacheProvider.Get<User>(this._cacheKey).Returns(this._testA);
            //模拟缓存过期标记已到期
            this._mockCacheProvider.Get<string>(this._signKey).Returns("");
            //添加缓存过期标记后，返回"a"
            this._mockCacheProvider.When(t => t.Add(this._signKey, "a", 10))
                .Do(invocation => this._mockCacheProvider.Get<string>(this._signKey).Returns("a"));

            //并发读取缓存
            UnitTest.TestConcurrency(() =>
            {
                var manager = new DefaultCacheManager(this._mockCacheProvider, new DefaultCacheKey());
                manager.Get(this._key, () => this._mockRepository.GetTest3(), 10);
            }, 5);

            //验证添加缓存过期标记1次
            this._mockCacheProvider.Received(1).Add(this._signKey, "a", 10);
            //验证不会调用添加缓存
            this._mockCacheProvider.DidNotReceive().Add(this._cacheKey, this._testA, 20);
            //验证缓存在过期时被更新
            this._mockCacheProvider.Received(1).Update(this._cacheKey, Arg.Any<object>(), 20);
        }

        /// <summary>
        /// 测试缓存过期时间单位为分
        /// </summary>
        [TestMethod]
        public void TestGetByMinutes()
        {
            //设置仓储操作
            this._mockRepository.GetTest3().Returns(this._testA);
            //读取缓存
            this._manager.GetByMinutes(this._key, () => this._mockRepository.GetTest3(), 1);
            //验证读取和添加缓存
            this._mockCacheProvider.Received().Add(this._cacheKey, this._testA, 120);
        }

        /// <summary>
        /// 测试缓存过期时间单位为小时
        /// </summary>
        [TestMethod]
        public void TestGetByHours()
        {
            //设置仓储操作
            this._mockRepository.GetTest3().Returns(this._testA);
            //读取缓存
            this._manager.GetByHours(this._key, () => this._mockRepository.GetTest3(), 1);
            //验证读取和添加缓存
            this._mockCacheProvider.Received().Add(this._cacheKey, this._testA, 7200);
            this._manager.Get<User>(this._cacheKey,()=>new User(),1000);
            this._manager.Update(this._cacheKey,new User(),1);
        }

        #endregion

        #region Update(更新缓存)

        /// <summary>
        /// 更新缓存
        /// </summary>
        [TestMethod]
        public void TestUpdate()
        {
            this._manager.Update(this._key, 1, 1);
            this._mockCacheProvider.Received().Update(this._signKey, "a", 1);
            this._mockCacheProvider.Received().Update(this._cacheKey, 1, 2);
        }

        #endregion

        #region Remove(移除缓存)

        /// <summary>
        /// 移除缓存
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            this._manager.Remove(this._key);
            this._mockCacheProvider.Received().Remove(this._signKey);
            this._mockCacheProvider.Received().Remove(this._cacheKey);
        }

        #endregion

        #region Clear(清空缓存)

        /// <summary>
        /// 清空缓存
        /// </summary>
        [TestMethod]
        public void TestClear()
        {
            this._manager.Clear();
            this._mockCacheProvider.Received().Clear();
        }

        #endregion
    }
}
