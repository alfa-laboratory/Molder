using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Molder.Helpers;
using OpenQA.Selenium.Chrome;

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
        
        public static DriverOptions AddUserProfilePreference(this ChromeOptions options, Dictionary<string, string> userProfilePreference)
        {
            var _options = options;
            
            if ((userProfilePreference is null) || (!userProfilePreference.Any()))
            {
                Log.Logger().LogInformation($"Dictionary with userProfilePreference is null or empty. Return {options.GetType().Name.ToLower()} without UserProfilePreference");
                return _options;
            }
            
            foreach(var (key, value) in userProfilePreference)
            {
                _options.AddUserProfilePreference(key, value);
            }
            
            return _options;
        }
    }
}