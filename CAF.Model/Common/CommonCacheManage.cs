
namespace CAF.Model.Common
{
    using CAF.Caches;

    public class CommonCacheManage : DefaultCacheManager
    {
        public CommonCacheManage(ICacheConfig config)
            : base(config)
        {
        }

        public CommonCacheManage(ICacheProvider provider, ICacheKey cacheKey)
            : base(provider, cacheKey)
        {
        }

        public CommonCacheManage()
            : base(TypeCreater.IocBuildUp<ICacheProvider>(), new DefaultCacheKey())
        {

        }
    }
}
