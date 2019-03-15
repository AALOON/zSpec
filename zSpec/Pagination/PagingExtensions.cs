using System;
using System.Linq;
using zSpec.Extensions;

namespace zSpec.Pagination
{
    /// <summary>
    /// Paging extensions
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
            var page = Math.Max(paging.Page, 0);

            return (IOrderedQueryable<T>)queryable
                .Skip(page * paging.Take)
                .Take(paging.Take);
        }
    }
}