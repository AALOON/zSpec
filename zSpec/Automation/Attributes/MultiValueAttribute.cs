using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Specifies that array will be used as multi value selection
    /// </summary>
    public class MultiValueAttribute : Attribute
    {
        /// <param name="composeKind">Specifies that multi value expression compose kind</param>
        public MultiValueAttribute(ComposeKind composeKind = ComposeKind.Or)
        {
            ComposeKind = composeKind;
        }

        /// <summary>
        /// Specifies that multi value expression compose kind
        /// </summary>
        public ComposeKind ComposeKind { get; }
    }
}