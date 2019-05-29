using System;
using System.Linq.Expressions;
using System.Reflection;

namespace zSpec.Automation.Predicates
{
    internal class PredicateInfo : IPredicateInfo
    {
        public PropertyInfo Property { get; set; }

        public object Value { get; set; }

        public string Key { get; set; }

        /// <inheritdoc />
        public bool IsOk()
        {
            return Value != null;
        }

        public Expression<Func<TSubject, bool>> ToExpression<TSubject>(ParameterExpression parameter)
        {
            var property = Expression.Property(parameter, Property);
            Expression value = Expression.Constant(Value);

            value = Expression.Convert(value, property.Type);
            var body = Conventions.Filters[new TypeKey(property.Type, Key)](property, value);

            return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
        }
    }
}