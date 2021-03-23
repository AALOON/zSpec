using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// This attribute allows to mark property search by part of name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainsFilterAttribute : Attribute
    {
        internal static readonly string Key = "Contains";
    }
}
