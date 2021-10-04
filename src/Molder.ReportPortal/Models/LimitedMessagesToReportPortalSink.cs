using System;
using System.Collections.Generic;
using Molder.ReportPortal.Extensions;
using ReportPortal.Shared;
using ReportPortal.Shared.Execution.Logging;
using Serilog.Core;
using Serilog.Events;

namespace Molder.ReportPortal.Models
{
    /// <summary>
    /// A limited message sink for reporting logs directly to the Report Portal. 
    /// <see href="https://github.com/reportportal/logger-net-serilog">Original repo</see>
    /// </summary>
    public class LimitedMessagesToReportPortalSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;

        public LimitedMessagesToReportPortalSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;

            LevelMap[LogEventLevel.Debug] = LogMessageLevel.Debug;
            LevelMap[LogEventLevel.Error] = LogMessageLevel.Error;
            LevelMap[LogEventLevel.Fatal] = LogMessageLevel.Fatal;
            LevelMap[LogEventLevel.Information] = LogMessageLevel.Info;
            LevelMap[LogEventLevel.Verbose] = LogMessageLevel.Trace;
            LevelMap[LogEventLevel.Warning] = LogMessageLevel.Warning;
        }

        protected Dictionary<LogEventLevel, LogMessageLevel> LevelMap = new();

        public void Emit(LogEvent logEvent)
        {
            var level = LogMessageLevel.Info;
            if (LevelMap.ContainsKey(logEvent.Level))
            {
                level = LevelMap[logEvent.Level];
            }

            var logMessage = new LogMessage(logEvent.RenderMessage(_formatProvider));
            logMessage.Time = logEvent.Timestamp.UtcDateTime;
            logMessage.Level = level;
            logMessage.Message = logMessage.Message.ToLimitedMessage(LoggerSettings.Settings.MessageSize);

            Context.Current.Log.Message(logMessage);
        }
    }
}