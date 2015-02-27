using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CAF
{
    public static class ObjectCloner
    {
        public static T DeepCopy<T>(T t)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);
                stream.Seek(0, SeekOrigin.Begin);
                var copy = (T)formatter.Deserialize(stream);
                stream.Close();
                return copy;
            }
        }
    }
}
