using CAF.FS.Core.Data;

namespace CAF.FS.Core.Infrastructure
{
    public interface IViewSet<TReturn>
    {
        Queue Queue { get; }
    }
}
