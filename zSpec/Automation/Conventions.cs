using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using zSpec.Automation.Attributes;
using zSpec.Automation.Predicates;
using zSpec.Expressions;

namespace zSpec.Automation
{
    internal static class Conventions
    {
        internal static readonly IReadOnlyDictionary<SortOrder, MethodInfo> OrderMethods;

        static Conventions()
        {
            var methods = typeof(Queryable).GetMethods();
            OrderMethods = new Dictionary<SortOrder, MethodInfo>
            {
                [SortOrder.Ascending] = methods.First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2),
                [SortOrder.Descending] =
                    methods.First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2),
                [SortOrder.AscendingThenBy] = methods.First(x => x.Name == "ThenBy" && x.GetParameters().Length == 2),
                [SortOrder.DescendingThenBy] =
                    methods.First(x => x.Name == "ThenByDescending" && x.GetParameters().Length == 2)
            };
        }

        public static ConventionalFilters Filters { get; } = new();
    }

    public static class Conventions<TSubject>
    {
        public static IQueryable<TSubject> Filter<TPredicate>(IQueryable<TSubject> query,
            TPredicate predicate,
            ComposeKind composeKind = ComposeKind.And)
        {
            var filterMap = FastTypeInfo<TPredicate>.PublicPropertiesMap;

            var modelType = typeof(TSubject);

            var parameter = Expression.Parameter(modelType);
            var propByColumnMap = FastPropInfo<TPredicate>.PropertiesByColumnMap;

            var props = FastTypeInfo<TSubject>.PublicProperties
                .Where(info => propByColumnMap.ContainsKey(info.Name))
                .SelectMany(propertyInfo =>
                    ExtractPredicateInfo(predicate, propertyInfo, propByColumnMap[propertyInfo.Name], filterMap))
                .Where(predicateInfo => predicateInfo.IsOk())
                .Select(predicateInfo => predicateInfo.ToExpression<TSubject>(parameter))
                .ToArray();

            if (!props.Any())
            {
                return query;
            }

            var expr = composeKind == ComposeKind.And
                ? props.Aggregate((c, n) => c.And(n))
                : props.Aggregate((c, n) => c.Or(n));

            return query.Where(expr);
        }

        public static IOrderedQueryable<TSubject> Sort(IQueryable<TSubject> query, string propertyName,
            SortOrder order = SortOrder.Ascending)
        {
            if (!FastTypeInfo<TSubject>.PublicPropertiesMap.ContainsKey(propertyName))
            {
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                                                    $"in type \"{typeof(TSubject)}\"");
            }

            var property = FastTypeInfo<TSubject>.PublicPropertiesMap[propertyName];

            var parameter = Expression.Parameter(typeof(TSubject));
            var body = Expression.Property(parameter, propertyName);

            var lambda = FastTypeInfo<Expression>.PublicMethods.First(x => x.Name == "Lambda");

            lambda = lambda.MakeGenericMethod(typeof(Func<,>)
                .MakeGenericType(typeof(TSubject), property.PropertyType));

            var expression = lambda.Invoke(null, new object[] { body, new[] { parameter } });

            var orderBy = Conventions.OrderMethods[order].MakeGenericMethod(typeof(TSubject), property.PropertyType);

            return (IOrderedQueryable<TSubject>)orderBy.Invoke(query, new[] { query, expression });
        }

        private static IPredicateInfo CreatePredicateInfo<TPredicate>(TPredicate predicate, PropertyInfo propertyInfo,
            Dictionary<string, PropertyInfo> filterMap, PropertyInfo predicateProp)
        {
            if (TryGetComplexAttribute<TPredicate>(predicateProp.Name, out var attribute))
            {
                return new ComplexPredicateInfo
                {
                    Property = propertyInfo,
                    Value = filterMap[predicateProp.Name].GetValue(predicate) as Array,
                    Key = GetAttributeKeyOrDefault<TPredicate>(predicateProp.Name),
                    Attribute = attribute
                };
            }

            return new PredicateInfo
            {
                Property = propertyInfo,
                Value = filterMap[predicateProp.Name].GetValue(predicate),
                Key = GetAttributeKeyOrDefault<TPredicate>(predicateProp.Name)
            };
        }

        private static IEnumerable<IPredicateInfo> ExtractPredicateInfo<TPredicate>(TPredicate predicate,
            PropertyInfo propertyInfo, PropertyInfo[] propertiesOfColumn,
            Dictionary<string, PropertyInfo> filterMap) =>
            propertiesOfColumn
                .Select(predicateProp => CreatePredicateInfo(predicate, propertyInfo, filterMap, predicateProp))
                .ToArray();

        private static string GetAttributeKeyOrDefault<TPredicate>(string name)
        {
            var attribute = FastPropInfo<TPredicate>.Attributes[name]
                .FirstOrDefault(p => ConventionalFilters.AttributeKeys.ContainsKey(p.GetType()));

            if (attribute != null)
            {
                return ConventionalFilters.AttributeKeys[attribute.GetType()];
            }

            return string.Empty;
        }

        private static bool TryGetComplexAttribute<TPredicate>(string name, out MultiValueAttribute multiValueAttribute)
        {
            var attribute = FastPropInfo<TPredicate>.Attributes[name]
                .FirstOrDefault(p => p.GetType() == typeof(MultiValueAttribute));

            multiValueAttribute = null;

            if (attribute != null)
            {
                multiValueAttribute = (MultiValueAttribute)attribute;
                return true;
            }

            return false;
        }
    }
}
