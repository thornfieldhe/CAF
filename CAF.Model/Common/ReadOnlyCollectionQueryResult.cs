namespace CAF
{
    using System.Collections.Generic;

    public class ReadOnlyCollectionQueryResult<K>
    {

        public int TotalCount { get; internal set; }

        public int PageSize { get; internal set; }

        public int PageIndex { get; internal set; }

        public Dictionary<string, object> Sum { get; internal set; }

        public Dictionary<string, object> Average { get; internal set; }

        public List<K> Result { get; internal set; }
    }
}