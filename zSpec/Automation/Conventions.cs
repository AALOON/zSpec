using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using zSpec.Expressions;

namespace zSpec.Automation
{
    public static class Conventions
    {
        public static ConventionalFilters Filters { get; } = new ConventionalFilters();
    }

    public static class Conventions<TSubject>
    {
        public static IOrderedQueryable<TSubject> Sort(IQueryable<TSubject> query, string propertyName)
        {
            (string, bool) GetSorting()
            {
                var arr = propertyName.Split('.');
                if (arr.Length == 1)
                    return (arr[0], false);
                var sort = arr[1];
                if (string.Equals(sort, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], false);
                if (string.Equals(sort, "DESC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], true);
                return (arr[0], false);
            }

            var (name, isDesc) = GetSorting();
            propertyName = name;

            var property = FastTypeInfo<TSubject>
                .PublicProperties
                .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));

            if (property == null)
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                    $"in type \"{typeof(TSubject)}\"");

            var parameter = Expression.Parameter(typeof(TSubject));
            var body = Expression.Property(parameter, propertyName);

            var lambda = FastTypeInfo<Expression>
                .PublicMethods
                .First(x => x.Name == "Lambda");

            lambda = lambda.MakeGenericMethod(typeof(Func<,>)
                .MakeGenericType(typeof(TSubject), property.PropertyType));

            var expression = lambda.Invoke(null, new object[] { body, new[] { parameter } });

            var methodName = isDesc ? "OrderByDescending" : "OrderBy";

            var orderBy = typeof(Queryable)
                .GetMethods()
                .First(x => x.Name == methodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TSubject), property.PropertyType);

            return (IOrderedQueryable<TSubject>)orderBy.Invoke(query, new[] { query, expression });
        }

        public static IQueryable<TSubject> Filter<TPredicate>(IQueryable<TSubject> query,
            TPredicate predicate,
            ComposeKind composeKind = ComposeKind.And)
        {
            var filterMap = FastTypeInfo<TPredicate>.PublicPropertiesMap;

            var modelType = typeof(TSubject);

            var parameter = Expression.Parameter(modelType);

            var props = FastTypeInfo<TSubject>
                .PublicProperties
                .Where(info => filterMap.ContainsKey(info.Name))
                .Select(info => new
                {
                    Property = info,
                    Value = filterMap[info.Name].GetValue(predicate),
                    Key = GetAttributeKeyOrDefault(filterMap[info.Name])
                })
                .Where(x => x.Value != null)
                .Select(x =>
                {
                    var property = Expression.Property(parameter, x.Property);
                    Expression value = Expression.Constant(x.Value);

                    value = Expression.Convert(value, property.Type);
                    var body = Conventions.Filters[new TypeKey(property.Type, x.Key)](property, value);

                    return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
                })
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

        private static string GetAttributeKeyOrDefault(PropertyInfo info)
        {
            
            var attribute = info
                .GetCustomAttributes(true)
                .FirstOrDefault(p => ConventionalFilters.AttributeKeys.ContainsKey(p.GetType()));
            if (attribute != null)
                return ConventionalFilters.AttributeKeys[attribute.GetType()];
            return string.Empty;
        }
    }
}