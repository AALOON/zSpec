using zSpec.Automation;
using zSpec.Automation.Attributes;
using zSpec.Pagination;
using zSpec.Tests.Context;

namespace zSpec.Tests.Pagings
{
    public class Paging4 : IPaging
    {
        /// <inheritdoc />
        public int Page { get; set; }

        /// <inheritdoc />
        public int Take { get; set; } = 2;

        /// <inheritdoc />
        [Order(SortOrder.AscendingThenBy)]
        public string OrderBy { get; set; }
    }
}