using System;

namespace zSpec.Automation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FromFilterAttribute : Attribute
    {
        internal static readonly string Key = "From";
    }
}