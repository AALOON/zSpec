using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using zSpec.Automation.Attributes;

namespace zSpec.Automation
{
    public class ConventionalFilters
    {
        private static readonly MethodInfo StartsWithMethod = typeof(string)
            .GetMethod("StartsWith", new[] { typeof(string) });

        private static readonly MethodInfo ContainsMethod = typeof(string)
            .GetMethod("Contains", new[] { typeof(string) });

        private static readonly Dictionary<TypeKey, Func<MemberExpression, Expression, Expression>> Filters
            = new Dictionary<TypeKey, Func<MemberExpression, Expression, Expression>>
            {
                {
                    new TypeKey(typeof(string), StartWithFilterAttribute.Key),
                    (p, v) => Expression.Call(p, StartsWithMethod, v)
                },
                {
                    new TypeKey(typeof(string), ContainsFilterAttribute.Key),
                    (p, v) => Expression.Call(p, ContainsMethod, v)
                },
                { new TypeKey(typeof(string), string.Empty), Expression.Equal },
                { new TypeKey(typeof(DateTime), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(DateTime), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(DateTimeOffset), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(DateTimeOffset), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(int), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(int), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(uint), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(uint), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(long), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(long), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(ulong), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(ulong), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(short), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(short), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(ushort), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(ushort), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(byte), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(byte), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(sbyte), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(sbyte), ToFilterAttribute.Key), Expression.LessThanOrEqual },

                { new TypeKey(typeof(DateTime?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(DateTime?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(DateTimeOffset?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(DateTimeOffset?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(int?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(int?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(uint?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(uint?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(long?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(long?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(ulong?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(ulong?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(short?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(short?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(ushort?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(ushort?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(byte?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(byte?), ToFilterAttribute.Key), Expression.LessThanOrEqual },
                { new TypeKey(typeof(sbyte?), FromFilterAttribute.Key), Expression.GreaterThanOrEqual },
                { new TypeKey(typeof(sbyte?), ToFilterAttribute.Key), Expression.LessThanOrEqual }
            };

        internal ConventionalFilters()
        {
        }

        public static Dictionary<Type, string> AttributeKeys { get; } = new Dictionary<Type, string>
        {
            { typeof(StartWithFilterAttribute), StartWithFilterAttribute.Key },
            { typeof(ContainsFilterAttribute), ContainsFilterAttribute.Key },
            { typeof(FromFilterAttribute), FromFilterAttribute.Key },
            { typeof(ToFilterAttribute), ToFilterAttribute.Key }
        };

        public Func<MemberExpression, Expression, Expression> this[(Type Type, string Key) key]
        {
            get => this[new TypeKey(key.Type, key.Key)];
            set => Filters[new TypeKey(key.Type, key.Key)] = value ?? throw new ArgumentException(nameof(value));
        }

        public Func<MemberExpression, Expression, Expression> this[TypeKey key]
        {
            get => Filters.ContainsKey(key) ? Filters[key] : Expression.Equal;
            set => Filters[key] = value ?? throw new ArgumentException(nameof(value));
        }
    }
}