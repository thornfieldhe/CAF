using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAF.Tests.Files {
    /// <summary>
    /// 流类型转换操作测试
    /// </summary>
    [TestClass]
    public class StreamConvertTest {

        #region StringToBytes(字符串转换成字节数组)

        /// <summary>
        ///字符串转换成字节数组,值为null
        ///</summary>
        [TestMethod]
        public void TestStringToBytes_Null() {
            Assert.AreEqual( 0, CAF.Utility.File.StringToBytes( null ).Count() );
        }

        /// <summary>
        ///字符串转换成字节数组,值为空字符串
        ///</summary>
        [TestMethod]
        public void TestStringToBytes_Empty() {
            Assert.AreEqual( 0, CAF.Utility.File.StringToBytes( " " ).Count() );
        }

        /// <summary>
        ///字符串转换成字节数组
        ///</summary>
        [TestMethod]
        public void TestStringToBytes() {
            var result = CAF.Utility.File.StringToBytes( "ab" );
            Assert.AreEqual( 97, result[0] );
            Assert.AreEqual( 98, result[1] );
        }

        /// <summary>
        ///字符串转换成字节数组,指定字符编码
        ///</summary>
        [TestMethod]
        public void TestStringToBytes_Encoding() {
            var result = CAF.Utility.File.StringToBytes( "ab", Encoding.GetEncoding( "utf-8" ) );
            Assert.AreEqual( 97, result[0] );
            Assert.AreEqual( 98, result[1] );
        }

        #endregion

        #region BytesToString(字节数组转换成字符串)

        /// <summary>
        ///字节数组转换成字符串,值为null
        ///</summary>
        [TestMethod]
        public void TestBytesToString_Null() {
            Assert.AreEqual( string.Empty, CAF.Utility.File.BytesToString( null ) );
        }

        /// <summary>
        ///字节数组转换成字符串,值为空字符串
        ///</summary>
        [TestMethod]
        public void TestBytesToString_Empty() {
            Assert.AreEqual( string.Empty, CAF.Utility.File.BytesToString( new byte[] { } ) );
        }

        /// <summary>
        ///字节数组转换成字符串
        ///</summary>
        [TestMethod]
        public void TestBytesToString() {
            var result = CAF.Utility.File.BytesToString( new byte[] { 97, 98 } );
            Assert.AreEqual( "ab", result );
        }

        /// <summary>
        ///字节数组转换成字符串,指定字符编码
        ///</summary>
        [TestMethod]
        public void TestBytesToString_Encoding() {
            var result = CAF.Utility.File.BytesToString( new byte[] { 97, 98 }, Encoding.GetEncoding( "utf-8" ) );
            Assert.AreEqual( "ab", result );
        }

        #endregion

        #region StreamToString(流转换成字符串)

        /// <summary>
        ///流转换成字符串,值为null
        ///</summary>
        [TestMethod]
        public void TestStreamToString_Null() {
            Assert.AreEqual( string.Empty, CAF.Utility.File.StreamToString( null ) );
        }

        /// <summary>
        ///流转换成字符串
        ///</summary>
        [TestMethod]
        public void TestStreamToString() {
            using ( var stream = new MemoryStream( new byte[] { 97, 98 } ) ) {
                Assert.AreEqual( "ab", CAF.Utility.File.StreamToString( stream ) );
            }
        }

        /// <summary>
        ///流转换成字符串
        ///</summary>
        [TestMethod]
        public void TestStreamToString_Encoding() {
            using ( var stream = new MemoryStream( new byte[] { 97, 98 } ) ) {
                Assert.AreEqual( "ab", CAF.Utility.File.StreamToString( stream, Encoding.GetEncoding( "utf-8" ) ) );
            }
        }

        #endregion

        #region StringToStream(字符串转换成流)

        /// <summary>
        ///字符串转换成流,值为null
        ///</summary>
        [TestMethod]
        public void TestStringToStream_Null() {
            Assert.AreEqual( Stream.Null, CAF.Utility.File.StringToStream( null ) );
        }

        /// <summary>
        ///字符串转换成流,值为空字符串
        ///</summary>
        [TestMethod]
        public void TestStringToStream_Empty() {
            Assert.AreEqual( Stream.Null, CAF.Utility.File.StringToStream( " " ) );
        }

        /// <summary>
        ///字符串转换成流
        ///</summary>
        [TestMethod]
        public void TestStringToStream() {
            var result = CAF.Utility.File.StringToStream( "ab" );
            byte[] bytes = new byte[2];
            result.Read( bytes, 0, 2 );
            Assert.AreEqual( 97, bytes[0] );
            Assert.AreEqual( 98, bytes[1] );
        }

        /// <summary>
        ///字符串转换成流,指定字符编码
        ///</summary>
        [TestMethod]
        public void TestStringToStream_Encoding() {
            var result = CAF.Utility.File.StringToStream( "ab", Encoding.GetEncoding( "utf-8" ) );
            byte[] bytes = new byte[2];
            result.Read( bytes, 0, 2 );
            Assert.AreEqual( 97, bytes[0] );
            Assert.AreEqual( 98, bytes[1] );
        }

        #endregion

        #region BytesToInt(字节数组转换成整数)

        /// <summary>
        ///字节数组转换成整数,值为null
        ///</summary>
        [TestMethod]
        public void TestBytesToInt_Null() {
            Assert.AreEqual( 0, CAF.Utility.File.BytesToInt( null ) );
        }

        /// <summary>
        ///字节数组转换成整数,值为空字符串
        ///</summary>
        [TestMethod]
        public void TestBytesToInt_Empty() {
            Assert.AreEqual( 0, CAF.Utility.File.BytesToInt( new byte[] { } ) );
        }

        /// <summary>
        ///字节数组转换成整数,无效值
        ///</summary>
        [TestMethod]
        public void TestBytesToInt_Invalid() {
            Assert.AreEqual( 0, CAF.Utility.File.BytesToInt( new byte[] { 0, 1, 3 } ) );
        }

        /// <summary>
        /// 字节数组转换成整数
        ///</summary>
        [TestMethod()]
        public void TestBytesToInt() {
            Assert.AreEqual( 256, CAF.Utility.File.BytesToInt( new byte[] { 0, 1, 0, 0 } ) );
        }

        #endregion

        #region TestStreamToBytes(流转换为字节流)

        /// <summary>
        /// 流转换为字节流
        /// </summary>
        [TestMethod]
        public void TestStreamToBytes() {
            using( var stream = new MemoryStream( new byte[] {97, 98} ) ) {
                Assert.AreEqual( 97, CAF.Utility.File.StreamToBytes( stream )[0] );
                Assert.AreEqual( 98, CAF.Utility.File.StreamToBytes( stream )[1] );
            }
        }

        #endregion
    }
}
