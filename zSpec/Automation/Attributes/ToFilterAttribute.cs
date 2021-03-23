using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Attribute allows to setup less than or equal expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ToFilterAttribute : Attribute
    {
        internal static readonly string Key = "To";
    }
}
