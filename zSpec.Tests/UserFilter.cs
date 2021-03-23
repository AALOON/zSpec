using System;
using zSpec.Automation.Attributes;

namespace zSpec.Tests
{
    public class UserFilter
    {
        [ContainsFilter] public string Name { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        [MultiValue] [ColumnName(nameof(Age))] public int[] MultiAge { get; set; }

        [FromFilter] public DateTimeOffset? CreatedAt { get; set; }

        [ToFilter]
        [ColumnName(nameof(CreatedAt))]
        public DateTimeOffset? To { get; set; }
    }
}
