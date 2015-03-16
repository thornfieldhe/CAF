using System.Collections.Generic;

namespace CAF.Ext
{
    public static class ListExt
    {
        public static bool HasItem<T>(this List<T> list) { return list != null && list.Count > 0; }
    }
}
