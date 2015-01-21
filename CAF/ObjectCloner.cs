using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CAF
{
    public static class ObjectCloner
    {
        public static T DeepCopy<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);
                stream.Seek(0, SeekOrigin.Begin);
                T copy = (T)formatter.Deserialize(stream);
                stream.Close();
                return copy;
            }
        }
    }
}
