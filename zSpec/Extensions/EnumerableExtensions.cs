using System.Collections.Generic;

namespace zSpec.Extensions
{
    public static class EnumerableExtensions
    {
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source) => new(source);
    }
}
