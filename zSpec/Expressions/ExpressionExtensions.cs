using System;
using System.Linq.Expressions;

namespace zSpec.Expressions
{
    public static class ExpressionExtensions
    {
        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
        {
            return CompiledExpressions<TIn, TOut>.AsFunc(expr);
        }

        public static bool Is<T>(this T entity, Expression<Func<T, bool>> expr)
        {
            return AsFunc(expr).Invoke(entity);
        }

        public static Expression<Func<TDestination, TReturn>> From<TSource, TDestination, TReturn>(
            this Expression<Func<TSource, TReturn>> source, Expression<Func<TDestination, TSource>> mapFrom)
        {
            return Expression.Lambda<Func<TDestination, TReturn>>(
                Expression.Invoke(source, mapFrom.Body),
                mapFrom.Parameters);
        }
    }
}