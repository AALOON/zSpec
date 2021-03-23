using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Attribute allows to setup greater than or equal expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FromFilterAttribute : Attribute
    {
        internal static readonly string Key = "From";
    }
}
