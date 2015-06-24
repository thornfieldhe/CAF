using System;
using System.Web;
using System.Web.Caching;

namespace CAF.Caches
{
    using CAF.Utility;

    /// <summary>
    /// 本地缓存提供程序
    /// </summary>
    public class LocalCacheProvider : CacheProviderBase {
        /// <summary>
        /// 添加缓存对象
        /// </summary>        
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位:秒.time=0永不过期</param>
        protected override void AddCache( string key, object target, int time ) {
            
            Fx.If(time > 0)
                .Then(() =>HttpRuntime.Cache.Insert( key, target, null, DateTime.Now.AddSeconds( time ), Cache.NoSlidingExpiration ) )
                .Else(() => HttpRuntime.Cache.Insert( key, target ));
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位：秒.time=0永不过期</param>
        protected override void UpdateCache( string key, object target, int time) {
            HttpRuntime.Cache.Insert( key, target, null, DateTime.Now.AddSeconds( time ), Cache.NoSlidingExpiration );
            HttpRuntime.Cache.Insert( key, target, null, DateTime.Now.AddSeconds( time ), Cache.NoSlidingExpiration );
            Fx.If(time> 0)
                .Then(
                    () =>
                    HttpRuntime.Cache.Insert(key, target, null, DateTime.Now.AddSeconds(time), Cache.NoSlidingExpiration))
                .Else(
                    () =>
                    HttpRuntime.Cache.Insert(key, target));
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        protected override T GetCache<T>( string key ) {
            return HttpRuntime.Cache.Get(key).To<T>();
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        protected override void RemoveCache( string key ) {
            HttpRuntime.Cache.Remove( key );
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public override void Clear() {
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while ( enumerator.MoveNext() ) {
                Remove( enumerator.Key.ToString() );
            }
        }
    }
}
