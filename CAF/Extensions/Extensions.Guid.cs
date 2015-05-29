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

        /// <summary>
        /// GUID为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfNotNullOrEmpty(this Guid? source, Action action)
        {
            if (!source.IsNullOrEmptuy()) { action(); }
        }

        /// <summary>
        /// GUID不为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfIsNullOrEmpty(this Guid? source, Action action)
        {
            if (source.IsNullOrEmptuy()) { action(); }
        }

        /// <summary>
        /// GUID为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfNotEmpty(this Guid source, Action action)
        {
            if (!source.IsEmptuy()) { action(); }
        }

        /// <summary>
        /// GUID不为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfIsEmpty(this Guid source, Action action)
        {
            if (source.IsEmptuy()) { action(); }
        }
    }
}
