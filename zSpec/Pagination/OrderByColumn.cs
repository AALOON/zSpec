using zSpec.Automation;

namespace zSpec.Pagination
{
    /// <summary>
    /// Sort column model.
    /// </summary>
    public class OrderByColumn
    {
        /// <summary>
        /// Column name.
        /// </summary>
        public string Column { get; set; }

        public SortOrder? Order { get; set; }

        public static implicit operator OrderByColumn(string str) => new() { Column = str };

        /// <inheritdoc />
        public override string ToString() => $"{this.Column} {this.Order}";
    }
}
