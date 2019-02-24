using System;
using System.Collections.Generic;
using System.Linq;

namespace zSpec.Extensions
{
    ///<summary>
    /// String class extensions
    ///</summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Indicates whether the specified string not null or an empty string.
        /// </summary>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static bool Contains(this string input, string value, StringComparison comparisonType)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.IndexOf(value, comparisonType) != -1;
            }

            return false;
        }

        public static bool LikewiseContains(this string input, string value)
        {
            return Contains(input, value, StringComparison.CurrentCulture);
        }
    }
}