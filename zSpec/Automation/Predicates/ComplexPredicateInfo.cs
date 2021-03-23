using System;
using System.Linq.Expressions;
using System.Reflection;
using zSpec.Automation.Attributes;
using zSpec.Expressions;

namespace zSpec.Automation.Predicates
{
    /// <summary>
    /// Information about complex predicate. 
    /// </summary>
    internal class ComplexPredicateInfo : IPredicateInfo
    {
        public PropertyInfo Property { get; set; }

        public Array Value { get; set; }

        public string Key { get; set; }

        public MultiValueAttribute Attribute { get; set; }

        /// <inheritdoc />
        public bool IsOk() => this.Value != null && this.Value.Length > 0;

        /// <inheritdoc />
        public Expression<Func<TSubject, bool>> ToExpression<TSubject>(ParameterExpression parameter)
        {
            var property = Expression.Property(parameter, this.Property);

            Expression<Func<TSubject, bool>> aggregatedExpression = null;

            var composeKind = this.Attribute.ComposeKind;

            foreach (var oneValue in this.Value)
            {
                var holder = new ValueHolder<object> { Value = oneValue };

                var value = Expression.Convert(
                    Expression.PropertyOrField(Expression.Constant(holder), nameof(holder.Value)), property.Type);

                var body = Conventions.Filters[new TypeKey(property.Type, this.Key)](property, value);

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

            return aggregatedExpression ?? throw new ArgumentNullException(this.Property.Name);
        }
    }
}
