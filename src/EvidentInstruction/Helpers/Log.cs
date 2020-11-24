using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Extensions.Logging;
using System;
using Serilog.Events;

namespace EvidentInstruction.Helpers
{
    /// <summary>
    /// Статичный класс для логирования.
    /// </summary>
    //[ExcludeFromCodeCoverage]
    //public static class Log
    //{
    //    public static ILogger Logger = new LoggerConfiguration()
    //         .Enrich.FromLogContext()
    //         .MinimumLevel.Verbose()
    //         .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    //         .CreateLogger();
    //}
    //

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
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                    ConfigureLogger(_Factory);
                }
                return _Factory;
            }
            set { _Factory = value; }
        }

        public static Microsoft.Extensions.Logging.ILogger Logger() => LoggerFactory.CreateLogger("Default");
    }
}