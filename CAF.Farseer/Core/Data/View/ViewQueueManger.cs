using CAF.FS.Core.Infrastructure;
using CAF.FS.Mapping.Context;

namespace CAF.FS.Core.Data.View
{
    public class ViewQueueManger : BaseQueueManger
    {
        public ViewQueueManger(DbExecutor database, ContextMap contextMap)
            : base(database, contextMap) { }
    }
}
