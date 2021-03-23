using System;
using System.Text;
using Autofac.Util;
using Microsoft.Extensions.Logging;

namespace zSpec.Tests
{
    public sealed class StringBuilderLogger : ILogger
    {
        private readonly StringBuilder stringBuilder = new();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter) => this.stringBuilder.AppendLine(formatter(state, exception));

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => new Disposable();

        public void Clear() => this.stringBuilder.Clear();

        public string GetData() => this.stringBuilder.ToString();
    }
}
