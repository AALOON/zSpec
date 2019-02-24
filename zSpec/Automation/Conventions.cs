﻿using System;
using System.Linq;
using System.Linq.Expressions;
using zSpec.Expressions;

namespace zSpec.Automation
{
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

            return (IOrderedQueryable<TSubject>)orderBy.Invoke(query, new [] { query, expression });
        }

        public static IQueryable<TSubject> Filter<TPredicate>(IQueryable<TSubject> query,
            TPredicate predicate,
            ComposeKind composeKind = ComposeKind.And)
        {
            var filterProps = FastTypeInfo<TPredicate>
                .PublicProperties
                .ToArray();

            var filterPropNames = filterProps
                .Select(x => x.Name)
                .ToArray();

            var modelType = typeof(TSubject);

            var parameter = Expression.Parameter(modelType);

            var props = FastTypeInfo<TSubject>
                .PublicProperties
                .Where(x => filterPropNames.Contains(x.Name))
                .Select(x => new
                {
                    Property = x,
                    Value = filterProps.Single(y => y.Name == x.Name).GetValue(predicate)
                })
                .Where(x => x.Value != null)
                .Select(x =>
                {
                    var property = Expression.Property(parameter, x.Property);
                    Expression value = Expression.Constant(x.Value);

                    value = Expression.Convert(value, property.Type);
                    var body = Conventions.Filters[property.Type](property, value);

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
    }

    public static class Conventions
    {
        public static ConventionalFilters Filters { get; } = new ConventionalFilters();
    }
}