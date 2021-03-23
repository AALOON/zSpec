using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Attribute allows to specify the default order column name.
    /// </summary>
    public class DefaultSortByAttribute : Attribute
    {
        public DefaultSortByAttribute(string columnName, SortOrder order = SortOrder.Ascending)
        {
            this.ColumnName = columnName;
            this.SortOrder = order;
        }

        /// <summary>
        /// Column name.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Sort order method.
        /// </summary>
        public SortOrder SortOrder { get; }
    }
}
