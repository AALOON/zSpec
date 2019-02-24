using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace zSpec.Tests.Logging
{
    public class XUnitLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _categoryName;
        private readonly LogLevel _fromLogLevel;

        public XUnitLogger(ITestOutputHelper testOutputHelper, string categoryName, LogLevel fromLogLevel)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
            _fromLogLevel = fromLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
            => DumpDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => logLevel >= _fromLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception)}");
            if (exception != null)
                _testOutputHelper.WriteLine(exception.ToString());
        }

        private class DumpDisposable : IDisposable
        {
            public static readonly DumpDisposable Instance = new DumpDisposable();
            public void Dispose()
            { }
        }
    }
}