using System;

namespace CAF.Ext
{
    public static class ObjExt
    {
        public static void IfNotNull<K>(this object obj, Func<object, K> func)
        {
            if (obj != null) { func.Invoke(obj); }
        }
    }
}
