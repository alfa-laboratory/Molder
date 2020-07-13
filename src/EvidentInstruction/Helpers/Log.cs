using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace EvidentInstruction.Helpers
{
    public static class Log
    {
        public static ILogger Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .MinimumLevel.Verbose()
             .WriteTo.Console(theme: AnsiConsoleTheme.Code)
             .CreateLogger();
    }
}