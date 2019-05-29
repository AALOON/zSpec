using System;
using System.Linq.Expressions;
using System.Reflection;
using zSpec.Automation.Attributes;
using zSpec.Expressions;

namespace zSpec.Automation.Predicates
{
    internal class ComplexPredicateInfo : IPredicateInfo
    {
        public PropertyInfo Property { get; set; }

        public Array Value { get; set; }

        public string Key { get; set; }

        public MultiValueAttribute Attribute { get; set; }

        /// <inheritdoc />
        public bool IsOk()
        {
            return Value != null && Value.Length > 0;
        }

        public Expression<Func<TSubject, bool>> ToExpression<TSubject>(ParameterExpression parameter)
        {
            var property = Expression.Property(parameter, Property);

            Expression<Func<TSubject, bool>> aggregatedExpression = null;

            var composeKind = Attribute.ComposeKind;

            foreach (var oneValue in Value)
            {
                Expression value = Expression.Constant(oneValue);
                value = Expression.Convert(value, property.Type);
                var body = Conventions.Filters[new TypeKey(property.Type, Key)](property, value);

                if (aggregatedExpression == null)
                {
                    aggregatedExpression = Expression.Lambda<Func<TSubject, bool>>(body, parameter);
                }
                else
                {
                    aggregatedExpression = composeKind == ComposeKind.And
                        ? aggregatedExpression.And(Expression.Lambda<Func<TSubject, bool>>(body, parameter))
                        : aggregatedExpression.Or(Expression.Lambda<Func<TSubject, bool>>(body, parameter));
                }
            }

            return aggregatedExpression ?? throw new ArgumentNullException(Property.Name);
        }
    }
}