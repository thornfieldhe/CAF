
namespace CAF.Utility
{
    public class KeyValueItem<K, T>
    {
        public KeyValueItem(K key,T t)
        {
            this.Key = key;
            this.Value = t;
        }

        public K Key { get; set; }
        public T Value { get; set; }
    }
}
