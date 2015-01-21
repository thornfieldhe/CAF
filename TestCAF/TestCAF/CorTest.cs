using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAF;
using CAF.Core;
namespace TestCAF
{
    /// <summary>
    /// CorTest 的摘要说明
    /// </summary>
    [TestClass]
    public class CorTest
    {
        public CorTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void CorTest1()
        {
            CoRHandlerBase<Request> handler1 = new InternalHandler();
            CoRHandlerBase<Request> handler2 = new MailHandler();
            CoRHandlerBase<Request> handler3 = new DiscountHandler();
            CoRHandlerBase<Request> handler4 = new RegularHandler();

            Request request = new Request(20, "Mail");
            handler1.AddSuccessor(handler3);
            handler3.AddSuccessor(handler2);
            handler3.AddSuccessor(handler4);
            handler1.HandleRequest(request);
            Assert.AreEqual<double>(20 * 1.3, request.Price);

            CoRHandlerBase<Request> temp = handler1.FindHandler("Discount");
            temp.HasBreakPoint = true;
            temp.Break += new EventHandler<CallHandlerEventArgs<Request>>(handler1_Break);
            request = new Request(20, "Mail");
            handler1.HandleRequest(request);
            Assert.AreEqual<double>(20, request.Price);
        }


        void handler1_Break(object sender, CallHandlerEventArgs<Request> e)
        {
            CoRHandlerBase<Request> handler = e.Handler;
            Assert.AreEqual("Discount", handler.Contex);
            handler.HasBreakPoint = false;
            handler.Successors = null;
            handler.HandleRequest(e.Request);
        }

    }

    public class InternalHandler : CoRHandlerBase<Request>
    {
        public InternalHandler() : base("Internal") { }


        public override void Process(Request request)
        {
            if (request.Contex == this.Contex)
            {
                request.Price *= 0.6;
            }
        }
    }

    public class MailHandler : CoRHandlerBase<Request>
    {
        public MailHandler() : base("Mail") { }

        public override void Process(Request request)
        {
            if (request.Contex == this.Contex)
            {
                request.Price *= 1.3;
            }
        }
    }

    public class DiscountHandler : CoRHandlerBase<Request>
    {
        public DiscountHandler() : base("Discount") { }

        public override void Process(Request request)
        {
            if (request.Contex == this.Contex)
            {
                request.Price *= 0.9;
            }
        }
    }

    public class RegularHandler : CoRHandlerBase<Request>
    {
        public RegularHandler() : base("Reguar") { }

        public override void Process(Request request)
        {
            if (request.Contex == this.Contex)
            {
                request.Price *= 5;
            }
        }
    }

    public class Request : ICorRequest
    {
        public double Price { get; set; }
        public string Contex { get; private set; }

        public Request(double price, string type)
        {
            this.Price = price;
            this.Contex = type;
        }
    }
}
