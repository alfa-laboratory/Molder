using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Molder.Helpers;

namespace Molder.Web.Extensions
{
    public static class OptionsExtension
    {
        public static DriverOptions AddCapabilities(this DriverOptions options, Dictionary<string, string> capabilities)
        {
            var _options = options;
            
            if ((capabilities is null) || (!capabilities.Any()))
            {
                Log.Logger().LogInformation($"Dictionary with capabilities is null or empty. Return {options.GetType().Name.ToLower()} without DesireCapability");
                return _options;
            }
            
            foreach(var (key, value) in capabilities)
            {
                _options.AddAdditionalCapability(key, value);
            }
            
            return _options;
        }
    }
}