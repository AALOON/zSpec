using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// This attribute allows to mark property search by beginning of name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class StartWithFilterAttribute : Attribute
    {
        internal static readonly string Key = "StartWith";
    }
}