using Autofac;
using NUnit.Framework;
using Serilog;
using TestContext = zSpec.Tests.Context.TestContext;

namespace zSpec.Tests
{
    public abstract class TestBase
    {
        protected TestContext DbContext => this.TestFixture.Container.Resolve<TestContext>();

        protected TestBaseFixture TestFixture { get; set; }

        protected IContainer Container => this.TestFixture.Container;

        protected string LoggedData => this.Container.Resolve<StringBuilderLoggerProvider>().GetLogger().GetData();

        protected ILogger Logger => this.TestFixture.Container.Resolve<ILogger>();

        [OneTimeSetUp]
        public void TestOneTimeSetup() => this.FixtureInitialize();

        [OneTimeTearDown]
        public void TestOneTimeTearDown() => this.FixtureCleanUp();

        [SetUp]
        public void TestSetUp() => this.SetUp();

        [TearDown]
        public void TestTearDown() => this.CleanUp();

        protected virtual void CleanUp() => this.Container.Resolve<StringBuilderLoggerProvider>().GetLogger().Clear();

        protected virtual void FixtureCleanUp() => this.TestFixture.Dispose();

        protected virtual void FixtureInitialize() => this.TestFixture = new TestBaseFixture();

        protected virtual void SetUp()
        {
        }
    }
}
