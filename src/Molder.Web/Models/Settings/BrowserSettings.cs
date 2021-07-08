using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Models.Settings
{
    [ExcludeFromCodeCoverage]
    public class BrowserSettings
    {
        private BrowserSettings() {}

        private static Lazy<Settings> _settings = new Lazy<Settings>(() => null);
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