using System;
using System.Linq.Expressions;
using zSpec.Expressions;

namespace zSpec.Specs
{
    /// <summary>
    /// Specification class which allows to encapsulate Expressions.
    /// </summary>
    /// <typeparam name="TElement">Type of element in expression.</typeparam>
    public class Spec<TElement>
    {
        private readonly Expression<Func<TElement, bool>> expression;

        public Spec(Expression<Func<TElement, bool>> expression) =>
            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));

        public Spec<TParent> From<TParent>(Expression<Func<TParent, TElement>> mapFrom) =>
            this.expression.From(mapFrom);

        /// <summary>
        /// Checks the if object satisfies the expressions, stores function in cache
        /// </summary>
        /// <param name="obj">element instance</param>
        public bool IsSatisfiedBy(TElement obj) => this.expression.AsFunc()(obj);

        /// <summary>
        /// Checks the if object satisfies the expressions, does NOT stores function in cache
        /// This method uses in case if you have parameters in expression
        /// </summary>
        /// <param name="obj">element instance</param>
        /// <returns></returns>
        public bool IsSatisfiedByNonCache(TElement obj) => this.expression.Compile()(obj);

        public static Spec<TElement> operator &(Spec<TElement> spec1, Spec<TElement> spec2) =>
            new(spec1.expression.And(spec2.expression));

        public static Spec<TElement> operator |(Spec<TElement> spec1, Spec<TElement> spec2) =>
            new(spec1.expression.Or(spec2.expression));

        public static bool operator false(Spec<TElement> spec) => false;

        public static implicit operator Expression<Func<TElement, bool>>(Spec<TElement> spec) => spec.expression;

        public static implicit operator Spec<TElement>(Expression<Func<TElement, bool>> expression) => new(expression);

        public static Spec<TElement> operator !(Spec<TElement> spec) => new(spec.expression.Not());

        public static bool operator true(Spec<TElement> spec) => true;
    }
}
