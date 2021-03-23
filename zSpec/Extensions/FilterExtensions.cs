using zSpec.Automation;

namespace zSpec.Extensions
{
    /// <summary>
    /// Extensions for IAutoFilter.
    /// </summary>
    public static class FilterExtensions
    {
        public static IAutoFilter<TEntity> ToAutoFilter<TFilter, TEntity>(this TFilter filter)
            where TEntity : class =>
            new AutoFilter<TEntity, TFilter>(filter);
    }
}
