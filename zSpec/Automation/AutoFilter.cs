using System.Linq;

namespace zSpec.Automation
{
    /// <summary>
    /// Auto filter allows to automatically map expressions of the filter model to entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TPredicate">Filter type.</typeparam>
    public class AutoFilter<TEntity, TPredicate> : IAutoFilter<TEntity>
        where TEntity : class
    {
        private readonly TPredicate predicate;

        public AutoFilter(TPredicate predicate) => this.predicate = predicate;

        /// <summary>
        /// Applies expressions
        /// </summary>
        public IQueryable<TEntity> Filter(IQueryable<TEntity> queryable) => this.DoFilter(queryable, this.predicate);

        private IQueryable<TEntity> DoFilter(IQueryable<TEntity> queryable, TPredicate predicate) =>
            queryable.AutoFilter(predicate);
    }
}
