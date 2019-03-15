using System;
using System.Collections.Generic;

namespace zSpec.Extensions
{
    ///<summary>
    /// String helpful extensions
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

        /// <summary>
        /// Join the enumerable with separator
        /// aka string.Join(separator, source)
        /// </summary>
        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Checks if string contains substring
        /// </summary>
        public static bool Contains(this string input, string value, StringComparison comparisonType)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.IndexOf(value, comparisonType) != -1;
            }

            return false;
        }

        /// <summary>
        /// Checks if string contains substring
        /// </summary>
        public static bool LikewiseContains(this string input, string value)
        {
            return Contains(input, value, StringComparison.CurrentCulture);
        }
    }
}