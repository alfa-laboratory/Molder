using Microsoft.Extensions.Logging;
using ReportPortal.Serilog;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace Molder.ReportPortal
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public static class ReportPortalLog
    {
        [BeforeTestRun(Order = -100000000)]
        public static void BeforeForReportPortal()
        {
            Helpers.Log.LoggerFactory.ConfigureLogger();
        }

        private static void ConfigureLogger(this ILoggerFactory factory)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.ReportPortal()
            .CreateLogger();

            factory.AddSerilog(logger);
        }
    }
}
