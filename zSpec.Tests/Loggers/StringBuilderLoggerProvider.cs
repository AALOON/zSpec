using System;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;

namespace zSpec.Tests
{
    public sealed class StringBuilderLoggerProvider : ILoggerProvider, IDisposable, ILogEventEnricher
    {
        private readonly StringBuilderLogger logger = new();

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName) => this.logger;

        public StringBuilderLogger GetLogger() => this.logger;
    }
}
