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

        /// <summary>
        /// 转换成int值
        /// true:1
        /// false:0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this bool obj) { return obj ? 1 : 0; }

    }
}
