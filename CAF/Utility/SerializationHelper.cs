using System;
using System.IO;

namespace CAF.Utility
{
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Formatters.Soap;

    public static class SerializationHelper
    {
        private const FormatterType DefaultFormatterType = FormatterType.Binary;
        /// 按照串行化的编码要求，生成对应的编码器    
        private static IRemotingFormatter GetFormatter(FormatterType formatterType)
        {
            switch (formatterType)
            {
                case FormatterType.Binary:
                    return new BinaryFormatter();
                case FormatterType.Soap:
                    return new SoapFormatter();
            }
            throw new NotSupportedException();
        }

        /// 把对象序列化转换为字符串    
        public static string SerializeObjectToString(object graph, FormatterType formatterType)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = GetFormatter(formatterType);
                formatter.Serialize(memoryStream, graph);
                var arrGraph = memoryStream.ToArray();
                return Convert.ToBase64String(arrGraph);
            }
        }

        public static string SerializeObjectToString(object graph)
        {
            return SerializeObjectToString(graph, DefaultFormatterType);
        }

        /// 把已序列化为字符串类型的对象反序列化为指定的类型 
        public static T DeserializeStringToObject<T>(string graph, FormatterType formatterType)
        {
            var arrGraph = Convert.FromBase64String(graph);
            using (var memoryStream = new MemoryStream(arrGraph))
            {
                var formatter = GetFormatter(formatterType);
                return (T)formatter.Deserialize(memoryStream);
            }
        }

        public static T DeserializeStringToObject<T>(string graph)
        {
            return DeserializeStringToObject<T>(graph, DefaultFormatterType);
        }
    }
    public enum FormatterType { Soap, Binary }
}
