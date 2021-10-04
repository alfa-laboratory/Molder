using System;
using Molder.ReportPortal.Models;
using Serilog;
using Serilog.Configuration;

namespace Molder.ReportPortal.Extensions
{
    public static class LimitedMessagesToReportPortalSinkExtension
    {
        public static LoggerConfiguration LimitedMessagesToReportPortal(this LoggerSinkConfiguration loggerConfiguration, IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new LimitedMessagesToReportPortalSink(formatProvider));
        }
    }
}