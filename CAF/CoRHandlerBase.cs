using CAF.Core;
using System.Collections.Generic;
using System;
namespace CAF
{
    public abstract class CoRHandlerBase<Request> where Request : ICorRequest
    {
        public CoRHandlerBase(string contex)
        {
            this.Contex = contex;
            if (this.Successors == null)
            {
                Successors = new List<CoRHandlerBase<Request>>();
            }
        }

        public abstract void Process(Request request);

        /// <summary>
        /// 处理客户请求
        /// </summary>
        /// <param name="request"></param>
        public virtual void HandleRequest(Request request)
        {
            if (HasBreakPoint)
            {
                OnBreak(new CallHandlerEventArgs<Request>(this, request));
            }
            if (request == null)
            {
                return;
            }
            Process(request);

            if (Successors != null)
            {
                foreach (CoRHandlerBase<Request> successor in Successors)
                {
                    successor.HandleRequest(request);
                }
            }
        }

        public virtual void OnBreak(CallHandlerEventArgs<Request> argus)
        {
            if (Break != null)
            {
                Break(this, argus);
            }
        }

        /// <summary>
        /// 添加后续节点
        /// </summary>
        /// <param name="success"></param>
        public void AddSuccessor(CoRHandlerBase<Request> success)
        {
            if (this.Successors == null)
            {
                Successors = new List<CoRHandlerBase<Request>>();
            }
            Successors.Add(success);
        }

        /// <summary>
        /// 实现迭代器，并且对容器对象实现隐性递归
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CoRHandlerBase<Request>> Enumerate()
        {
            yield return this;
            if ((Successors != null) && (Successors.Count > 0))
                foreach (CoRHandlerBase<Request> child in Successors)
                {
                    foreach (CoRHandlerBase<Request> item in child.Enumerate())
                        yield return item;
                }
        }

        /// <summary>
        /// 查询指定Handler
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public CoRHandlerBase<Request> FindHandler(string type)
        {
            if (this.Contex == type)
            {
                return this;
            }
            else
            {
                if ((Successors != null) && (Successors.Count > 0))
                    foreach (CoRHandlerBase<Request> child in Successors)
                    {
                        foreach (CoRHandlerBase<Request> item in child.Enumerate())
                        {
                            if (item.Contex == type)
                            {
                                return item;
                            }
                        }
                    }
            }
            return null;
        }

        /// <summary>
        /// 后继节点
        /// </summary>
        public List<CoRHandlerBase<Request>> Successors { get; set; }

        /// <summary>
        /// 当前Handler处理的请求类型
        /// </summary>
        public string Contex { get; set; }

        /// <summary>
        /// 是否定义断点
        /// </summary>
        public bool HasBreakPoint { get; set; }

        /// <summary>
        /// 断点事件
        /// </summary>
        public event EventHandler<CallHandlerEventArgs<Request>> Break;


    }
}
