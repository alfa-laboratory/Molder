using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Molder.Helpers;

namespace Molder.Web.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DriverExtension
    {
        public static void GoToUrl(this IWebDriver driver, string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                driver.Navigate().GoToUrl(new Uri(url));
            }
            else
            {
                driver.Navigate().GoToUrl(url);
            }

            Log.Logger().LogDebug($"Go to {url}");
            
            if (BrowserSettings.Settings.Timeout == null) return;
            
            Log.Logger().LogInformation($"Driver wait page ready state complated");
            driver.Wait((int) BrowserSettings.Settings.Timeout).ForPage().ReadyStateComplete();
        }
    }
}