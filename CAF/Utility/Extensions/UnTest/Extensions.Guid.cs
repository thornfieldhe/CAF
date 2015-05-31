using System;

namespace CAF
{
    public partial class Extensions
    {

        /// <summary>
        /// GUID为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfNotNullOrEmpty(this Guid? source, Action action)
        {
            if (!source.IsEmpty()) { action(); }
        }

        /// <summary>
        /// GUID不为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfIsNullOrEmpty(this Guid? source, Action action)
        {
            if (source.IsEmpty()) { action(); }
        }

        /// <summary>
        /// GUID为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfNotEmpty(this Guid source, Action action)
        {
            if (!source.IsEmpty()) { action(); }
        }

        /// <summary>
        /// GUID不为空则执行 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfIsEmpty(this Guid source, Action action)
        {
            if (source.IsEmpty()) { action(); }
        }
    }
}
