using System;

namespace CAF
{
    public static class DoubleExt
    {
        /// <summary>
        /// 将双精度浮点值按指定的小数位数截断
        /// </summary>
        /// <param name="d">要截断的双精度浮点数</param>
        /// <param name="s">小数位数，s大于等于0，小于等于15</param>
        /// <returns></returns>
        public static double ToFixed(this double d, int s)
        {
            var sp = Math.Pow(10, s);

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }

        /// <summary>
        /// 按照位数四舍五入
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double Round(this double d, int s)
        {
            return Math.Round(d, s);
        }


        /// <summary>
        /// 是否在范围之间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="allowEqual">是否包含等于</param>
        /// <returns></returns>
        public static bool IsBetween(this double obj, double max, double min, bool allowEqual)
        {
            if (allowEqual)
            {
                return obj >= min && obj <= max;
            }
            return obj > min && obj < max;
        }
    }
}