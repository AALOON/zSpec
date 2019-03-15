using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Attribute allows to specify the default order column name
    /// </summary>
    public class DefaultSortByAttribute : Attribute
    {
        public DefaultSortByAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// Column name
        /// </summary>
        public string ColumnName { get; }
    }
}