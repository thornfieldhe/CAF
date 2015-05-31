﻿using System;

namespace CAF
{
    /// <summary>
    /// Set of very useful extension methods for hour by hour use in .NET code.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Negates a condition
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool Not(this bool @this)
        {
            return !@this;
        }
        /// <summary>
        /// Anding with a condition
        /// </summary>
        /// <param name="this"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool And(this bool @this, bool right)
        {
            return @this && right;
        }
        /// <summary>
        /// Anding with a condition predicate
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool And(this bool @this, Func<bool> action)
        {
            return @this && action();
        }
        /// <summary>
        /// Anding Not with a condition
        /// </summary>
        /// <param name="this"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool AndNot(this bool @this, bool right)
        {
            return @this && !right;
        }
        /// <summary>
        /// Anding Not with a condition predicate
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool AndNot(this bool @this, Func<bool> action)
        {
            return @this && !action();
        }
        /// <summary>
        /// Oring with a condition
        /// </summary>
        /// <param name="this"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Or(this bool @this, bool right)
        {
            return @this || right;
        }
        /// <summary>
        /// Oring with a condition predicate
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Or(this bool @this, Func<bool> action)
        {
            return @this || action();
        }
        /// <summary>
        /// Oring Not with a condition
        /// </summary>
        /// <param name="this"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool OrNot(this bool @this, bool right)
        {
            return @this || !right;
        }
        /// <summary>
        /// Oring Not with a condition predicate
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool OrNot(this bool @this, Func<bool> action)
        {
            return @this || !action();
        }
        /// <summary>
        /// Xoring with a condition
        /// </summary>
        /// <param name="this"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Xor(this bool @this, bool right)
        {
            return @this ^ right;
        }
        /// <summary>
        /// 异或，只有一个为真时为真
        /// </summary>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Xor(this bool @this, Func<bool> action)
        {
            return @this ^ action();
        }
    }
}
