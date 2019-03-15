using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace zSpec.Expressions
{
    internal static class CompiledExpressions<TIn, TOut>
    {
        private static readonly ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>> Cache
            = new ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>>();

        internal static Func<TIn, TOut> AsFunc(Expression<Func<TIn, TOut>> expr)
        {
            return Cache.GetOrAdd(expr, k => k.Compile());
        }
    }
}