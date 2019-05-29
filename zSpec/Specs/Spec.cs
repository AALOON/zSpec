using System;
using System.Linq.Expressions;
using zSpec.Expressions;

namespace zSpec.Specs
{
    /// <summary>
    /// Specification class which allows to encapsulate Expressions
    /// </summary>
    /// <typeparam name="TElement">Type of element in expression</typeparam>
    public class Spec<TElement>
    {
        private readonly Expression<Func<TElement, bool>> _expression;

        public Spec(Expression<Func<TElement, bool>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public static bool operator false(Spec<TElement> spec)
        {
            return false;
        }

        public static bool operator true(Spec<TElement> spec)
        {
            return true;
        }

        public static Spec<TElement> operator &(Spec<TElement> spec1, Spec<TElement> spec2)
        {
            return new Spec<TElement>(spec1._expression.And(spec2._expression));
        }

        public static Spec<TElement> operator |(Spec<TElement> spec1, Spec<TElement> spec2)
        {
            return new Spec<TElement>(spec1._expression.Or(spec2._expression));
        }

        public static Spec<TElement> operator !(Spec<TElement> spec)
        {
            return new Spec<TElement>(spec._expression.Not());
        }

        public static implicit operator Expression<Func<TElement, bool>>(Spec<TElement> spec)
        {
            return spec._expression;
        }

        public static implicit operator Spec<TElement>(Expression<Func<TElement, bool>> expression)
        {
            return new Spec<TElement>(expression);
        }

        /// <summary>
        /// Checks the if object satisfies the expressions, stores function in cache
        /// </summary>
        /// <param name="obj">element instance</param>
        public bool IsSatisfiedBy(TElement obj)
        {
            return _expression.AsFunc()(obj);
        }

        public Spec<TParent> From<TParent>(Expression<Func<TParent, TElement>> mapFrom)
        {
            return _expression.From(mapFrom);
        }

        /// <summary>
        /// Checks the if object satisfies the expressions, does NOT stores function in cache
        /// This method uses in case if you have parameters in expression
        /// </summary>
        /// <param name="obj">element instance</param>
        /// <returns></returns>
        public bool IsSatisfiedByNonCache(TElement obj)
        {
            return _expression.Compile()(obj);
        }
    }
}