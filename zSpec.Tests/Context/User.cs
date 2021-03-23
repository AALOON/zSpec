using System;
using zSpec.Specs;

namespace zSpec.Tests.Context
{
    public class User
    {
        private const int MatureAge = 18;

        public User() => this.CreatedAt = DateTimeOffset.UtcNow;

        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public static Spec<User> IsMatureSpec => new(user => user.Age >= MatureAge);

        public static Spec<User> IsHaveEmailSpec => new(user => user.Email != null);

        public bool IsHasName(string name) => IsHasNameSpec(name).IsSatisfiedByNonCache(this);

        public bool IsHasNameEqualsEquals(string name) => IsHasNameEqualsSpec(name).IsSatisfiedBy(this);

        public static Spec<User> IsHasNameEqualsSpec(string name) =>
            new(user => user.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public static Spec<User> IsHasNameSpec(string name) => new(user => user.Name == name);

        public bool IsHaveEmail() => IsHaveEmailSpec.IsSatisfiedBy(this);

        public bool IsMature() => IsMatureSpec.IsSatisfiedBy(this);
    }
}
