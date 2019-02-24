using System;
using System.Linq.Expressions;
using zSpec.Expressions;

namespace zSpec.Specs
{
    public static class SpecExtenions
    {
        public static bool Satisfy<T>(this T obj, Func<T, bool> spec)
        {
            return spec(obj);
        }

        public static bool SatisfyExpresion<T>(this T obj, Expression<Func<T, bool>> spec)
        {
            return spec.AsFunc()(obj);
        }

        public static bool IsSatisfiedBy<T>(this Func<T, bool> spec, T obj)
        {
            return spec(obj);
        }

        public static bool IsSatisfiedBy<T>(this Expression<Func<T, bool>> spec, T obj)
        {
            return spec.AsFunc()(obj);
        }
    }
}