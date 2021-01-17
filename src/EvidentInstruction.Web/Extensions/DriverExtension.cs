using EvidentInstruction.Web.Models.Settings;
using EvidentInstruction.Web.Models.Settings.Interfaces;
using FluentAssertions;
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

        public static bool IsRemoteRunning(this ISetting setting)
        {
            var browserSetting = setting as BrowserSetting;

            if (browserSetting.Remote == true)
            {
                browserSetting.RemoteUrl.Should().NotBeNullOrWhiteSpace("Remote url for remote browser launch is null or whitespace");
                bool isValid = Uri.TryCreate(browserSetting.RemoteUrl, UriKind.Absolute, out Uri outUrl);
                isValid.Should().BeTrue("Remote url for remote browser launch is not valid");
                browserSetting.RemoteVersion.Should().NotBeNullOrWhiteSpace("Remote version for remote browser launch is null or whitespace");
                return true;
            }
            return false;
        }
    }
}
