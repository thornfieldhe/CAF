using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAF.Core;
using CAF;

namespace TestCAF
{
    /// <summary>
    /// CommandTest 的摘要说明
    /// </summary>
    [TestClass]
    public class CommandTest
    {
        public CommandTest()
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

        #region 附加测试属性
        //
        // 编写测试时，还可使用以下附加属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void TestCommand()
        //{

        //}
    }

    public interface IPlugin
    {
        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="manager"></param>
        void Initialize();
        /// <summary>
        /// 初始化完成
        /// </summary>
        void OnLoad();
        /// <summary>
        /// 正在登录时触发
        /// </summary>
        void OnLogining();
        /// <summary>
        /// 登录成功时触发
        /// </summary>
        void OnLoginSucced();
        /// <summary>
        /// 登录失败时触发
        /// </summary>
        void OnLoginFailed();
        /// <summary>
        /// 注销时触发
        /// </summary>
        void OnLogout();
        /// <summary>
        /// 销毁插件
        /// </summary>
        void OnClosed();

        /// <summary>
        /// 插件名称
        /// </summary>
        string PluginName { get; }
    }

    public class PluginA : IPlugin
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnLoad()
        {
            throw new NotImplementedException();
        }

        public void OnLogining()
        {
            throw new NotImplementedException();
        }

        public void OnLoginSucced()
        {
            throw new NotImplementedException();
        }

        public void OnLoginFailed()
        {
            throw new NotImplementedException();
        }

        public void OnLogout()
        {
            throw new NotImplementedException();
        }

        public void OnClosed()
        {
            throw new NotImplementedException();
        }

        public string PluginName
        {
            get { return "PluginA"; }
        }

    }

    public class PluginB : IPlugin
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnLoad()
        {
            throw new NotImplementedException();
        }

        public void OnLogining()
        {
            throw new NotImplementedException();
        }

        public void OnLoginSucced()
        {
            throw new NotImplementedException();
        }

        public void OnLoginFailed()
        {
            throw new NotImplementedException();
        }

        public void OnLogout()
        {
            throw new NotImplementedException();
        }

        public void OnClosed()
        {
            throw new NotImplementedException();
        }

        public string PluginName {
            get { return "PluginB"; }
        }
    }

    public class PluginInvoker : CommandInvoker<IPlugin>,IPlugin
    {

        #region IPlugin 成员

        public void Initialize()
        {
            foreach (IPlugin item in _plugins)
            {
                item.Initialize();
            }
        }

        public void OnLoad()
        {
            foreach (IPlugin item in _plugins)
            {
                item.OnLoad();
            }
        }

        public void OnLogining()
        {
            foreach (IPlugin item in _plugins)
            {
                item.OnLogining();
            }
        }

        public void OnLoginSucced()
        {
            foreach (IPlugin item in _plugins)
            {
                item.OnLoginSucced();
            }
        }

        public void OnLoginFailed()
        {
            foreach (IPlugin item in _plugins)
            {
                item.OnLoginFailed();
            }
        }

        public void OnLogout()
        {
            foreach (IPlugin item in _plugins)
            {
                item.OnLogout();
            }
        }

        public void OnClosed()
        {
            foreach (IPlugin item in _plugins)
            {
                item.OnClosed();
            }
        }

        public string PluginName
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
