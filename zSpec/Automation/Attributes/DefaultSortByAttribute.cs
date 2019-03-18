using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Attribute allows to specify the default order column name
    /// </summary>
    public class DefaultSortByAttribute : Attribute
    {
        public DefaultSortByAttribute(string columnName, SortOrder order = SortOrder.Ascending)
        {
            ColumnName = columnName;
            SortOrder = order;
        }

        /// <summary>
        /// Column name
        /// </summary>
        public string ColumnName { get; }

        public SortOrder SortOrder { get; }
    }
}