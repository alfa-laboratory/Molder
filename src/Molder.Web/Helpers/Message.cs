using System;
using OpenQA.Selenium;

namespace Molder.Web.Helpers
{
    public static class Message
    {
        public static string CreateMessage(DriverOptions driverOptions)
        {
            var message = $@"{Environment.NewLine}{driverOptions}{Environment.NewLine}";
            return message;
        }
    }
}
