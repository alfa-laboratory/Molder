using EvidentInstruction.Web.Models.Settings;
using EvidentInstruction.Web.Models.Settings.Interfaces;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;

namespace EvidentInstruction.Web.Extensions
{
    public static class DriverExtension
    {
        public static void GoToUrl(this IWebDriver driver, ISetting settings, string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                driver.Navigate().GoToUrl(new Uri(url));
            }
            else
            {
                driver.Navigate().GoToUrl(url);
            }
            driver.Wait((int)
                ((BrowserSetting)settings).Timeout).ForPage().ReadyStateComplete();
        }
    }
}
