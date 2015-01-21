using System;

namespace CAF
{
    /// <summary>
    /// 结果为true就执行语句
    /// </summary>
    public static class BoolExt
    {
        public static void IfIsTrue(this bool allowExcuteAction, Action action)
        {
            if (allowExcuteAction) { action.Invoke(); }
        }

    }
}
