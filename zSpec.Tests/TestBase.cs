using Autofac;
using NUnit.Framework;
using Serilog;

namespace zSpec.Tests
{
    public abstract class TestBase
    {
        [OneTimeSetUp]
        public void TestOneTimeSetup()
        {
            FixtureInitialize();
        }

        [OneTimeTearDown]
        public void TestOneTimeTearDown()
        {
            FixtureCleanUp();
        }

        [SetUp]
        public void TestSetUp()
        {
            SetUp();
        }

        [TearDown]
        public void TestTearDown()
        {
            CleanUp();
        }

        protected virtual void FixtureInitialize()
        {
            TestFixture = new TestBaseFixture();
        }

        protected virtual void FixtureCleanUp()
        {
            TestFixture.Dispose();
        }

        protected virtual void SetUp()
        {
        }

        protected virtual void CleanUp()
        {
        }

        protected Context.TestContext DbContext => TestFixture.Container.Resolve<Context.TestContext>();

        protected TestBaseFixture TestFixture { get; private set; }

        protected ILogger Logger => TestFixture.Container.Resolve<ILogger>();
    }
}