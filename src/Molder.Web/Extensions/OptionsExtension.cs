using OpenQA.Selenium;
using System.Collections.Generic;

namespace Molder.Web.Extensions
{
    public static class OptionsExtension
    {
        public static DriverOptions AddCapabilities(this DriverOptions options, Dictionary<string, string> capabilities)
        {
            var _options = options;
            foreach(var (key, value) in capabilities)
            {
                _options.AddAdditionalCapability(key, value);
            };
            return _options;
        }
    }
}