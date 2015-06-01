using System.IO;

namespace CAF.Utility {
    /// <summary>
    /// 图片操作
    /// </summary>
    public class Image {
        /// <summary>
        /// 图片文件的绝对路径
        /// </summary>
        /// <param name="filePath">图片文件的绝对路径</param>
        public static Image FromFile( string filePath ) {
            return Image.FromFile( filePath );
        }

        /// <summary>
        /// 图片文件的绝对路径
        /// </summary>
        /// <param name="stream">流</param>
        public static System.Drawing.Image FromStream(Stream stream)
        {
            return Image.FromStream( stream );
        }

        /// <summary>
        /// 图片文件的绝对路径
        /// </summary>
        /// <param name="buffer">字节流</param>
        public static System.Drawing.Image FromStream(byte[] buffer)
        {
            using( var stream = new MemoryStream(buffer) ) {
                return FromStream( stream );
            }
        }
    }
}
