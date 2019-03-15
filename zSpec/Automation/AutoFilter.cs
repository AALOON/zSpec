using System.Linq;

namespace zSpec.Automation
{
    /// <summary>
    /// Auto filter allows to automaticaly map expressions of the filter model to entity
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPredicate">Filter type</typeparam>
    public class AutoFilter<TEntity, TPredicate> : IAutoFilter<TEntity>
        where TEntity : class
    {
        private readonly TPredicate _predicate;

        public AutoFilter(TPredicate predicate)
        {
            _predicate = predicate;
        }

        private IQueryable<TEntity> DoFilter(IQueryable<TEntity> queryable, TPredicate predicate)
        {
            return queryable.AutoFilter(predicate);
        }

        /// <summary>
        /// Applies expressions 
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Filter(IQueryable<TEntity> queryable)
        {
            return DoFilter(queryable, _predicate);
        }
    }
}