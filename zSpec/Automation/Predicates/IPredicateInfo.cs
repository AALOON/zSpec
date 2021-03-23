using System;
using System.Linq.Expressions;

namespace zSpec.Automation.Predicates
{
    /// <summary>
    /// Predicate information.
    /// </summary>
    internal interface IPredicateInfo
    {
        /// <summary>
        /// Validation of expression.
        /// </summary>
        bool IsOk();

        /// <summary>
        /// Convert predication information to expression.
        /// </summary>
        Expression<Func<TSubject, bool>> ToExpression<TSubject>(ParameterExpression parameter);
    }
}
