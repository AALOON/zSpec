using Autofac;
using NUnit.Framework;
using Serilog;
using TestContext = zSpec.Tests.Context.TestContext;

namespace zSpec.Tests
{
    public abstract class TestBase
    {
        protected TestContext DbContext => TestFixture.Container.Resolve<TestContext>();

        protected TestBaseFixture TestFixture { get; set; }

        protected IContainer Container => TestFixture.Container;

        protected string LoggedData => Container.Resolve<StringBuilderLoggerProvider>().GetLogger().GetData();

        protected ILogger Logger => TestFixture.Container.Resolve<ILogger>();

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
            Container.Resolve<StringBuilderLoggerProvider>().GetLogger().Clear();
        }
    }
}