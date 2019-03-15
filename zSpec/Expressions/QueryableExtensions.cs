using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zSpec.Automation;

namespace zSpec.Expressions
{
    /// <summary>
    /// Helpful extension of <see cref="IQueryable"/>
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Allows to filter by aggregation field
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TParam">Aggregation field type</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="prop">Selected property</param>
        /// <param name="where">Search expression</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Where<TEntity, TParam>(this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TParam>> prop, Expression<Func<TParam, bool>> where)
        {
            return queryable.Where(prop.Compose<Func<TEntity, bool>>(where, Expression.AndAlso));
        }

        /// <summary>
        /// Allows to filter by aggregation collection
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <typeparam name="TParam">Aggregation field type</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="prop">Selected property</param>
        /// <param name="where">Search expression</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Where<TEntity, TParam>(this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, ICollection<TParam>>> prop, Expression<Func<TParam, bool>> where)
        {
            return queryable.Where(prop.Compose<Func<TEntity, bool>>(where, Expression.AndAlso));
        }

        /// <summary>
        /// Applies filter to queryable
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="filter">Filter which will be applied</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> queryable,
            IAutoFilter<TEntity> filter)
            where TEntity : class
        {
            return filter.Filter(queryable);
        }
    }
}