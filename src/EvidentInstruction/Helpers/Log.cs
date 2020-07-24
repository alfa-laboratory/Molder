using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Helpers
{
    /// <summary>
    /// Статичный класс для логирования.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Log
    {
        public static ILogger Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .MinimumLevel.Verbose()
             .WriteTo.Console(theme: AnsiConsoleTheme.Code)
             .CreateLogger();
    }
}