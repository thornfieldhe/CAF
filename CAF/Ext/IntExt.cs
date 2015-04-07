﻿namespace CAF.Ext
{
    using System.Runtime.InteropServices;

    public static class IntExt
    {
        /// <summary>
        /// 转换成bool值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBool(this int obj) { return obj==1?true:false ; }

        /// <summary>
        /// 是否在范围之间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="allowEqual">是否包含等于</param>
        /// <returns></returns>
        public static bool IsBetween(this int obj,int max,int min,bool allowEqual)
        {
            if (allowEqual)
            {
                return obj >= min && obj <= max;
            }
            return obj > min && obj < max;
        }
    }
}