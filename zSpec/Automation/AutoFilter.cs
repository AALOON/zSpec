using System.Linq;

namespace zSpec.Automation
{
    public class AutoFilter<TEntity, TPredicate> : IAutoFilter<TEntity>
        where TEntity : class
    {
        private readonly TPredicate _predicate;

        public AutoFilter(TPredicate predicate)
        {
            _predicate = predicate;
        }

        private IQueryable<TEntity> DoFilter(IQueryable<TEntity> queryable, TPredicate predicate)
            => queryable.AutoFilter(predicate);

        public IQueryable<TEntity> Filter(IQueryable<TEntity> queryable)
            => DoFilter(queryable, _predicate);
    }
}