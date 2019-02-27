using System.Linq;

namespace zSpec.Automation
{
    public interface IAutoFilter<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Filter(IQueryable<TEntity> queryable);
    }
}