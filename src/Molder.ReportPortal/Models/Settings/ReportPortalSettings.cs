using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.ReportPortal.Models.Settings
{
    [ExcludeFromCodeCoverage]
    public class ReportPortalSettings
    {
        private ReportPortalSettings() { }

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
