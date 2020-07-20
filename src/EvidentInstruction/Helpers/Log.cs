using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Helpers
{
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