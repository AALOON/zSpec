﻿using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using zSpec.Automation;
using zSpec.Expressions;
using zSpec.Tests.Context;

namespace zSpec.Tests
{
    public class AutoFilterTests : TestBase
    {
        public AutoFilterTests(ITestOutputHelper output) : base(output)
        {
            Seed();
        }

        /// <summary>
        /// SELECT "u"."Id", "u"."Age", "u"."Email", "u"."Name"
        /// FROM "Users" AS "u"
        /// WHERE "u"."Name" LIKE 'Alpha' || '%' AND (substr("u"."Name", 1, length('Alpha')) = 'Alpha')
        /// </summary>
        [Fact]
        public void Test1()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter()
            {
                Name = "lph"
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Name == "Alpha");
        }

        [Fact]
        public void Test3()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter()
            {
                Email = "gamma@gmail.com"
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Email == "gamma@gmail.com");
        }

        [Fact]
        public void Test4()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter()
            {
                Email = "gamma@gmail.com"
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(1);
            list.Should().OnlyContain(p => p.Email == "gamma@gmail.com");
        }

        [Fact]
        public void Test2()
        {
            var filter = new AutoFilter<User, UserFilter>(new UserFilter()
            {
                Name = "Alpha",
                Age = 9
            });
            var list = DbContext.Users.Filter(filter).ToList();
            list.Should().HaveCount(0);
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