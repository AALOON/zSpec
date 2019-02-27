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
        /// Orders by by first property there are any.
        /// </summary>
        public static IOrderedQueryable<T> OrderByFirstProperty<T>(this IQueryable<T> queryable)
            => Conventions<T>.Sort(queryable, FastTypeInfo<T>.PublicProperties.First().Name);

        /// <summary>
        /// Orders by property name.
        /// </summary>
        public static IOrderedQueryable<TSubject> OrderBy<TSubject>(this IQueryable<TSubject> query, string propertyName)
            => Conventions<TSubject>.Sort(query, propertyName);
    }
}