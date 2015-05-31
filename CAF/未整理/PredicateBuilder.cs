﻿using System;
using System.Linq;

namespace CAF.Utility
{
    using System.Linq.Expressions;

    /// <summary>
    /// 谓词过滤器
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Presents a true predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return x => true; }
        /// <summary>
        /// Presents false predicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return x => false; }
        /// <summary>
        /// Ors a predicate with another
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            var expInvoked = Expression.Invoke(expRight, expLeft.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expLeft.Body, expInvoked), expLeft.Parameters);
        }
        /// <summary>
        /// And a predicate with another
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            var expInvoked = Expression.Invoke(expRight, expLeft.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expLeft.Body, expInvoked), expLeft.Parameters);
        }
        /// <summary>
        /// XOring a predicate with another
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Xor<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            var expInvoked = Expression.Invoke(expRight, expLeft.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.ExclusiveOr(expLeft.Body, expInvoked), expLeft.Parameters);
        }
    }
}
