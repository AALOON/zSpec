using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using zSpec.Pagination;
using zSpec.Tests.Context;
using zSpec.Tests.Pagings;

namespace zSpec.Tests
{
    [TestFixture]
    public class PagingTests : TestBase
    {
        [Test]
        public void TestPaginateByInterface()
        {
            IPaging paging = new Paging3 { Take = 2 };
            var list = DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInsertDefaultAge()
        {
            var paging = new Paging3 { Take = 2 };
            var list = DbContext.Users.OrderBy(p => p.Name).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInserThenBy()
        {
            var paging = new Paging4 { Take = 2, OrderBy = nameof(User.Age) };
            var list = DbContext.Users.OrderBy(p => p.Name).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInsertOrder()
        {
            var paging = new Paging { Take = 2, OrderBy = nameof(User.Name) };
            var list = DbContext.Users.OrderBy(p => p.Age).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateInsertOrderFirstColumn()
        {
            var paging = new Paging { Take = 2 };
            var list = DbContext.Users.OrderBy(p => p.Age).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateOrderByDefaultAge()
        {
            var paging = new Paging3 { Take = 2 };
            var list = DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateOrderFirstColumn()
        {
            var paging = new Paging { Take = 2 };
            var list = DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateOrderFirstColumnWithSkip()
        {
            var paging = new Paging2 { Take = 2 };
            var list = DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateSimple()
        {
            var paging = new Paging { Take = 2, OrderBy = nameof(User.Name) };
            var list = DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateSkipOrder()
        {
            var paging = new Paging2 { Take = 2 };
            var list = DbContext.Users.OrderBy(p => p.Age).Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }

        [Test]
        public void TestPaginateThenByNotUsesInsteadOrderBy()
        {
            var paging = new Paging4 { Take = 2, OrderBy = nameof(User.Age) };
            var list = DbContext.Users.Paginate(paging).ToList();
            list.Should().HaveCount(2);
        }
    }
}