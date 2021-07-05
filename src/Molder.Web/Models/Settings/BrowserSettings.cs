using System;

namespace Molder.Web.Models.Settings
{
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
