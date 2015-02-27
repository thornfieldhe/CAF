﻿using System;

namespace CAF
{
    public class DoubleExt
    {
        /// <summary>
        /// 将双精度浮点值按指定的小数位数截断
        /// </summary>
        /// <param name="d">要截断的双精度浮点数</param>
        /// <param name="s">小数位数，s大于等于0，小于等于15</param>
        /// <returns></returns>
        public static double ToFixed(double d, int s)
        {
            var sp = Math.Pow(10, s);

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }
    }
}