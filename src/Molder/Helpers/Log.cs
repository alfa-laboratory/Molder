using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Molder.Helpers
{
    /// <summary>
    /// Статичный класс для логирования.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Log
    {
        private static ILoggerFactory _Factory = null;

        private static void ConfigureLogger(ILoggerFactory factory)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

            factory.AddSerilog(logger);
        }

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory != null) return _Factory;
                _Factory = new LoggerFactory();
                ConfigureLogger(_Factory);
                return _Factory;
            }
            set => _Factory = value;
        }

        public static Microsoft.Extensions.Logging.ILogger Logger() => LoggerFactory.CreateLogger("Default");
    }
}