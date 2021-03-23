using System;
using System.Linq.Expressions;
using zSpec.Expressions;

namespace zSpec.Specs
{
    /// <summary>
    /// Extensions for Specification pattern.
    /// </summary>
    public static class SpecExtensions
    {
        public static bool IsSatisfiedBy<T>(this Func<T, bool> spec, T obj) => spec(obj);

        public static bool IsSatisfiedBy<T>(this Expression<Func<T, bool>> spec, T obj) => spec.AsFunc()(obj);

        public static bool Satisfy<T>(this T obj, Func<T, bool> spec) => spec(obj);

        public static bool SatisfyExpresion<T>(this T obj, Expression<Func<T, bool>> spec) => spec.AsFunc()(obj);
    }
}
