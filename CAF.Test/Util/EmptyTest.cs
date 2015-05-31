using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Test
{
    using System;

    [TestClass]
    public class EnptyTest
    {
        [TestMethod]
        public void TestIsNull()
        {
            string a = null;
            Assert.IsTrue(a.IsNull());
            Assert.IsFalse(a.IsNotNull());
        }

        [TestMethod]
        public void TestStringIsEmpty()
        {
            var a = "";
            Assert.IsTrue(a.IsEmpty());
            a = null;
            Assert.IsTrue(a.IsEmpty());
        }

        [TestMethod]
        public void TestGuidIsEmpty()
        {
            Guid? a = null;
            Assert.IsTrue(a.IsEmpty());
            a = Guid.Empty;
            Assert.IsTrue(a.IsEmpty());
            var b = Guid.Empty;
            Assert.IsTrue(b.IsEmpty());
        }
    }
}
