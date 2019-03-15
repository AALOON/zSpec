using zSpec.Pagination;

namespace zSpec.Tests
{
    public class Paging : IPaging   
    {
        /// <inheritdoc />
        public int Page { get; set; }

        /// <inheritdoc />
        public int Take { get; set; } = 2;

        /// <inheritdoc />
        public string OrderBy { get; set; }
    }
}