using FluentAssertions;
using NUnit.Framework;
using zSpec.Automation;
using zSpec.Tests.Pagings;

namespace zSpec.Tests
{
    [TestFixture]
    public class FastTypeInfoTests
    {
        [Test]
        public void TestGenericCheckProperties()
        {
            FastTypeInfo<Paging>.PublicPropertiesMap.Should().ContainKey(nameof(Paging.OrderBy));
            FastTypeInfo<Paging>.PublicPropertiesMap.Should().ContainKey(nameof(Paging.Page));
            FastTypeInfo<Paging>.PublicPropertiesMap.Should().ContainKey(nameof(Paging.Take));
        }

        [Test]
        public void TestGenericPublicPropertiesMapSamePublicProperties()
        {
            FastTypeInfo<Paging>.PublicProperties.Count.Should().Be(3);
            FastTypeInfo<Paging>.PublicPropertiesMap.Count.Should().Be(3);

            FastTypeInfo<Paging>.PublicPropertiesMap.Values.Should()
                .BeEquivalentTo(FastTypeInfo<Paging>.PublicProperties);
        }

        [Test]
        public void TestGenericSimpleCreate()
        {
            var paging = FastTypeInfo<Paging>.Create();
            paging.Should().NotBeNull();
            paging.Take.Should().Be(2);
        }
    }
}
