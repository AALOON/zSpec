using System.Linq;
using zSpec.Automation;
using zSpec.Extensions;

namespace zSpec.Pagination
{
    public static class PagingExtensions
    {
        public static IOrderedQueryable<T> Paginate<T>(this IQueryable<T> queryable, IPaging paging)
        {
            var orderedQueryable = queryable as IOrderedQueryable<T>
                                   ?? (string.IsNullOrWhiteSpace(paging.OrderBy)
                                       ? queryable.OrderByFirstProperty()
                                       : queryable.OrderBy(paging.OrderBy));

            return orderedQueryable.Paginate(paging);
        }

        public static IOrderedQueryable<T> Paginate<T>(this IOrderedQueryable<T> queryable, IPaging paging)
        {
            return (IOrderedQueryable<T>)queryable
                .Skip((paging.Page - 1) * paging.Take)
                .Take(paging.Take);
        }

        public static IOrderedQueryable<TSubject> OrderBy<TSubject>(this IQueryable<TSubject> query, string propertyName)
        {
            return Conventions<TSubject>.Sort(query, propertyName);
        }
    }
}