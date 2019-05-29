using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using zSpec.Tests.Context;

namespace zSpec.Tests
{
    [TestFixture]
    public class SpecTests : TestBase
    {
        [Test]
        public void Test1()
        {
            var list = DbContext.Users.Where(User.IsMatureSpec).ToList();
            list.Should().HaveCount(2);
            list.Should().OnlyContain(p => p.Age >= 18);
        }

        [Test]
        public void Test2()
        {
            var list = DbContext.Users.Where(User.IsHaveEmailSpec).ToList();
            list.Should().HaveCount(2);
            list.Should().OnlyContain(p => p.Email != null);
        }

        [Test]
        public void Test3()
        {
            var list = DbContext.Users.Where(User.IsMatureSpec && User.IsHaveEmailSpec).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Age >= 18 && p.Email != null);
        }

        [Test]
        public void Test4()
        {
            var list = DbContext.Users.Where(User.IsMatureSpec || User.IsHaveEmailSpec).ToList();
            list.Should().HaveCount(2);
            list.Should().OnlyContain(p => p.Age >= 18 || p.Email != null);
        }

        /// <summary>
        /// TestBase [0] --------------------------1
        /// Client Evaluation
        /// TestBase [0] --------------------------2
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// WHERE "user"."Name" = 'alpha'
        /// TestBase [0] --------------------------3
        /// Client Evaluation
        /// TestBase [0] --------------------------4
        /// Client Evaluation
        /// TestBase [0] --------------------------5
        /// Client Evaluation
        /// TestBase [0] --------------------------6
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// WHERE upper("user"."Name") = 'ALPHA'
        /// TestBase [0] --------------------------7
        /// SELECT "user"."Id", "user"."Age", "user"."Email", "user"."Name"
        /// FROM "Users" AS "user"
        /// WHERE @__ToUpper_0 = upper("user"."Name")
        /// </summary>
        [Test]
        public void Test5()
        {
            Logger.Information("--------------------------1");
            DbContext.Users.Invoking(users =>
            {
                _ = users
                    .Where(user => user.Name.Equals("alpha", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }).Should().Throw<InvalidOperationException>();


            Logger.Information("--------------------------2");
            _ = DbContext.Users
                .Where(user => user.Name == "alpha")
                .ToList();

            Logger.Information("--------------------------3");
            DbContext.Users.Invoking(users =>
            {
                _ = users
                    .Where(user => "alpha".Equals(user.Name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }).Should().Throw<InvalidOperationException>();

            Logger.Information("--------------------------4");
            DbContext.Users.Invoking(users =>
            {
                _ = users
                    .Where(user => "ALPHA".Equals(user.Name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }).Should().Throw<InvalidOperationException>();


            Logger.Information("--------------------------5");
            DbContext.Users.Invoking(users =>
            {
                _ = users
                    .Where(user => user.Name.ToUpper().Equals("alpha", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }).Should().Throw<InvalidOperationException>();


            Logger.Information("--------------------------6");
            _ = DbContext.Users
                .Where(user => user.Name.ToUpper() == "ALPHA")
                .ToList();

            Logger.Information("--------------------------7");
            _ = DbContext.Users
                .Where(user => "ALPHA".ToUpper().Equals(user.Name.ToUpper()))
                .ToList();
        }

        [Test]
        public void Test6()
        {
            var user = new User { Age = 10, Name = "Alpha" };
            user.IsHasName("Alpha").Should().BeTrue();
            user.IsHasName("Alpha1").Should().BeFalse();
            user.IsHasName("Alpha2").Should().BeFalse();
        }

        [Test]
        public void Test7()
        {
            var list = DbContext.Users.Where(User.IsHasNameSpec("Alpha")).ToList();
            list.Should().HaveCount(1);
        }
    }
}