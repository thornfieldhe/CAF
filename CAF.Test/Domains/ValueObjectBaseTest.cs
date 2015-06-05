using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Domains.ValueObjecBases
{
    using CAF.Tests.Samples;

    /// <summary>
    /// 值对象测试
    /// </summary>
    [TestClass]
    public class ValueObjectBaseTest
    {
        /// <summary>
        /// 地址1
        /// </summary>
        private Address _address1;
        /// <summary>
        /// 地址2
        /// </summary>
        private Address _address2;
        /// <summary>
        /// 地址3
        /// </summary>
        private Address _address3;
        /// <summary>
        /// 地址4
        /// </summary>
        private Address _address4;
        /// <summary>
        /// 地址5
        /// </summary>
        private Address _address5;
        /// <summary>
        /// 客户
        /// </summary>
        private Customer2 _customer;
        /// <summary>
        /// 地6
        /// </summary>
        private Address _address6;
        /// <summary>
        /// 地址7
        /// </summary>
        private Address _address7;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._address1 = new Address("a", "b");
            this._address2 = new Address("a", "b");
            this._address3 = new Address("1", "");
            this._customer = new Customer2();
            this._address4 = new Address("a", "b", this._customer);
            this._address5 = new Address("a", "b", this._customer);
            this._address6 = new Address("a", "b", this._customer, new Address("a", "b"));
            this._address7 = new Address("a", "b", this._customer, new Address("a", "b"));
        }

        /// <summary>
        /// 测试对象相等性
        /// </summary>
        [TestMethod]
        public void TestEquals()
        {
            Assert.IsFalse(this._address1.Equals(null));
            Assert.IsFalse(this._address1 == null);
            Assert.IsFalse(null == this._address1);
            Assert.IsFalse(this._address1.Equals(new Customer2()));
            Assert.IsTrue(this._address1.Equals(this._address2), "_address1.Equals( _address2 )");
            Assert.IsTrue(this._address1 == this._address2, "_address1 == _address2");
            Assert.IsFalse(this._address1 != this._address2, "_address1 != _address2");
            Assert.IsFalse(this._address1 == this._address3, "_address1 == _address3");
            Assert.IsTrue(this._address4 == this._address5, "_address4 == _address5");
            Assert.IsTrue(this._address4.Equals(this._address5), "_address4.Equals( _address5 )");
            Assert.IsFalse(this._address6 == this._address7, "_address6 == _address7");
            Assert.IsFalse(this._address6.Equals(this._address7), "_address6.Equals( _address7 )");
        }

        /// <summary>
        /// 测试哈希
        /// </summary>
        [TestMethod]
        public void TestGetHashCode()
        {
            Assert.IsTrue(this._address1.GetHashCode() == this._address2.GetHashCode());
        }

        /// <summary>
        /// 测试克隆
        /// </summary>
        [TestMethod]
        public void TestClone()
        {
            this._address3 = this._address1.Clone();
            Assert.IsTrue(this._address1 == this._address3);
        }
    }
}
