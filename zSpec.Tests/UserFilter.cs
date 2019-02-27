using zSpec.Automation.Attributes;

namespace zSpec.Tests
{
    public class UserFilter
    {
        [ContainsFilter]
        public string Name { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }
    }
}