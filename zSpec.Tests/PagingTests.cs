using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using zSpec.Pagination;
using zSpec.Tests.Context;

namespace zSpec.Tests
{
    public class PagingTests : TestBase
    {
        public PagingTests(ITestOutputHelper output)
            : base(output)
        {
            Seed();
        }


        [Fact]
        public void Test1()
        {
            var paging = new Paging { Take = 2 , OrderBy = nameof(User.Name)};
            var list = DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        private void Seed()
        {
            var sw = new Stopwatch();
            sw.Start();
            var context = DbContext;
            context.Users.Add(new User { Age = 10, Name = "Alpha", CreatedAt = DateTimeOffset.UtcNow.AddDays(-1) });
            context.Users.Add(new User { Age = 18, Name = "Beta", CreatedAt = DateTimeOffset.UtcNow.AddDays(-1) });
            context.Users.Add(new User { Age = 18, Name = "Gamma", Email = "gamma@gmail.com" });
            context.Users.Add(new User { Age = 6, Name = "Delta", Email = "delta@gmail.com" });
            context.SaveChanges();
            sw.Stop();
            Logger.LogInformation($"Seed Elapsed:{sw.Elapsed.ToString()}");
        }
    }
}