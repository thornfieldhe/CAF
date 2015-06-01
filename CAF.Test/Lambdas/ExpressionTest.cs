using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Lambdas {
    using CAF.Tests.Samples;
    using CAF.Utility;

    /// <summary>
    /// 测试表达式
    /// </summary>
    [TestClass]
    public class ExpressionTest {

        #region GetName(获取成员名称)

        /// <summary>
        /// 获取成员名称
        /// </summary>
        [TestMethod]
        public void TestGetName() {
            //空值返回空字符串
            Assert.AreEqual( "", Lambda.GetName( null ) );

            //返回一级属性名
            Expression<Func<Test1, string>> expression = test => test.Name;
            Assert.AreEqual( "Name", Lambda.GetName( expression ) );

            //返回二级属性名
            Expression<Func<Test1, string>> expression2 = test => test.A.Address;
            Assert.AreEqual( "A.Address", Lambda.GetName( expression2 ) );

            //返回三级属性名
            Expression<Func<Test1, string>> expression3 = test => test.A.B.Name;
            Assert.AreEqual( "A.B.Name", Lambda.GetName( expression3 ) );

            //测试可空整型
            Expression<Func<Test1, int?>> expression4 = test => test.NullableInt;
            Assert.AreEqual( "NullableInt", Lambda.GetName( expression4 ) );

            //测试类型转换
            Expression<Func<Test1, int?>> expression5 = test => test.Age;
            Assert.AreEqual( "Age", Lambda.GetName( expression5 ) );
        }

        #endregion

        #region GetNames(获取成员名称列表)

        /// <summary>
        /// 获取成员名称列表
        /// </summary>
        [TestMethod]
        public void TestGetNames() {
            Expression<Func<Test1, object[]>> expression = ( t => new object[] {t.A.Address, t.Age} );
            Assert.AreEqual( 2, Lambda.GetNames( expression ).Count );
            Assert.AreEqual( "A.Address", Lambda.GetNames( expression )[0] );
            Assert.AreEqual( "Age", Lambda.GetNames( expression )[1] );
        }

        #endregion

        #region GetValue(获取成员值)

        /// <summary>
        /// 获取成员值,委托返回类型为Object
        /// </summary>
        [TestMethod]
        public void TestGetValue_Object() {
            Expression<Func<Test1, object>> expression = test => test.Name == "A";
            Assert.AreEqual( "A", Lambda.GetValue( expression ) );
        }

        /// <summary>
        /// 获取成员值,委托返回类型为bool
        /// </summary>
        [TestMethod]
        public void TestGetValue_Boolean() {
            //空值返回null
            Assert.AreEqual( null, Lambda.GetValue( null ) );

            //一级返回值
            Expression<Func<Test1, bool>> expression = test => test.Name == "A";
            Assert.AreEqual( "A", Lambda.GetValue( expression ) );

            //二级返回值
            Expression<Func<Test1, bool>> expression2 = test => test.A.Integer == 1;
            Assert.AreEqual( 1, Lambda.GetValue( expression2 ) );

            //三级返回值
            Expression<Func<Test1, bool>> expression3 = test => test.A.B.Name == "B";
            Assert.AreEqual( "B", Lambda.GetValue( expression3 ) );
        }

        /// <summary>
        /// 获取成员值,测试运算符
        /// </summary>
        [TestMethod]
        public void TestGetValue_Operation() {
            //不等于
            Expression<Func<Test1, bool>> expression = test => test.A.Integer != 1;
            Assert.AreEqual( 1, Lambda.GetValue( expression ), "!=" );

            //大于
            expression = test => test.A.Integer > 1;
            Assert.AreEqual( 1, Lambda.GetValue( expression ),">" );

            //小于
            expression = test => test.A.Integer < 1;
            Assert.AreEqual( 1, Lambda.GetValue( expression ),"<" );

            //大于等于
            expression = test => test.A.Integer >= 1;
            Assert.AreEqual( 1, Lambda.GetValue( expression ),">=" );

            //小于等于
            expression = test => test.A.Integer <= 1;
            Assert.AreEqual( 1, Lambda.GetValue( expression ), "<=" );
        }

        /// <summary>
        /// 获取可空类型的值
        /// </summary>
        [TestMethod]
        public void TestGetValue_Nullable() {
            //可空整型
            Expression<Func<Test1, bool>> expression = test => test.NullableInt == 1;
            Assert.AreEqual( 1, Lambda.GetValue( expression ) );

            //可空decimal
            expression = test => test.NullableDecimal == 1.5M;
            Assert.AreEqual( 1.5M, Lambda.GetValue( expression ) );
        }

        /// <summary>
        /// 获取成员值，运算符为方法
        /// </summary>
        [TestMethod]
        public void TestGetValue_Method() {
            //1级返回值
            Expression<Func<Test1, bool>> expression = t => t.Name.Contains( "A" );
            Assert.AreEqual( "A", Lambda.GetValue( expression ) );

            //二级返回值
            expression = t => t.A.Address.Contains( "B" );
            Assert.AreEqual( "B", Lambda.GetValue( expression ) );

            //三级返回值
            expression = t => t.A.B.Name.StartsWith( "C" );
            Assert.AreEqual( "C", Lambda.GetValue( expression ) );
        }

        /// <summary>
        /// 从实例中获取值
        /// </summary>
        [TestMethod]
        public void TestGetValue_Instance() {
            var test = new Test1() { Name = "a", A = new Test1.TestA() { Address = "b", B = new Test1.TestA.TestB() { Name = "c" } } };

            //一级属性
            Expression<Func<string>> expression = () => test.Name;
            Assert.AreEqual( "a", Lambda.GetValue( expression ) );

            //二级属性
            Expression<Func<string>> expression2 = () => test.A.Address;
            Assert.AreEqual( "b", Lambda.GetValue( expression2 ) );

            //三级属性
            Expression<Func<string>> expression3 = () => test.A.B.Name;
            Assert.AreEqual( "c", Lambda.GetValue( expression3 ) );
        }

        /// <summary>
        /// 测试值为复杂类型
        /// </summary>
        [TestMethod]
        public void TestGetValue_Complex() {
            var test = new Test1() { Name = "a", A = new Test1.TestA() { Address = "b" } };

            //获取表达式的值
            Expression<Func<Test1, bool>> expression = t => t.Name == test.Name;
            Assert.AreEqual( "a", Lambda.GetValue( expression ), "==test.Name" );
            Expression<Func<Test1, bool>> expression2 = t => t.Name == test.A.Address;
            Assert.AreEqual( "b", Lambda.GetValue( expression2 ), "==test.A.Address" );

            //获取方法的值
            Expression<Func<Test1, bool>> expression3 = t => t.Name.Contains( test.Name );
            Assert.AreEqual( "a", Lambda.GetValue( expression3 ), "Contains test.Name" );
            Expression<Func<Test1, bool>> expression4 = t => t.Name.Contains( test.A.Address );
            Assert.AreEqual( "b", Lambda.GetValue( expression4 ), "==test.A.Address" );
        }

        /// <summary>
        /// 测试值为枚举
        /// </summary>
        [TestMethod]
        public void TestGetValue_Enum() {
            var test1 = new Test1();
            test1.NullableEnumValue = LogType.Error;

            //属性为枚举,值为枚举
            Expression<Func<Test1, bool>> expression = test => test.EnumValue == LogType.Debug;
            Assert.AreEqual( LogType.Debug.Value(), Lambda.GetValue( expression ) );

            //属性为枚举,值为可空枚举
            expression = test => test.EnumValue == test1.NullableEnumValue;
            Assert.AreEqual( LogType.Error, Lambda.GetValue( expression ) );

            //属性为可空枚举,值为枚举
            expression = test => test.NullableEnumValue == LogType.Debug;
            Assert.AreEqual( LogType.Debug, Lambda.GetValue( expression ) );

            //属性为可空枚举,值为可空枚举
            expression = test => test.NullableEnumValue == test1.NullableEnumValue;
            Assert.AreEqual( LogType.Error, Lambda.GetValue( expression ) );

            //属性为可空枚举,值为null
            test1.NullableEnumValue = null;
            expression = test => test.NullableEnumValue == test1.NullableEnumValue;
            Assert.AreEqual( null, Lambda.GetValue( expression ) );
        }

        #endregion

        #region GetParameter(获取参数)

        /// <summary>
        /// 获取参数
        /// </summary>
        [TestMethod]
        public void TestGetParameter() {
            //空值返回null
            Assert.AreEqual( null, Lambda.GetParameter( null ) );

            //一级
            Expression<Func<Test1, object>> expression = test => test.Name == "A";
            Assert.AreEqual( "test", Lambda.GetParameter( expression ).ToString() );

            //二级
            expression = test => test.A.Integer == 1;
            Assert.AreEqual( "test", Lambda.GetParameter( expression ).ToString() );

            //三级
            expression = test => test.A.B.Name == "B";
            Assert.AreEqual( "test", Lambda.GetParameter( expression ).ToString() );

            //属性
            expression = test => test.A.Integer;
            Assert.AreEqual( "test", Lambda.GetParameter( expression ).ToString() );

            //方法
            expression = test => test.A.B.Name.Contains( "B" );
            Assert.AreEqual( "test", Lambda.GetParameter( expression ).ToString() );
        }

        #endregion

        #region GetCriteriaCount(获取谓词条件的个数)

        /// <summary>
        /// 获取谓词条件的个数
        /// </summary>
        [TestMethod]
        public void TestGetCriteriaCount() {
            //0个条件
            Assert.AreEqual( 0, Lambda.GetCriteriaCount( null ) );

            //1个条件
            Expression<Func<Test1, bool>> expression = test => test.Name == "A";
            Assert.AreEqual( 1, Lambda.GetCriteriaCount( expression ) );

            //2个条件，与连接符
            expression = test => test.Name == "A" && test.Name == "B";
            Assert.AreEqual( 2, Lambda.GetCriteriaCount( expression ) );

            //2个条件，或连接符
            expression = test => test.Name == "A" || test.Name == "B";
            Assert.AreEqual( 2, Lambda.GetCriteriaCount( expression ) );

            //3个条件
            expression = test => test.Name == "A" && test.Name == "B" || test.Name == "C";
            Assert.AreEqual( 3, Lambda.GetCriteriaCount( expression ) );

            //3个条件,包括导航属性
            expression = test => test.A.Address == "A" && test.Name == "B" || test.Name == "C";
            Assert.AreEqual( 3, Lambda.GetCriteriaCount( expression ) );
        }

        /// <summary>
        /// 获取谓词条件的个数，运算符为方法
        /// </summary>
        [TestMethod]
        public void TestGetCriteriaCount_Method() {
            //1个条件
            Expression<Func<Test1, bool>> expression = t => t.Name.Contains( "A" );
            Assert.AreEqual( 1, Lambda.GetCriteriaCount( expression ) );

            //2个条件,与连接
            expression = t => t.Name.Contains( "A" ) && t.Name == "A";
            Assert.AreEqual( 2, Lambda.GetCriteriaCount( expression ) );

            //2个条件,或连接,包含导航属性
            expression = t => t.Name.Contains( "A" ) || t.A.Address == "A";
            Assert.AreEqual( 2, Lambda.GetCriteriaCount( expression ) );
        }

        #endregion

        #region Equal(创建等于表达式)

        /// <summary>
        /// 创建等于表达式
        /// </summary>
        [TestMethod]
        public void TestEqual() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Age == 1;
            Assert.AreEqual( expected.ToString(), Lambda.Equal<Test1>( "Age", 1 ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Integer == 1;
            Assert.AreEqual( expected2.ToString(), Lambda.Equal<Test1>( "A.Integer", 1 ).ToString() );
        }

        #endregion

        #region NotEqual(创建不等于表达式)

        /// <summary>
        /// 创建不等于表达式
        /// </summary>
        [TestMethod]
        public void TestNotEqual() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Age != 1;
            Assert.AreEqual( expected.ToString(), Lambda.NotEqual<Test1>( "Age", 1 ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Integer != 1;
            Assert.AreEqual( expected2.ToString(), Lambda.NotEqual<Test1>( "A.Integer", 1 ).ToString() );
        }

        #endregion

        #region Greater(创建大于表达式)

        /// <summary>
        /// 创建大于表达式
        /// </summary>
        [TestMethod]
        public void TestGreater() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Age > 1;
            Assert.AreEqual( expected.ToString(), Lambda.Greater<Test1>( "Age", 1 ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Integer > 1;
            Assert.AreEqual( expected2.ToString(), Lambda.Greater<Test1>( "A.Integer", 1 ).ToString() );
        }

        #endregion

        #region Less(创建小于表达式)

        /// <summary>
        /// 创建小于表达式
        /// </summary>
        [TestMethod]
        public void TestLess() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Age < 1;
            Assert.AreEqual( expected.ToString(), Lambda.Less<Test1>( "Age", 1 ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Integer < 1;
            Assert.AreEqual( expected2.ToString(), Lambda.Less<Test1>( "A.Integer", 1 ).ToString() );
        }

        #endregion

        #region GreaterEqual(创建大于等于表达式)

        /// <summary>
        /// 创建大于等于表达式
        /// </summary>
        [TestMethod]
        public void TestGreaterEqual() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Age >= 1;
            Assert.AreEqual( expected.ToString(), Lambda.GreaterEqual<Test1>( "Age", 1 ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Integer >= 1;
            Assert.AreEqual( expected2.ToString(), Lambda.GreaterEqual<Test1>( "A.Integer", 1 ).ToString() );
        }

        #endregion

        #region LessEqual(创建小于等于表达式)

        /// <summary>
        /// 创建小于等于表达式
        /// </summary>
        [TestMethod]
        public void TestLessEqual() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Age <= 1;
            Assert.AreEqual( expected.ToString(), Lambda.LessEqual<Test1>( "Age", 1 ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Integer <= 1;
            Assert.AreEqual( expected2.ToString(), Lambda.LessEqual<Test1>( "A.Integer", 1 ).ToString() );
        }

        #endregion

        #region Contains(调用Contains方法)

        /// <summary>
        /// 调用Contains方法
        /// </summary>
        [TestMethod]
        public void TestContains() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Name.Contains( "A" );
            Assert.AreEqual( expected.ToString(), Lambda.Contains<Test1>( "Name", "A" ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Address.Contains( "A" );
            Assert.AreEqual( expected2.ToString(), Lambda.Contains<Test1>( "A.Address", "A" ).ToString() );

            //三级属性
            Expression<Func<Test1, bool>> expected3 = t => t.A.B.Name.Contains( "A" );
            Assert.AreEqual( expected3.ToString(), Lambda.Contains<Test1>( "A.B.Name", "A" ).ToString() );
        }

        #endregion

        #region Starts(调用StartsWith方法)

        /// <summary>
        /// 调用StartsWith方法
        /// </summary>
        [TestMethod]
        public void TestStarts() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Name.StartsWith( "A" );
            Assert.AreEqual( expected.ToString(), Lambda.Starts<Test1>( "Name", "A" ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Address.StartsWith( "A" );
            Assert.AreEqual( expected2.ToString(), Lambda.Starts<Test1>( "A.Address", "A" ).ToString() );

            //三级属性
            Expression<Func<Test1, bool>> expected3 = t => t.A.B.Name.StartsWith( "A" );
            Assert.AreEqual( expected3.ToString(), Lambda.Starts<Test1>( "A.B.Name", "A" ).ToString() );
        }

        #endregion

        #region Ends(调用EndsWith方法)

        /// <summary>
        /// 调用EndsWith方法
        /// </summary>
        [TestMethod]
        public void TestEnds() {
            //一级属性
            Expression<Func<Test1, bool>> expected = t => t.Name.EndsWith( "A" );
            Assert.AreEqual( expected.ToString(), Lambda.Ends<Test1>( "Name", "A" ).ToString() );

            //二级属性
            Expression<Func<Test1, bool>> expected2 = t => t.A.Address.EndsWith( "A" );
            Assert.AreEqual( expected2.ToString(), Lambda.Ends<Test1>( "A.Address", "A" ).ToString() );

            //三级属性
            Expression<Func<Test1, bool>> expected3 = t => t.A.B.Name.EndsWith( "A" );
            Assert.AreEqual( expected3.ToString(), Lambda.Ends<Test1>( "A.B.Name", "A" ).ToString() );
        }

        #endregion

        #region GetConst(获取常量表达式)

        /// <summary>
        /// 获取常量表达式
        /// </summary>
        [TestMethod]
        public void TestGetConst() {
            Expression<Func<Test1, int?>> property = t => t.NullableInt;
            ConstantExpression constantExpression = Lambda.Constant( property, 1 );
            Assert.AreEqual( typeof( int ), constantExpression.Type );
        }

        #endregion

        #region GetAttribute(获取特性)

        /// <summary>
        /// 测试获取特性
        /// </summary>
        [TestMethod]
        public void TestGetAttribute() {
            DisplayAttribute attribute = Lambda.GetAttribute<Test1, string, DisplayAttribute>( t => t.Name );
            Assert.AreEqual( "名称", attribute.Name );
        }

        #endregion

        #region GetAttributes(获取特性列表)

        /// <summary>
        /// 测试获取特性列表
        /// </summary>
        [TestMethod]
        public void TestGetAttributes() {
            IEnumerable<ValidationAttribute> attributes = Lambda.GetAttributes<Test1, string, ValidationAttribute>( t => t.Name );
            Assert.AreEqual( 2, attributes.Count() );
        }

        #endregion
    }
}
