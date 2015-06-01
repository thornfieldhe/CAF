using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Specifications
{
    using CAF.Utility;

    [TestClass]
    public class SpecificationTest
    {
        [TestMethod]
        public void Test_And_Specification()
        {
            ISpecification<int> rule1 = new ExpressionSpecification<int>(x => x == 1, "rule1 failed");
            ISpecification<int> rule2 = new ExpressionSpecification<int>(x => x == 2, "rule2 failed");
            ISpecification<int> rule3 = new ExpressionSpecification<int>(x => x == 3, "rule3 failed");
            ISpecification<int> rule4 = rule1.Or(rule2).Or(rule3);

            var result = rule4.ValidateWithMessages(3);

            Assert.IsTrue(result.Count == 0);
        }
    }
}
