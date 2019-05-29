using System;
using zSpec.Specs;

namespace zSpec.Tests.Context
{
    public class User
    {
        private const int MatureAge = 18;

        public User()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public static Spec<User> IsMatureSpec => new Spec<User>(user => user.Age >= MatureAge);

        public static Spec<User> IsHaveEmailSpec => new Spec<User>(user => user.Email != null);

        public bool IsMature()
        {
            return IsMatureSpec.IsSatisfiedBy(this);
        }

        public bool IsHaveEmail()
        {
            return IsHaveEmailSpec.IsSatisfiedBy(this);
        }

        public static Spec<User> IsHasNameEqualsSpec(string name)
        {
            return new Spec<User>(user => user.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsHasNameEqualsEquals(string name)
        {
            return IsHasNameEqualsSpec(name).IsSatisfiedBy(this);
        }

        public static Spec<User> IsHasNameSpec(string name)
        {
            return new Spec<User>(user => user.Name == name);
        }

        public bool IsHasName(string name)
        {
            return IsHasNameSpec(name).IsSatisfiedByNonCache(this);
        }
    }
}