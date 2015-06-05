using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAF.Tests.Domains.Warning
{
    using CAF.Exceptions;
    using CAF.Logs;
    using CAF.Utility;

    /// <summary>
    /// 应用程序异常测试
    /// </summary>
    [TestClass]
    public class WarningTest
    {

        #region 常量

        /// <summary>
        /// 异常消息
        /// </summary>
        public const string Message = "A";

        /// <summary>
        /// 异常消息2
        /// </summary>
        public const string Message2 = "B";

        /// <summary>
        /// 异常消息3
        /// </summary>
        public const string Message3 = "C";

        /// <summary>
        /// 异常消息4
        /// </summary>
        public const string Message4 = "D";

        #endregion

        #region TestValidate_MessageIsNull(验证消息为空)

        /// <summary>
        /// 验证消息为空
        /// </summary>
        [TestMethod]
        public void TestValidate_MessageIsNull()
        {
            Warning warning = new Warning(null, "P1");
            Assert.AreEqual(string.Empty, warning.Message);
        }

        #endregion

        #region TestCode(设置错误码)

        /// <summary>
        /// 设置错误码
        /// </summary>
        [TestMethod]
        public void TestCode()
        {
            Warning warning = new Warning(Message, "P1");
            Assert.AreEqual("P1", warning.Code);
        }

        #endregion

        #region TestLogLevel(测试日志级别)

        /// <summary>
        /// 测试日志级别
        /// </summary>
        [TestMethod]
        public void TestLogLevel()
        {
            Warning warning = new Warning(Message, "P1", LogLevel.Fatal);
            Assert.AreEqual(LogLevel.Fatal, warning.Level);
        }

        #endregion

        #region TestMessage_OnlyMessage(仅设置消息)

        /// <summary>
        /// 仅设置消息
        /// </summary>
        [TestMethod]
        public void TestMessage_OnlyMessage()
        {
            Warning warning = new Warning(Message);
            Assert.AreEqual(Message, warning.Message);
        }

        #endregion

        #region TestMessage_OnlyException(仅设置异常)

        /// <summary>
        /// 仅设置异常
        /// </summary>
        [TestMethod]
        public void TestMessage_OnlyException()
        {
            Warning warning = new Warning(GetException());
            Assert.AreEqual(Message, warning.Message);
        }

        /// <summary>
        /// 获取异常
        /// </summary>
        private Exception GetException()
        {
            return new Exception(Message);
        }

        #endregion

        #region TestMessage_Message_Exception(设置错误消息和异常)

        /// <summary>
        /// 设置错误消息和异常
        /// </summary>
        [TestMethod]
        public void TestMessage_Message_Exception()
        {
            Warning warning = new Warning(Message2, "P1", LogLevel.Fatal, GetException());
            Assert.AreEqual(string.Format("{0}\r\n{1}", Message2, Message), warning.Message);
        }

        #endregion

        #region TestMessage_2LayerException(设置2层异常)

        /// <summary>
        /// 设置2层异常
        /// </summary>
        [TestMethod]
        public void TestMessage_2LayerException()
        {
            Warning warning = new Warning(Message3, "P1", LogLevel.Fatal, Get2LayerException());
            Assert.AreEqual(string.Format("{0}\r\n{1}\r\n{2}", Message3, Message2, Message), warning.Message);
        }

        /// <summary>
        /// 获取2层异常
        /// </summary>
        private Exception Get2LayerException()
        {
            return new Exception(Message2, new Exception(Message));
        }

        #endregion

        #region TestMessage_Warning(设置Warning异常)

        /// <summary>
        /// 设置Warning异常
        /// </summary>
        [TestMethod]
        public void TestMessage_Warning()
        {
            Warning warning = new Warning(GetWarning());
            Assert.AreEqual(Message, warning.Message);
        }

        /// <summary>
        /// 获取异常
        /// </summary>
        private Warning GetWarning()
        {
            return new Warning(Message);
        }

        #endregion

        #region TestMessage_2LayerWarning(设置2层Warning异常)

        /// <summary>
        /// 设置2层Warning异常
        /// </summary>
        [TestMethod]
        public void TestMessage_2LayerWarning()
        {
            Warning warning = new Warning(Message3, "", Get2LayerWarning());
            Assert.AreEqual(string.Format("{0}\r\n{1}\r\n{2}", Message3, Message2, Message), warning.Message);
        }

        /// <summary>
        /// 获取2层异常
        /// </summary>
        private Warning Get2LayerWarning()
        {
            return new Warning(Message2, "", new Warning(Message));
        }

        #endregion

        #region TestMessage_3LayerWarning(设置3层Warning异常)

        /// <summary>
        /// 设置3层Warning异常
        /// </summary>
        [TestMethod]
        public void TestMessage_3LayerWarning()
        {
            Warning warning = new Warning(Message4, "", Get3LayerWarning());
            Assert.AreEqual(string.Format("{0}\r\n{1}\r\n{2}\r\n{3}", Message4, Message3, Message2, Message), warning.Message);
        }

        /// <summary>
        /// 获取3层异常
        /// </summary>
        private Warning Get3LayerWarning()
        {
            return new Warning(Message3, "", new Exception(Message2, new Warning(Message)));
        }

        #endregion

        #region 添加异常数据

        /// <summary>
        /// 添加异常数据
        /// </summary>
        [TestMethod]
        public void TestAdd_1Layer()
        {
            Warning warning = new Warning(Message);
            warning.Data.Add("key1", "value1");
            warning.Data.Add("key2", "value2");

            Str expected = new Str();
            expected.AddLine(Message);
            expected.AddLine("key1:value1");
            expected.AddLine("key2:value2");
            Assert.AreEqual(expected.ToString(), warning.Message);
        }

        /// <summary>
        /// 添加异常数据
        /// </summary>
        [TestMethod]
        public void TestAdd_2Layer()
        {
            Exception exception = new Exception(Message);
            exception.Data.Add("a", "a1");
            exception.Data.Add("b", "b1");

            Warning warning = new Warning(exception);
            warning.Data.Add("key1", "value1");
            warning.Data.Add("key2", "value2");

            Str expected = new Str();
            expected.AddLine(Message);
            expected.AddLine("a:a1");
            expected.AddLine("b:b1");
            expected.AddLine("key1:value1");
            expected.AddLine("key2:value2");
            Assert.AreEqual(expected.ToString(), warning.Message);
        }

        #endregion
    }
}
