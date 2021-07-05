using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;

namespace Molder.Web.Extensions
{
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

            if (BrowserSettings.Settings.Timeout != null)
                driver.Wait((int) BrowserSettings.Settings.Timeout).ForPage().ReadyStateComplete();
        }
    }
}
