using System.Linq;
using zSpec.Extensions;

namespace zSpec.Pagination
{
    /// <summary>
    /// Paging extenensions
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// Allows to add skip and take and also may be sort to IQueryable
        /// </summary>
        public static IOrderedQueryable<T> Paginate<T>(this IQueryable<T> queryable, IPaging paging)
        {
            var orderedQueryable = queryable as IOrderedQueryable<T>
                                   ?? (string.IsNullOrWhiteSpace(paging.OrderBy)
                                       ? queryable.OrderByFirstProperty()
                                       : queryable.OrderBy(paging.OrderBy));

            return orderedQueryable.Paginate(paging);
        }

        /// <summary>
        /// Allows to add skip and take to IOrderedQueryable
        /// </summary>
        public static IOrderedQueryable<T> Paginate<T>(this IOrderedQueryable<T> queryable, IPaging paging)
        {
            return (IOrderedQueryable<T>)queryable
                .Skip((paging.Page - 1) * paging.Take)
                .Take(paging.Take);
        }
    }
}