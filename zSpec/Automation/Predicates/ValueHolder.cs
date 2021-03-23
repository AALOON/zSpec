namespace zSpec.Automation.Predicates
{
    /// <summary>
    /// Value holder for parametrization query
    /// </summary>
    internal class ValueHolder<TValue>
    {
        /// <summary>
        /// Value which will be parametrized
        /// </summary>
        public TValue Value { get; set; }
    }
}
