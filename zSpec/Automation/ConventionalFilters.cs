using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace zSpec.Automation
{
    public class ConventionalFilters
    {
        private static readonly MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] { typeof(string) });

        private static readonly Dictionary<Type, Func<MemberExpression, Expression, Expression>> Filters
            = new Dictionary<Type, Func<MemberExpression, Expression, Expression>>
            {
                { typeof(string),  (p, v) => Expression.Call(p, StartsWith, v) }
            };

        internal ConventionalFilters()
        {
        }

        public Func<MemberExpression, Expression, Expression> this[Type key]
        {
            get => Filters.ContainsKey(key)
                ? Filters[key]
                : Expression.Equal;
            set => Filters[key] = value ?? throw new ArgumentException(nameof(value));
        }
    }
}