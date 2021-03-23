using zSpec.Automation.Attributes;
using zSpec.Pagination;

namespace zSpec.Tests.Pagings
{
    public class Paging2 : IPaging
    {
        /// <inheritdoc />
        public int Page { get; set; }

        /// <inheritdoc />
        public int Take { get; set; } = 2;

        /// <inheritdoc />
        [SkipOrderIfEmpty]
        public OrderByColumn OrderBy { get; set; }
    }
}
