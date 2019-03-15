using System.Linq;
using zSpec.Automation;

namespace zSpec.Extensions
{
    /// <summary>
    /// Additional extensions of IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Orders by first property if there are any.
        /// </summary>
        public static IOrderedQueryable<T> OrderByFirstProperty<T>(this IQueryable<T> queryable)
            => Conventions<T>.Sort(queryable, FastTypeInfo<T>.PublicProperties.First().Name);

        /// <summary>
        /// Orders by Descending first property if there are any.
        /// </summary>
        public static IOrderedQueryable<T> OrderByDescendingFirstProperty<T>(this IQueryable<T> queryable)
            => Conventions<T>.Sort(queryable, FastTypeInfo<T>.PublicProperties.First().Name, SortOrder.Descending);

        /// <summary>
        /// Orders by property name.
        /// </summary>
        public static IOrderedQueryable<TSubject> OrderBy<TSubject>(this IQueryable<TSubject> query, string propertyName)
            => Conventions<TSubject>.Sort(query, propertyName);

        /// <summary>
        /// Orders by Descending property name.
        /// </summary>
        public static IOrderedQueryable<TSubject> OrderByDescending<TSubject>(this IQueryable<TSubject> query, string propertyName)
            => Conventions<TSubject>.Sort(query, propertyName, SortOrder.Descending);
    }
}