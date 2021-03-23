using System;
using System.Linq.Expressions;
using System.Reflection;

namespace zSpec.Automation.Predicates
{
    /// <inheritdoc />
    internal class PredicateInfo : IPredicateInfo
    {
        public PropertyInfo Property { get; set; }

        public object Value { get; set; }

        public string Key { get; set; }

        /// <inheritdoc />
        public bool IsOk() => this.Value != null;

        /// <inheritdoc />
        public Expression<Func<TSubject, bool>> ToExpression<TSubject>(ParameterExpression parameter)
        {
            var property = Expression.Property(parameter, this.Property);

            var holder = new ValueHolder<object> { Value = this.Value };

            var value = Expression.Convert(
                Expression.PropertyOrField(Expression.Constant(holder), nameof(holder.Value)), property.Type);

            var body = Conventions.Filters[new TypeKey(property.Type, this.Key)](property, value);

            return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
        }
    }
}
