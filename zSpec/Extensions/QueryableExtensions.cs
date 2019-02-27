using System.Linq;
using zSpec.Automation;

namespace zSpec.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderByFirstProperty<T>(this IQueryable<T> queryable)
            => Conventions<T>.Sort(queryable, FastTypeInfo<T>.PublicProperties.First().Name);
    }
}