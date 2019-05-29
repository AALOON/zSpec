using System;
using System.Linq.Expressions;

namespace zSpec.Automation.Predicates
{
    internal interface IPredicateInfo
    {
        bool IsOk();

        Expression<Func<TSubject, bool>> ToExpression<TSubject>(ParameterExpression parameter);
    }
}