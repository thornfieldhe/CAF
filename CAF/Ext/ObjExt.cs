using System;

namespace CAF.Ext
{
    public static class ObjExt
    {
        public static void IfIsNotNull(this object allowExcuteAction, Action action)
        {
            if (allowExcuteAction != null) { action.Invoke(); }
        }
    }
}
