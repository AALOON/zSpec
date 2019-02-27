using System;
using zSpec.Specs;

namespace zSpec.Tests.Context
{
    public class User
    {
        private const int MatureAge = 18;

        public long Id { get; set; }
        
        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public static Spec<User> IsMatureSpec => new Spec<User>(user => user.Age >= MatureAge);

        public bool IsMature() => IsMatureSpec.IsSatisfiedBy(this);
        
        public static Spec<User> IsHaveEmailSpec => new Spec<User>(user => user.Email != null);

        public bool IsHaveEmail() => IsHaveEmailSpec.IsSatisfiedBy(this);

        public static Spec<User> IsHasNameEqualsSpec(string name)
            => new Spec<User>(user => user.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public bool IsHasNameEqualsEquals(string name) => IsHasNameEqualsSpec(name).IsSatisfiedBy(this);

        public static Spec<User> IsHasNameSpec(string name)
            => new Spec<User>(user => user.Name == name);

        public bool IsHasName(string name) => IsHasNameSpec(name).IsSatisfiedByNonCache(this);
    }
}