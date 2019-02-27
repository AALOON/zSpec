using System.Linq;

namespace zSpec.Automation
{
    public static class ConventionsExtensions
    {
        public static IQueryable<TSubject> AutoFilter<TSubject, TPredicate>(
            this IQueryable<TSubject> query, TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
            => Conventions<TSubject>.Filter(query, predicate, composeKind);
    }
}