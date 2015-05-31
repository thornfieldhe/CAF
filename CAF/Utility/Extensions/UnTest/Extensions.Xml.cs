using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAF
{
    /// <summary>
    /// Xml操作辅助类
    /// </summary>
    public partial class Extensions
    {
        /// <summary>
        /// 序列化实例到Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(this object entity) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var writer = new StringWriter();
            serializer.Serialize(writer, entity);
            return writer.ToString();
        }

        /// <summary>
        /// 反序列化Xml到实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmleDserialize<T>(this string xml) where T : class
        {
            XmlReader rdr = new System.Xml.XmlTextReader(xml);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(rdr);
        }
    }
}