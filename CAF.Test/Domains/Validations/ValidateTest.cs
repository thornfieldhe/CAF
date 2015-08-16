using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Domains.Validations
{
//    using CAF.DI;
    using CAF.Exceptions;
    using CAF.Tests.Samples;
    using CAF.Validations;

    /// <summary>
    /// 验证测试
    /// </summary>
    [TestClass]
    public class ValidateTest
    {
        /// <summary>
        /// 客户
        /// </summary>
        private Customer2 _customer2;
//        /// <summary>
//        /// 测试初始化
//        /// </summary>
//        [TestInitialize]
//        public void TestInit()
//        {
//            Ioc.Register(new IocConfig());
//            this._customer2 = new Customer2();
//       
//        }

        /// <summary>
        /// 基本验证
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Warning))]
        public void TestSetValidationHandler2()
        {
            try
            {
                this._customer2.Name = null;
                this._customer2.Validate();
            }
            catch (Warning ex)
            {
                Assert.AreEqual("姓名不能为空", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 外部调用方法AddValidationRule增加验证条件
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Warning))]
        public void TestAddValidationRule()
        {
            try
            {
                this._customer2 = new Customer2();
                this._customer2.Name = "1";
                this._customer2.AddValidationRule(new MinLengthValidationRule(this._customer2.Name));
                this._customer2.Validate();
            }
            catch (Warning ex)
            {
                Assert.AreEqual("姓名长度不能小于2", ex.Message);
                throw;
            }
        }
        /// <summary>
        ///重载方法Validate(ValidationResultCollection results)增加验证条件
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Warning))]
        public void TestSetValidationHandler()
        {
            try
            {
                this._customer2 = new Customer2 { Name = "ddddd" };
                this._customer2.Validate();
            }
            catch (Warning ex)
            {
                Assert.AreEqual("姓名长度不能大于3", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 设置验证处理器,不进行任何操作，所以不会抛出异常
        /// </summary>
        [TestMethod]
        public void TestSetValidationHandler_NotThow()
        {
            this._customer2 = new Customer2();
            this._customer2.Name = null;
            this._customer2.SetValidationHandler(new NothingValidationHandler());
            this._customer2.Validate();
        }

    }
}
