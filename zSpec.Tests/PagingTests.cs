using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using zSpec.Automation;
using zSpec.Pagination;
using zSpec.Tests.Context;
using zSpec.Tests.Pagings;

namespace zSpec.Tests
{
    [TestFixture]
    public sealed class PagingTests : TestBase
    {
        public static IEnumerable<IPaging> WithoutOrderBy => new[]
        {
            new Paging3 { Take = 2 }, new Paging3 { Take = 2, Page = 1 }, new Paging3 { Page = 1 }
        };

        [TestCaseSource(nameof(WithoutOrderBy))]
        public void ToQueryParams_WithoutOrderBy_NotContainsOrderKeys(IPaging paging)
        {
            var result = paging.ToQueryParams();
            result.Should().HaveCount(2);
            result.Should().ContainKey(nameof(paging.Page));
            result.Should().ContainKey(nameof(paging.Take));
            result[nameof(paging.Page)].Should().Be(paging.Page);
            result[nameof(paging.Take)].Should().Be(paging.Take);
        }

        public static IEnumerable<IPaging> WithOrderBy => new[]
        {
            new Paging3 { Take = 2, OrderBy = "string" }, new Paging3 { Take = 2, Page = 1, OrderBy = "string" },
            new Paging3 { Page = 1, OrderBy = "string" }, new Paging3 { OrderBy = "string" },
            new Paging3 { OrderBy = new OrderByColumn { Order = SortOrder.Ascending, Column = "string" } },
            new Paging3 { OrderBy = new OrderByColumn { Order = SortOrder.DescendingThenBy, Column = "string2" } }
        };

        [TestCaseSource(nameof(WithOrderBy))]
        public void ToQueryParams_WithOrderBy_MustContainsOrderKeys(IPaging paging)
        {
            var result = paging.ToQueryParams();
            const string column = nameof(paging.OrderBy) + "." + nameof(paging.OrderBy.Column);
            const string order = nameof(paging.OrderBy) + "." + nameof(paging.OrderBy.Order);
            result.Should().HaveCount(4);
            result.Should().ContainKey(column);
            result.Should().ContainKey(order);
            result[column].Should().Be(paging.OrderBy.Column);
            result[order].Should().Be(paging.OrderBy.Order);
        }

        [Test]
        public void TestPaginateByInterface()
        {
            IPaging paging = new Paging3 { Take = 2 };
            var list = this.DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInsertDefaultAge()
        {
            var paging = new Paging3 { Take = 2 };
            var list = this.DbContext.Users.OrderBy(p => p.Name).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInserThenBy()
        {
            var paging = new Paging4 { Take = 2, OrderBy = nameof(User.Age) };
            var list = this.DbContext.Users.OrderBy(p => p.Name).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInsertOrder()
        {
            var paging = new Paging { Take = 2, OrderBy = nameof(User.Name) };
            var list = this.DbContext.Users.OrderBy(p => p.Age).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInsertOrderFirstColumn()
        {
            var paging = new Paging { Take = 2 };
            var list = this.DbContext.Users.OrderBy(p => p.Age).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateOrderByDefaultAge()
        {
            var paging = new Paging3 { Take = 2 };
            var list = this.DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateOrderFirstColumn()
        {
            var paging = new Paging { Take = 2 };
            var list = this.DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateOrderFirstColumnWithSkip()
        {
            var paging = new Paging2 { Take = 2 };
            var list = this.DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateSimple()
        {
            var paging = new Paging { Take = 2, OrderBy = nameof(User.Name) };
            var list = this.DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateSkipOrder()
        {
            var paging = new Paging2 { Take = 2 };
            var list = this.DbContext.Users.OrderBy(p => p.Age).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateThenByNotUsesInsteadOrderBy()
        {
            var paging = new Paging4 { Take = 2, OrderBy = nameof(User.Age) };
            var list = this.DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void ToQueryParams_WithoutOrderBy_NotContainsOrderKeys()
        {
            var paging = new Paging3 { Take = 2, Page = 1 };
            var result = paging.ToQueryParams();
            result.Should().HaveCount(2);
            result.Should().ContainKey(nameof(paging.Page));
            result.Should().ContainKey(nameof(paging.Take));
            result[nameof(paging.Page)].Should().Be(paging.Page);
            result[nameof(paging.Take)].Should().Be(paging.Take);
        }
    }
}
