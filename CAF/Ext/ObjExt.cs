using System;

namespace CAF.Ext
{
    public static class ObjExt
    {
        public static void IfNotNull<T, K>(this T obj, Func<T, K> func)
        {
            if (obj != null) { func.Invoke(obj); }
        }
        public static void IfNull(this object obj, Action action)
        {
            if (obj == null) { action.Invoke(); }
        }
    }
}
