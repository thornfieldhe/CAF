using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;
using System.Linq.Expressions;

namespace CAF.FS.Core.Client.Common
{
    public class ExpressionNew : DbExpressionNewProvider
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="queueManger">队列管理模块</param>
        /// <param name="queue">包含数据库SQL操作的队列</param>
        public ExpressionNew(BaseQueueManger queueManger, Queue queue) : base(queueManger, queue) { }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            var methodName = m.Method.Name;
            switch (methodName)
            {
                case "ConvertType": return this.Visit(m.Object ?? m.Arguments[0]);
            }
            return base.VisitMethodCall(m);
        }
    }
}