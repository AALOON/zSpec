using zSpec.Automation.Attributes;

namespace zSpec.Pagination
{
    /// <summary>
    /// Paging interface allows to implement your paging model
    /// </summary>
    public interface IPaging
    {
        /// <summary>
        /// Page of paginated list from zero
        /// All values less than zero will automatically use zero
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Amount of items to retrieve
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Allows to specify order column name
        /// Also this value supports attributes
        /// <see cref="DefaultSortByAttribute"/>,
        /// <see cref="OrderAttribute"/>,
        /// <see cref="SkipOrderIfEmptyAttribute"/>
        /// </summary>
        OrderByColumn OrderBy { get; }
    }
}