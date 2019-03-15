using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Attribute allows to specify the order
    /// </summary>
    public class OrderAttribute : Attribute
    {
        public OrderAttribute(SortOrder sortOrder)
        {
            SortOrder = sortOrder;
        }

        /// <summary>
        /// Order of sorting
        /// </summary>
        public SortOrder SortOrder { get; }
    }
}