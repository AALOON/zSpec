using System;

namespace zSpec.Automation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StartWithFilterAttribute : Attribute
    {
        public static readonly string Key = "StartWith";
    }
}