using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using zSpec.Automation;
using zSpec.Expressions;
using zSpec.Extensions;
using zSpec.Tests.Context;

namespace zSpec.Tests
{
    [TestFixture]
    public class AutoFilterTests : TestBase
    {
        /// <summary>
        /// SELECT "u"."Id", "u"."Age", "u"."Email", "u"."Name"
        /// FROM "Users" AS "u"
        /// WHERE "u"."Name" LIKE 'Alpha' || '%' AND (substr("u"."Name", 1, length('Alpha')) = 'Alpha')
        /// </summary>
        [Test]
        public void Test1()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                Name = "lph"
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Name == "Alpha");
        }

        [Test]
        public void Test2()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                Name = "Alpha",
                Age = 9
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(0);
        }

        [Test]
        public void Test3()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                Email = "gamma@gmail.com"
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Email == "gamma@gmail.com");
        }

        [Test]
        public void Test4()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                Email = "gamma@gmail.com"
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Email == "gamma@gmail.com");
        }

        [Test]
        public void TestFromFilter()
        {
            var searchFrom = DateTimeOffset.UtcNow.AddHours(-5);
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                CreatedAt = searchFrom
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(2);

            list.Should().HaveCount(2);

            var data = LoggedData;
            data.Should().Contain("WHERE [u].[CreatedAt] >= @__value_0");
        }

        [Test]
        public void TestFromAndToFilter()
        {
            var searchFrom = DateTimeOffset.UtcNow.AddHours(-5);
            var searchTo = DateTimeOffset.UtcNow;
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                CreatedAt = searchFrom,
                To = searchTo
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(2);

            var data = LoggedData;
            data.Should().Contain("WHERE ([u].[CreatedAt] >= @__value_0) AND ([u].[CreatedAt] <= @__value_1)");
        }

        [Test]
        public void TestMultiFilter()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                MultiAge = new[] { 10, 6 }
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(2);

            var data = LoggedData;

            data.Should().Contain("WHERE [u].[Age] IN (@__value_0, @__value_1)");
        }

        [Test]
        public void TestToFilter()
        {
            var searchTo = DateTimeOffset.UtcNow.AddHours(-6);
            var filter = new UserFilter { To = searchTo }.ToAutoFilter<UserFilter, User>();
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(2);
        }
    }
}