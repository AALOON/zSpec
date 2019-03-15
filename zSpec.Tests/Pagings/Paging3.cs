using zSpec.Automation.Attributes;
using zSpec.Pagination;
using zSpec.Tests.Context;

namespace zSpec.Tests.Pagings
{
    public class Paging3 : IPaging
    {
        /// <inheritdoc />
        public int Page { get; set; }

        /// <inheritdoc />
        public int Take { get; set; } = 2;

        /// <inheritdoc />
        [DefaultSortBy(nameof(User.Age))]
        public string OrderBy { get; set; }
    }
}