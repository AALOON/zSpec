using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using zSpec.Automation;
using zSpec.Expressions;
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
            list.Should().HaveCount(expected: 1);
            list.Should().OnlyContain(p => p.Name == "Alpha");
        }

        [Test]
        public void Test3()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                Email = "gamma@gmail.com"
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(expected: 1);
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
            list.Should().HaveCount(expected: 1);
            list.Should().OnlyContain(p => p.Email == "gamma@gmail.com");
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
            list.Should().HaveCount(expected: 0);
        }

        [Test]
        public void TestFromFilter()
        {
            var searchFrom = DateTimeOffset.UtcNow.AddHours(hours: -5);
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                CreatedAt = searchFrom
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(expected: 2);
        }

        [Test]
        public void TestToFilter()
        {
            var searchTo = DateTimeOffset.UtcNow.AddHours(hours: -6);
            var filter = new AutoFilter<User, UserFilter>(new UserFilter
            {
                To = searchTo
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(expected: 2);
        }
    }
}