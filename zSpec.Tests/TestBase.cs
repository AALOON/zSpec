#define NotInMemory // NotInMemory/InMemory

using System;
using System.Diagnostics;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using zSpec.Tests.Context;
using zSpec.Tests.Logging;

namespace zSpec.Tests
{
    public abstract class TestBase : IDisposable
    {
        private const string SettingPath = "appsettings.json";

        protected TestBase(ITestOutputHelper output)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(SettingPath, optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddLogging();

            AddMemoryContext(services);
            AddSqlContext(services);

            var containerBuilder = new ContainerBuilder();

            RegisterLogger(containerBuilder);

            containerBuilder.Populate(services);

            ContextLoggerFactory = new LoggerFactory();
            ContextLoggerFactory.AddProvider(new XUnitLoggerProvider(output,
                (category, level) => category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information));

            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new XUnitLoggerProvider(output, LogLevel.Information));

            Container = containerBuilder.Build();

            var context = Container.Resolve<TestContext>();
            context.Database.EnsureCreated();
        }

        protected TestContext DbContext => Container.Resolve<TestContext>();

        protected LoggerFactory ContextLoggerFactory { get; }

        protected LoggerFactory LoggerFactory { get; }

        protected IConfigurationRoot Configuration { get; }

        protected IContainer Container { get; }

        protected ILogger Logger => LoggerFactory.CreateLogger(nameof(TestBase));

        protected void RegisterLogger(ContainerBuilder builder)
        {
            builder.Register((_, __) => Logger);
        }

        [Conditional("InMemory")]
        private void AddMemoryContext(ServiceCollection services)
        {
            services.AddDbContext<TestContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseLoggerFactory(ContextLoggerFactory);
            }, ServiceLifetime.Transient);
        }

        [Conditional("NotInMemory")]
        private void AddSqlContext(ServiceCollection services)
        {
            var connectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection");
            services.AddDbContext<TestContext>(options =>
            {
                options.UseSqlite(connectionString.Value);
                options.UseLoggerFactory(ContextLoggerFactory);
            }, ServiceLifetime.Transient);
        }

        public void Dispose()
        {
            var context = Container.Resolve<TestContext>();
            context.Database.EnsureDeleted();
            ContextLoggerFactory?.Dispose();
            Container?.Dispose();
        }
    }
}