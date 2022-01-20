using Molder.Web.Extensions;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Molder.Helpers;
using Molder.Web.Exceptions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Settings;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WDSE;
using WDSE.ScreenshotMaker;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class DriverProvider : IDriverProvider
    {
        #region  WebDriver
        private AsyncLocal<IWebDriver> _driver = new() { Value = null };
        public IWebDriver WebDriver
        {
            get => _driver.Value;
            set => _driver.Value = value;
        }
        #endregion
        
        public string PageSource => WebDriver.PageSource;
        public string Title => WebDriver.Title;
        public string Url => WebDriver.Url;
        public string CurrentWindowHandle => WebDriver.CurrentWindowHandle;
        public int Tabs => WebDriver.WindowHandles.Count; 
        public ReadOnlyCollection<string> WindowHandles => WebDriver.WindowHandles;
        public void CreateDriver(Func<IWebDriver> action)
        {
            try
            {
                WebDriver = action();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Create driver is return error with message {ex.Message}");
            }
        }
        public IWebDriver GetDriver()
        {
            return WebDriver;
        }
        public void Back()
        {
            try
            {
                WebDriver.Navigate().Back();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Navigate().Back() is return error with message {ex.Message}");
            }
        }
        public void Close()
        {
            try
            {
                WebDriver.Close();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Close() is return error with message {ex.Message}");
            }
        }
        public void Forward()
        {
            try
            {
                WebDriver.Navigate().Forward();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Navigate().Forward() is return error with message {ex.Message}");
            }
        }
        public IElementProvider GetElement(string locator, How how)
        {
            var by = how.GetBy(locator);
            var element = WebDriver.Wait((int)BrowserSettings.Settings.Timeout).ForElement(by).ToExist();
            return new ElementProvider(BrowserSettings.Settings.Timeout, by)
            {
                WebElement = element,
                WebDriver = WebDriver
            };
        }
        public IEnumerable<IElementProvider> GetElements(string locator, How how)
        {      
            var by = how.GetBy(locator);
            var elements = WebDriver.FindAllBy(by);
            var listElement = elements.Select(element => new ElementProvider((int)BrowserSettings.Settings.Timeout, by) {WebElement = element, WebDriver = WebDriver}).Cast<IElementProvider>().ToList();
            return listElement;
        }
        public void SwitchTo(int number)
        {
            try
            {
                Log.Logger().LogInformation($"SwitchTo().Window to number");
                WebDriver.SwitchTo().Window(WebDriver.WindowHandles[number]);
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Window is return error with message {ex.Message}");
            }
        }
        public IAlertProvider GetAlert()
        {
            try
            {
                var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds((long) BrowserSettings.Settings.Timeout));
                var alert = wait.Until(ExpectedConditions.AlertIsPresent());
                return new AlertProvider()
                {
                    Alert = alert
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"Switch().Alert is return error with message {ex.Message}");
            }
        }
        public IDriverProvider GetDefaultFrame()
        {
            try
            {
                var driver = WebDriver.SwitchTo().DefaultContent();
                driver.Wait((int)BrowserSettings.Settings.Timeout).ForPage().ReadyStateComplete();
                return new DriverProvider()
                {
                    WebDriver = driver
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().DefaultContent is return error with message {ex.Message}");
            }
        }
        public IDriverProvider GetParentFrame()
        {
            try
            {
                var driver = WebDriver.SwitchTo().ParentFrame();
                driver.Wait((int)BrowserSettings.Settings.Timeout).ForPage().ReadyStateComplete();
                return new DriverProvider()
                {
                    WebDriver = driver
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().ParentFrame is return error with message {ex.Message}");
            }
        }
        public IDriverProvider GetFrame(int id)
        {
            try
            {
                Log.Logger().LogDebug($"SwitchTo().Frame by id \"{id}\"");
                var driver = WebDriver.SwitchTo().Frame(id);
                driver.Wait((int)BrowserSettings.Settings.Timeout).ForPage().ReadyStateComplete();
                return new DriverProvider()
                {
                    WebDriver = driver
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Frame by id \"{id}\" is return error with message {ex.Message}");
            }
        }
        public IDriverProvider GetFrame(string name)
        {
            try
            {
                Log.Logger().LogDebug($"SwitchTo().Frame by name \"{name}\"");
                var driver = WebDriver.SwitchTo().Frame(name);
                driver.Wait((int)BrowserSettings.Settings.Timeout).ForPage().ReadyStateComplete();
                return new DriverProvider()
                {
                    WebDriver = driver
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Frame by name \"{name}\" is return error with message {ex.Message}");
            }
        }
        public IDriverProvider GetFrame(By by)
        {
            try
            {
                Log.Logger().LogDebug($"SwitchTo().Frame by locator");
                var element = WebDriver.FindElement(by);
                var driver = WebDriver.SwitchTo().Frame(element);
                driver.Wait((int)BrowserSettings.Settings.Timeout).ForPage().ReadyStateComplete();
                return new DriverProvider()
                {
                    WebDriver = driver
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Frame by locator is return error with message {ex.Message}");
            }
        }
        public void GoToUrl(string url)
        {
            try
            {
                Log.Logger().LogDebug($"Go to \"{url}\"");
                WebDriver.GoToUrl(url);
            }
            catch (Exception ex)
            {
                throw new DriverException($"Go to \"{url}\" is return error with message {ex.Message}");
            }
        }
        public void Maximize()
        {
            try
            {
                WebDriver.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Manage().Window.Maximize is return error with message {ex.Message}");
            }
        }
        public void Quit()
        {
            try
            {
                WebDriver.Quit();
                WebDriver = null;
            }
            catch (Exception ex)
            {
                throw new DriverException($"Quit browser is return error with message {ex.Message}");
            }
        }
        public void Refresh()
        {
            try
            {
                WebDriver.Navigate().Refresh();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Navigate().Refresh is return error with message {ex.Message}");
            }
        }
        public byte[] Screenshot()
        {
            var screenshotMaker = new ScreenshotMaker();
            screenshotMaker.RemoveScrollBarsWhileShooting();
            return WebDriver.TakeScreenshot(screenshotMaker);
        }
        public void WindowSize(int width, int height)
        {
            try
            {
                Log.Logger().LogDebug($"Set browser window size as ({width},{height})");
                WebDriver.Manage().Window.Size = new System.Drawing.Size(width, height);
            }
            catch (Exception ex)
            {
                throw new DriverException($"Manage().Window.Size as ({width},{height}) is return error with message {ex.Message}");
            }
        }
    }
}