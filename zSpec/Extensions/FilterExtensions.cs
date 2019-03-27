using zSpec.Automation;

namespace zSpec.Extensions
{
    public static class FilterExtensions
    {
        public static IAutoFilter<TEntity> ToAutoFilter<TFilter, TEntity>(this TFilter filter)
            where TEntity : class
        {
            return new AutoFilter<TEntity, TFilter>(filter);
        }
    }
}