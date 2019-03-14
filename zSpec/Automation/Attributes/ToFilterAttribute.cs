using System;

namespace zSpec.Automation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ToFilterAttribute : Attribute
    {
        internal static readonly string Key = "To";
    }
}