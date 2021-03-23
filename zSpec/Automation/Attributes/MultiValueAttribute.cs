using System;

namespace zSpec.Automation.Attributes
{
    /// <summary>
    /// Specifies that array will be used as multi Value selection.
    /// </summary>
    public class MultiValueAttribute : Attribute
    {
        /// <param name="composeKind">Specifies that multi Value expression compose kind.</param>
        public MultiValueAttribute(ComposeKind composeKind = ComposeKind.Or) => this.ComposeKind = composeKind;

        /// <summary>
        /// Specifies that multi Value expression compose kind.
        /// </summary>
        public ComposeKind ComposeKind { get; }
    }
}
