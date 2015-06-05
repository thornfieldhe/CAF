using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Lambdas {
    using CAF;
    using CAF.Lambdas;
    using CAF.Tests.Samples;
    using CAF.Utility;

    /// <summary>
    /// 动态解析表达式测试
    /// </summary>
    [TestClass]
    public class ParseTest {
        /// <summary>
        /// 验证属性名错误
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void TestParse_Validate_PropertyError() {
            Assert.AreEqual( "(t.Name == \"A\")", Lambda.ParsePredicate<Test2>( "Name1", "A", Operator.Equal ).ToStr() );
        }

        /// <summary>
        /// 解析为表达式
        /// </summary>
        [TestMethod]
        public void TestParse() {
            Assert.AreEqual( "t => (t.Name == \"A\")", Lambda.ParsePredicate<Test2>( "Name", "A", Operator.Equal ).ToStr() );
            Assert.AreEqual( "t => t.Name.Contains(\"A\")", Lambda.ParsePredicate<Test2>( "Name", "A", Operator.Contains ).ToStr() );
        }

        /// <summary>
        /// 解析谓词表达式字符串
        /// </summary>
        [TestMethod]
        public void TestParse_2() {
            Assert.AreEqual( "Param_0 => (Param_0.Name == \"A\")", Lambda.ParsePredicate<Test2>( "Name == \"A\"" ).ToStr() );
            Assert.AreEqual( "Param_0 => Param_0.Name.Contains(\"A\")", Lambda.ParsePredicate<Test2>( "Name.Contains(\"A\")" ).ToStr() );
        }

        /// <summary>
        /// 解析谓词表达式，带参数占位符
        /// </summary>
        [TestMethod]
        public void TestParse_Param() {
            Assert.AreEqual( "Param_0 => (Param_0.Name == \"A\")", Lambda.ParsePredicate<Test2>( "Name == @0", "A" ).ToStr() );
            Assert.AreEqual( "Param_0 => Param_0.Name.Contains(\"A\")", Lambda.ParsePredicate<Test2>( "Name.Contains(@0)", "A" ).ToStr() );
            Assert.AreEqual( "Param_0 => ((Param_0.Name == \"A\") AndAlso (Param_0.Int > 1))", Lambda.ParsePredicate<Test2>( "Name == @0 && Int > @1", "A", 1 ).ToStr() );
        }
    }
}
