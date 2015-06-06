using System;

namespace CAF.Mapper
{
    using System.Linq.Expressions;

    /// <summary>
    /// When overriden it provides functionalities to map from object to object (source to destination) using convention based mapping and a set of custom user rules mapping.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface IMapper<TSource, TDestination> : IMapper where TDestination : new()
    {
        /// <summary>
        /// Conditionally maps the specified destination member from the source instance if found according to conditionalAction value.
        /// </summary>
        /// <param name="destinationMember">The member on destination which will be mapped (its value updated)</param>
        /// <param name="conditionalAction">The action which its value will determine if to ignore or not mapping this member</param>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> Conditional(Expression<Func<TDestination, object>> destinationMember, Func<TSource, bool> conditionalAction);
        /// <summary>
        /// Maps the specified destination member using the specified resolver.
        /// </summary>
        /// <param name="destinationMember">The member on destination which will be mapped (its value updated)</param>
        /// <param name="resolver">The func action which will return the value and update the destination member</param>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> For<T>(Expression<Func<TDestination, T>> destinationMember, Func<TSource, T> resolver);
        /// <summary>
        /// Maps the specified destination member using the specified resolver if the conditionalAction evaluated to true
        /// </summary>
        /// <param name="destinationMember">The member on destination which will be mapped (its value updated)</param>
        /// <param name="resolver">The func action which will return the value and update the destination member</param>
        /// <param name="conditionalAction">The action which its value will determine if to map using the resolver specified or not mapping this member</param>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> ForIf<T>(Expression<Func<TDestination, T>> destinationMember, Func<TSource, T> resolver, Func<TSource, bool> conditionalAction);
        /// <summary>
        /// Ignores mapping of the specified destination member
        /// </summary>
        /// <param name="destinationMember">The member on destination which will be ignored from mapping</param>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> Ignore(Expression<Func<TDestination, object>> destinationMember);
        /// <summary>
        /// Conditionally ignores the specified destination member from mapping
        /// </summary>
        /// <param name="destinationMember">The member on destination which will be ignored from mapping</param>
        /// <param name="conditionalAction">The action which its value will determine if to ignore or not mapping this member</param>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> IgnoreIf(Expression<Func<TDestination, object>> destinationMember, Func<TSource, bool> conditionalAction);
        /// <summary>
        /// Adds the specified mapper to the list of mappers that will be used in case a name match and types match found during mapping.
        /// </summary>
        /// <typeparam name="TSrc">Type of source instance to map from</typeparam>
        /// <typeparam name="TDest">Type of destination instance to map to</typeparam>
        /// <param name="subMapper">An instance of a mapper that will be used as a sub mapper in the current mapper in case a match is found for mapping</param>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> UseMapper<TSrc, TDest>(IMapper<TSrc, TDest> subMapper) where TDest : new();
        /// <summary>
        /// Creates and adds a new mapper to list of mappers that will be used in case a name match and types match found during mapping.
        /// </summary>
        /// <typeparam name="TSrc">Type of source instance to map from</typeparam>
        /// <typeparam name="TDest">Type of destination instance to map to</typeparam>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> UseMapper<TSrc, TDest>() where TDest : new();
        /// <summary>
        /// Use this method to do custom actions through the mapper, handy to tigh custom resolvings with the mapper. Resolvers are the last things to be executed throught the mapper.
        /// </summary>
        /// <param name="resolver">Custom action to do whatever on the mapper</param>
        /// <returns>Returns instance of IMapper for chaining purposes</returns>
        IMapper<TSource, TDestination> Resolve(Action<TSource, TDestination> resolver);
        /// <summary>
        /// Executes mapping and returns the destination instance mapped using the mapping rules specified in the mapper instance.
        /// </summary>
        /// <param name="source">The source instance to map from</param>
        /// <returns>The destination instance which will be returned from the process of mapping</returns>
        TDestination Map(TSource source);
        /// <summary>
        /// Executes mapping between source and destination instances
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        TDestination Map(TSource source, TDestination dest);
    }
}
