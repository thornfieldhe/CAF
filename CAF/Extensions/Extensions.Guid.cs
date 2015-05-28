using System;

namespace CAF
{
    public partial class Extensions
    {
        /// <summary>
        /// 是否为空白GUID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmptuy(this Guid obj) { return obj == new Guid(); }

        /// <summary>
        /// 是否为空白GUID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmptuy(this Guid? obj) { return obj.HasValue && obj.Value == new Guid(); }

        /// <summary>
        /// 是否为空白GUID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptuy(this Guid? obj)
        {
            if (!obj.HasValue)
            {
                return true;
            }
            return obj.Value == new Guid();

        }
    }
}
