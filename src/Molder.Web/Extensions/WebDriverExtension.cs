using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Microsoft.Extensions.Logging;
using Molder.Helpers;
using Molder.Web.Exceptions;
using OpenQA.Selenium.Support.UI;

namespace Molder.Web.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class WebDriverExtension
    {
        public static IWebElement TryFindElement(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException ex)
            {
                Log.Logger().LogDebug($"Element {by}  not found, because {ex.Message}");
                return null;
            }
        }
        
        public static IWebElement FindBy(this IWebElement parent, By by, IWebDriver driver, int timeoutInSeconds = 30)
        {
            return FindElement(driver, by, parent, timeoutInSeconds);
        }
        
        public static IWebElement FindBy(this IWebDriver driver, By by, int timeoutInSeconds = 30)
        {
            return FindElement(driver, by, timeoutInSeconds);
        }
        
        public static IEnumerable<IWebElement> FindAllBy(this IWebElement parent, By by, IWebDriver driver, int timeoutInSeconds = 30)
        {
            return FindElements(driver, by, parent, timeoutInSeconds);
        }
        public static IEnumerable<IWebElement> FindAllBy(this IWebDriver driver, By by, int timeoutInSeconds = 30)
        {
            return FindElements(driver, by, timeoutInSeconds);
        }
        

        private static IEnumerable<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds <= 0) return driver.FindElements(@by);
            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv => drv.FindElements(@by));
        }
        private static IEnumerable<IWebElement> FindElements(this IWebDriver driver, By by, IWebElement parent, int timeoutInSeconds)
        {
            if (timeoutInSeconds <= 0) return parent.FindElements(by);
            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(_ => parent.FindElements(by));
        }
        
        private static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInMilliseconds)
        {
            if (timeoutInMilliseconds <= 0) return driver.FindElement(@by);
            
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeoutInMilliseconds));
            return wait.Until(drv => drv.FindElement(@by));
        }
        private static IWebElement FindElement(this IWebDriver driver, By by, IWebElement parent, int timeoutInMilliseconds)
        {
            if (timeoutInMilliseconds <= 0) return parent.FindElement(by);
            
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeoutInMilliseconds));
            return wait.Until(_ => parent.FindElement(by));
        }

        public static void Click(this IWebDriver driver, By by, int timeoutInSeconds = 30)
        {
            driver.ExecuteWithRetry(() =>
            {
                var element = driver.FindBy(by, timeoutInSeconds);
                element.Click();
            }, timeoutInSeconds);
        }
        
        public static void ExecuteWithRetry(this IWebDriver driver, Action action, int timeoutInSeconds = 30)
        {
            driver.ExecuteWithRetry(action, DateTime.Now.AddSeconds(timeoutInSeconds));
        }

        public static void ExecuteWithRetry(this IWebDriver driver, Action action, DateTime retryDeadline)
        {
            driver.ExecuteWithRetry(action, retryDeadline, null);
        }

        private static void ExecuteWithRetry(this IWebDriver driver, Action action, DateTime retry, Exception innerxception)
        {
            if (DateTime.Now > retry)
            {
                throw new DriverException(
                    "Could not complete the requested action within the allowed timeout. See inner exception for details.", 
                    innerxception);
            }

            try
            {
                action();
            }
            catch (Exception ex)
            {
                Thread.Sleep(1000);
                driver.ExecuteWithRetry(action, retry, ex);
            }
        }
        
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