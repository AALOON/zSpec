using System;
using System.Linq;

namespace zSpec.Automation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}