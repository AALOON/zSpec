using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace zSpec.Tests.Logging
{
    public class XUnitFuncLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _filter;

        public XUnitFuncLogger(ITestOutputHelper testOutputHelper,
            string categoryName, Func<string, LogLevel, bool> filter)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
            _filter = filter;
        }

        public IDisposable BeginScope<TState>(TState state)
            => DumpDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => _filter(_categoryName, logLevel);

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