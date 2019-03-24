using System;
using System.Linq;
using zSpec.Automation;
using zSpec.Automation.Attributes;

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
        public static IOrderedQueryable<TElement> Paginate<TElement, TPaging>(this IQueryable<TElement> queryable,
            TPaging paging) where TPaging : IPaging
        {
            if (queryable is IOrderedQueryable<TElement> orderedQueryable)
                return orderedQueryable.Paginate(paging);

            var oderBy = GetOrderColumnName<TElement, TPaging>(paging);

            var order = GetOrder(paging);

            // Changing attribute since we can not then by if not ordered
            if (order == SortOrder.AscendingThenBy)
                order = SortOrder.Ascending;
            else if (order == SortOrder.DescendingThenBy)
                order = SortOrder.Descending;

            return Conventions<TElement>.Sort(queryable, oderBy, order)
                .PaginateInternal(paging);
        }

        /// <summary>
        /// Allows to add skip and take to IOrderedQueryable
        /// </summary>
        public static IOrderedQueryable<TElement> Paginate<TElement, TPaging>(this IOrderedQueryable<TElement> orderedQueryable,
            TPaging paging) where TPaging : IPaging
        {
            if (paging == null || paging.OrderBy == null || string.IsNullOrWhiteSpace(paging.OrderBy.Column))
            {
                var skipAttribute = GetPropInfo(paging).FindAttribute<SkipOrderIfEmptyAttribute>(nameof(paging.OrderBy));
                if (skipAttribute != null)
                    return orderedQueryable.PaginateInternal(paging);
            }
            var oderBy = GetOrderColumnName<TElement, TPaging>(paging);

            orderedQueryable = Conventions<TElement>.Sort(orderedQueryable, oderBy, GetOrder(paging));

            return orderedQueryable.PaginateInternal(paging);
        }


        private static IOrderedQueryable<TElement> PaginateInternal<TElement, TPaging>(this IQueryable<TElement> queryable,
            TPaging paging) where TPaging : IPaging
        {
            var page = Math.Max(paging.Page, val2: 0);

            return (IOrderedQueryable<TElement>)queryable
                .Skip(page * paging.Take)
                .Take(paging.Take);
        }

        private static string GetOrderColumnName<TElement, TPaging>(TPaging paging)
            where TPaging : IPaging
        {
            if (paging != null && paging.OrderBy != null && !string.IsNullOrWhiteSpace(paging.OrderBy.Column))
                return paging.OrderBy.Column;

            var defaultColumnAttribute = GetPropInfo(paging)
                .FindAttribute<DefaultSortByAttribute>(nameof(paging.OrderBy));
            if (defaultColumnAttribute == null)
                return FastTypeInfo<TElement>.PublicProperties.First().Name;
            return defaultColumnAttribute.ColumnName;
        }

        private static SortOrder GetOrder<TPaging>(TPaging paging)
            where TPaging : IPaging
        {
            if (paging != null && paging.OrderBy?.Order != null)
            {
                return paging.OrderBy.Order.Value;
            }
            var defaultOrderAttribute = GetPropInfo(paging)
                .FindAttribute<DefaultSortByAttribute>(nameof(paging.OrderBy));
            if (defaultOrderAttribute == null)
                return SortOrder.Ascending;
            return defaultOrderAttribute.SortOrder;
        }

        public static FastPropInfo GetPropInfo<TPaging>(TPaging paging)
        {
            if (paging == null)
                return FastPropInfo.GetInstance(typeof(TPaging));
            return FastPropInfo.GetInstance(paging.GetType());
        }
    }
}