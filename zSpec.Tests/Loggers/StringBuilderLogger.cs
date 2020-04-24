using System;
using System.Text;
using Autofac.Util;
using Microsoft.Extensions.Logging;

namespace zSpec.Tests
{
    public sealed class StringBuilderLogger : ILogger
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _stringBuilder.AppendLine(formatter(state, exception));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new Disposable();
        }

        public string GetData()
        {
            return _stringBuilder.ToString();
        }

        public void Clear()
        {
            _stringBuilder.Clear();
        }
    }
}