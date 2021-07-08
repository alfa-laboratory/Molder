using Microsoft.Extensions.Logging;
using Molder.Helpers;
using OpenQA.Selenium;

namespace Molder.Web.Helpers
{
    public static class Message
    {
        public static string CreateMessage(this DriverOptions driverOptions)
        {
            if (driverOptions is { }) return $@"{driverOptions}";
            Log.Logger().LogInformation("DriverOptions for create string is null");
            return string.Empty;
        }
    }
}
