using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using zSpec.Tests.Context;

namespace zSpec.Tests
{
    public class SpecTests : TestBase
    {
        public SpecTests(ITestOutputHelper output)
            : base(output)
        {
            Seed();
        }

        [Fact]
        public void Test1()
        {
            var list = DbContext.Users.Where(User.IsMatureSpec).ToList();
            list.Should().HaveCount(2);
            list.Should().OnlyContain(p => p.Age >= 18);
        }

        [Fact]
        public void Test2()
        {
            var list = DbContext.Users.Where(User.IsHaveEmailSpec).ToList();
            list.Should().HaveCount(2);
            list.Should().OnlyContain(p => p.Email != null);
        }

        [Fact]
        public void Test3()
        {
            var list = DbContext.Users.Where(User.IsMatureSpec && User.IsHaveEmailSpec).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Age >= 18 && p.Email != null);
        }

        [Fact]
        public void Test4()
        {
            var list = DbContext.Users.Where(User.IsMatureSpec || User.IsHaveEmailSpec).ToList();
            list.Should().HaveCount(3);
            list.Should().OnlyContain(p => p.Age >= 18 || p.Email != null);
        }

        /// <summary>
        /// TestBase [0] --------------------------1
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// 
        /// TestBase [0] --------------------------2
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// WHERE "user"."Name" = 'alpha'
        /// 
        /// TestBase [0] --------------------------3
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// 
        /// TestBase [0] --------------------------4
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// 
        /// TestBase [0] --------------------------5
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// 
        /// TestBase [0] --------------------------6
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// WHERE upper("user"."Name") = 'ALPHA'
        /// 
        /// TestBase [0] --------------------------7
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// WHERE @__ToUpper_0 = upper("user"."Name")
        /// </summary>
        [Fact]
        public void Test5()
        {
            Logger.LogInformation("--------------------------1");
            var list = DbContext.Users
                .Where(user => user.Name.Equals("alpha", StringComparison.OrdinalIgnoreCase))
                .ToList();


            Logger.LogInformation("--------------------------2");
            list = DbContext.Users
                .Where(user => user.Name == "alpha")
                .ToList();

            Logger.LogInformation("--------------------------3");
            list = DbContext.Users
                .Where(user => "alpha".Equals(user.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Logger.LogInformation("--------------------------4");
            list = DbContext.Users
                .Where(user => "ALPHA".Equals(user.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Logger.LogInformation("--------------------------5");
            list = DbContext.Users
                .Where(user => user.Name.ToUpper().Equals("alpha", StringComparison.OrdinalIgnoreCase))
                .ToList();

            Logger.LogInformation("--------------------------6");
            list = DbContext.Users
                .Where(user => user.Name.ToUpper() == "ALPHA")
                .ToList();

            Logger.LogInformation("--------------------------7");
            list = DbContext.Users
                .Where(user => "ALPHA".ToUpper().Equals(user.Name.ToUpper()))
                .ToList();
        }

        private void Seed()
        {
            var context = DbContext;
            context.Users.Add(new User { Age = 10, Name = "Alpha" });
            context.Users.Add(new User { Age = 18, Name = "Beta" });
            context.Users.Add(new User { Age = 18, Name = "Gamma", Email = "gamma@gmail.com" });
            context.Users.Add(new User { Age = 6, Name = "Delta", Email = "delta@gmail.com" });
            context.SaveChanges();
        }
    }
}
