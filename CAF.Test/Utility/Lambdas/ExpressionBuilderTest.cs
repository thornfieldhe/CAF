using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Lambdas {
    using CAF.Lambdas;
    using CAF.Tests.Samples;
    using CAF.Utility;

    /// <summary>
    /// 测试表达式生成器
    /// </summary>
    [TestClass]
    public class ExpressionBuilderTest {
        /// <summary>
        /// 表达式生成器
        /// </summary>
        ExpressionBuilder<Test2> _builder;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit() {
            _builder = new ExpressionBuilder<Test2>();
        }

        /// <summary>
        /// 创建表达式
        /// </summary>
        [TestMethod]
        public void TestCreate_Int() {
            Expression<Func<Test2, int>> property = t => t.Int;
            var expression = _builder.Create( property, Operator.Equal, 1 );
            Expression<Func<Test2, bool>> expected = t => t.Int == 1;
            Assert.AreEqual( expected.ToString(),_builder.ToLambda( expression ).ToString());
        }

        /// <summary>
        /// 创建表达式
        /// </summary>
        [TestMethod]
        public void TestCreate_Int_Nullable() {
            Expression<Func<Test2, int?>> property = t => t.NullableInt;
            var expression = _builder.Create( property, Operator.Equal, 1 );
            Assert.AreEqual( "t => (t.NullableInt == 1)", _builder.ToLambda( expression ).ToString() );
        }
    }
}
