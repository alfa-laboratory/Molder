using System;

namespace Molder.ReportPortal.Models
{
    public class LoggerSettings
    {
        private LoggerSettings() {}

        private static Lazy<Settings> _settings = new(() => null);
        public static Settings Settings
        {
            get => _settings.Value;
            set
            {
                _settings = new Lazy<Settings>(() => value);
            }
        }
    }
}