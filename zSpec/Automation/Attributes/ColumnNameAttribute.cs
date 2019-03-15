using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Attribute allows to setup name of column for filter
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the column
        /// </summary>
        public string Name { get; }
    }
}