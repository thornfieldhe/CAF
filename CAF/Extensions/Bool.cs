using System;

namespace CAF
{
    /// <summary>
    /// 结果为true就执行语句
    /// </summary>
    public static class Bool
    {
        public static void IfIsTrue(this bool allowExcuteAction, Action action)
        {
            if (allowExcuteAction) { action.Invoke(); }
        }
        public static void IfIsFalse(this bool allowExcuteAction, Action action)
        {
            if (!allowExcuteAction) { action.Invoke(); }
        }
    }
}
