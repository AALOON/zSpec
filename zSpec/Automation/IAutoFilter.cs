using System.Linq;

namespace zSpec.Automation
{
    /// <summary>
    /// Auto filter interface
    /// </summary>
    /// <typeparam name="TEntity">Type of target entity</typeparam>
    public interface IAutoFilter<TEntity> where TEntity : class
    {
        /// <summary>
        /// Builds query from filter
        /// </summary>
        IQueryable<TEntity> Filter(IQueryable<TEntity> queryable);
    }
}