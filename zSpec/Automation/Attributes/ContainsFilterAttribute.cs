using System;

namespace zSpec.Automation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainsFilterAttribute : Attribute
    {
        public static readonly string Key = "Contains";
    }
}