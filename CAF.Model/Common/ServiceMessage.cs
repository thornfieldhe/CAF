using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SweetDessert.Model
{
    [DataContract]
    public class ServiceMessage<T>
    {
        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public T Body { get; set; }

        [DataMember]
        public List<T> Bodies { get; set; }

        [DataMember]
        public string Additional { get; set; }
    }
}