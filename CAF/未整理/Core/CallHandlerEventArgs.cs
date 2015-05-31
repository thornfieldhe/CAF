using System;
namespace CAF.Core
{
    /// <summary>
    /// 职责链响应断点的事件参数
    /// </summary>
    public class CallHandlerEventArgs<T> : EventArgs where T:ICorRequest
    {

        public CoRHandlerBase<T> Handler { get; private set; }
        public T Request { get; private set; }

        public CallHandlerEventArgs(CoRHandlerBase<T> handler, T request)
        {
            this.Handler = handler;
            this.Request = request;
        }
    }
}
