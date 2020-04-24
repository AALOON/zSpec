using System;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;

namespace zSpec.Tests
{
    public sealed class StringBuilderLoggerProvider : ILoggerProvider, IDisposable, ILogEventEnricher
    {
        private readonly StringBuilderLogger _logger = new StringBuilderLogger();

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
        }

        public StringBuilderLogger GetLogger()
        {
            return _logger;
        }
    }
}