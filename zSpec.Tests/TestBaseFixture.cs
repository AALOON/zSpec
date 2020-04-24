#define NotInMemory // NotInMemory/InMemory

using System;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using zSpec.Tests.Context;
using ILogger = Serilog.ILogger;

namespace zSpec.Tests
{
    public class TestBaseFixture : IDisposable
    {
        private const string SettingPath = "appsettings.json";

        public TestBaseFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(SettingPath, false, true);
            Configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddLogging();

            AddMemoryContext(services);
            AddSqlContext(services);

            var containerBuilder = new ContainerBuilder();

            RegisterLogger(containerBuilder);

            containerBuilder.Populate(services);

            ContextLoggerFactory = new LoggerFactory();
            ContextLoggerFactory.AddProvider(new SerilogLoggerProvider());
            var sbProvider = new StringBuilderLoggerProvider();
            ContextLoggerFactory.AddProvider(sbProvider);
            containerBuilder.RegisterInstance(sbProvider);

            Container = containerBuilder.Build();

            var context = Container.Resolve<TestContext>();
            context.Database.EnsureCreated();
            Seed();
        }

        internal TestContext DbContext => Container.Resolve<TestContext>();

        internal LoggerFactory ContextLoggerFactory { get; }

        internal LoggerFactory LoggerFactory { get; }

        internal IConfigurationRoot Configuration { get; }

        internal IContainer Container { get; }

        internal ILogger Logger => Container.Resolve<ILogger>();

        /// <inheritdoc />
        public void Dispose()
        {
            var context = Container.Resolve<TestContext>();
            //context.Database.EnsureDeleted();
            context.Users.RemoveRange(context.Users.AsNoTracking().ToArray());
            context.SaveChanges();
            ContextLoggerFactory?.Dispose();
            Container?.Dispose();
        }

        private void RegisterLogger(ContainerBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            builder.RegisterInstance(Log.Logger);
        }

        [Conditional("InMemory")]
        private void AddMemoryContext(ServiceCollection services)
        {
            services.AddDbContext<TestContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseLoggerFactory(ContextLoggerFactory);
                options.ConfigureWarnings(builder => builder.Throw(RelationalEventId.QueryClientEvaluationWarning));
            }, ServiceLifetime.Transient);
        }

        [Conditional("NotInMemory")]
        private void AddSqlContext(ServiceCollection services)
        {
            var connectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            services.AddDbContext<TestContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.UseLoggerFactory(ContextLoggerFactory);
                options.ConfigureWarnings(builder => builder.Throw(RelationalEventId.QueryClientEvaluationWarning));
            }, ServiceLifetime.Transient);
        }

        private void Seed()
        {
            var context = DbContext;
            context.Users.Add(new User { Age = 10, Name = "Alpha", CreatedAt = DateTimeOffset.UtcNow.AddDays(-1) });
            context.Users.Add(new User { Age = 18, Name = "Beta", CreatedAt = DateTimeOffset.UtcNow.AddDays(-1) });
            context.Users.Add(new User { Age = 18, Name = "Gamma", Email = "gamma@gmail.com" });
            context.Users.Add(new User { Age = 6, Name = "Delta", Email = "delta@gmail.com" });
            context.SaveChanges();
        }
    }
}