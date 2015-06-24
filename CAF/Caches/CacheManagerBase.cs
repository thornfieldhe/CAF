using System;

namespace CAF.Caches {
    using System.Threading;

    /// <summary>
    /// 基缓存管理器
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager {
        /// <summary>
        /// 初始化基缓存管理器
        /// </summary>
        /// <param name="config">缓存配置</param>
        protected CacheManagerBase( ICacheConfig config )
            : this( config.GetProvider(), config.GetKey() ) {
        }

        /// <summary>
        /// 初始化基缓存管理器
        /// </summary>
        /// <param name="provider">缓存提供程序</param>
        /// <param name="cacheKey">缓存键</param>
        protected CacheManagerBase( ICacheProvider provider, ICacheKey cacheKey ) {
            provider.CheckNull( "provider" );
            cacheKey.CheckNull( "cacheKey" );
            this.CacheProvider = provider;
            this.CacheKey = cacheKey;
        }

        /// <summary>
        /// 缓存提供程序
        /// </summary>
        public ICacheProvider CacheProvider { get; private set; }

        /// <summary>
        /// 缓存键
        /// </summary>
        public ICacheKey CacheKey { get; private set; }

        /// <summary>
        /// 缓存过期标记
        /// </summary>
        public const string CacheSign = "a";

        /// <summary>
        /// 获取缓存对象，当缓存对象不存在，则执行方法并添加到缓存中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addHandler">添加缓存方法，当缓存对象不存在时，执行该方法获得缓存对象</param>
        /// <param name="time">缓存过期时间，单位：秒</param>
        public T Get<T>( string key, Func<T> addHandler, int time = 0 ) {
            var lockKey = this.GetKey( key );
            var signKey = this.GetSignKey( key );
            var result = this.CacheProvider.Get<T>( lockKey );
            var sign = this.CacheProvider.Get<string>( signKey );
            if ( !sign.IsEmpty() )
                return result;
            lock ( signKey ) {
                sign = this.CacheProvider.Get<string>( signKey );
                if ( !sign.IsEmpty() )
                    return result;
                this.CacheProvider.Add( signKey, CacheSign, this.GetCacheTime( time ) );
                return this.UpdateCache( addHandler, lockKey, time, result );
            }
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        public T Get<T>(string key)
        {
            var lockKey = this.GetKey(key);
            var signKey = this.GetSignKey(key);
            var result = this.CacheProvider.Get<T>(lockKey);
            var sign = this.CacheProvider.Get<string>(signKey);
            if (!sign.IsEmpty())
                return result;
            lock (signKey)
            {
                sign = this.CacheProvider.Get<string>(signKey);
                return !sign.IsEmpty() ? result : default(T);
            }
        }

        /// <summary>
        /// 获取缓存键
        /// </summary>
        private string GetKey( string key ) {
            return this.CacheKey.GetKey( key );
        }

        /// <summary>
        /// 获取缓存过期标记
        /// </summary>
        private string GetSignKey( string key ) {
            return string.Intern(this.CacheKey.GetSignKey( key ) );
        }

        /// <summary>
        /// 获取缓存时间
        /// </summary>
        private int GetCacheTime( int time ) {
            return time;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        private T UpdateCache<T>( Func<T> addHandler, string lockKey, int time,T result ) {
            if ( Equals( result, null ) ) {
                result = addHandler();
                this.CacheProvider.Add( lockKey, result, this.GetCacheTime( time ) * 2 );
                return result;
            }
            Utility.Thread.StartTask( () => this.CacheProvider.Update( lockKey, addHandler(), this.GetCacheTime( time ) * 2 ) );
            return result;
        }

        /// <summary>
        /// 获取缓存对象，当缓存对象不存在，则执行方法并添加到缓存中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addHandler">添加缓存方法，当缓存对象不存在时，执行该方法获得缓存对象</param>
        /// <param name="time">缓存过期时间，单位：分</param>
        public T GetByMinutes<T>( string key, Func<T> addHandler, int time = 0 ) {
            return this.Get( key, addHandler, time * 60 );
        }

        /// <summary>
        /// 获取缓存对象，当缓存对象不存在，则执行方法并添加到缓存中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addHandler">添加缓存方法，当缓存对象不存在时，执行该方法获得缓存对象</param>
        /// <param name="time">缓存过期时间，单位：小时</param>
        public T GetByHours<T>( string key, Func<T> addHandler, int time = 0 ) {
            return this.Get( key, addHandler, time * 3600 );
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位：秒</param>
        public void Update( string key, object target, int time ) {
            this.CacheProvider.Update(this.GetSignKey( key ), CacheSign, this.GetCacheTime( time ) );
            this.CacheProvider.Update(this.GetKey( key ), target, this.GetCacheTime( time ) * 2 );
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove( string key ) {
            this.CacheProvider.Remove(this.GetSignKey( key ) );
            this.CacheProvider.Remove(this.GetKey( key ) );
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void Clear() {
            this.CacheProvider.Clear();
        }
    }
}
