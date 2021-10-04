using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Molder.ReportPortal.Extensions
{
    public static class ConfigureLoggerExtension
    {
        public static void ConfigureLogger(this ILoggerFactory factory)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.LimitedMessagesToReportPortal()
                .CreateLogger();

            factory.AddSerilog(logger);
        }
    }
}