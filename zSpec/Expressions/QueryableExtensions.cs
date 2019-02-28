using System;
using System.Linq;
using System.Linq.Expressions;
using zSpec.Automation;

namespace zSpec.Expressions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T, TParam>(this IQueryable<T> queryable,
            Expression<Func<T, TParam>> prop, Expression<Func<TParam, bool>> where)
        {
            return queryable.Where(prop.Compose<Func<T, bool>>(where, Expression.AndAlso));
        }

        public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> queryable,
            IAutoFilter<TEntity> filter)
            where TEntity : class
        {
            return filter.Filter(queryable);
        }
    }
}