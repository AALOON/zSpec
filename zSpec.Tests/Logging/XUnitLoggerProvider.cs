using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace zSpec.Tests.Logging
{
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly LogLevel _fromLogLevel;

        public XUnitLoggerProvider(ITestOutputHelper testOutputHelper, LogLevel fromLogLevel)
        {
            _testOutputHelper = testOutputHelper;
            _fromLogLevel = fromLogLevel;
        }

        public XUnitLoggerProvider(ITestOutputHelper testOutputHelper, Func<string, LogLevel, bool> filter)
        {
            _testOutputHelper = testOutputHelper;
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName)
            => _filter == null
                ? new XUnitLogger(_testOutputHelper, categoryName, _fromLogLevel)
                : new XUnitFuncLogger(_testOutputHelper, categoryName, _filter) as ILogger;

        public void Dispose()
        {
        }
    }
}