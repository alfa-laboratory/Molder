using Molder.Helpers;
using Molder.Web.Extensions;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using WDSE;
using WDSE.ScreenshotMaker;
using Molder.Web.Models.Settings;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class DriverProvider : IDriverProvider
    {
        #region  WebDriver

        private AsyncLocal<IWebDriver> _driver = new AsyncLocal<IWebDriver> { Value = null };
        protected IWebDriver WebDriver
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
            WebDriver = action();
        }
        public IWebDriver GetDriver()
        {
            return WebDriver;
        }

        public void Back()
        {
            WebDriver.Navigate().Back();
        }

        public bool Close()
        {
            try
            {
                WebDriver.Close();
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public void Forward()
        {
            WebDriver.Navigate().Forward();
        }

        public IElementProvider GetActiveElement()
        {
            var element = WebDriver.SwitchTo().ActiveElement();
            return new ElementProvider(BrowserSettings.Settings.Timeout)
            {
                WebElement = element
            };
        }

        public IElementProvider GetElement(By by)
        {
            var element = WebDriver.Wait((int)BrowserSettings.Settings.Timeout).ForElement(by).ToExist();
            return new ElementProvider(BrowserSettings.Settings.Timeout)
            {
                WebElement = element
            };
        }

        public ReadOnlyCollection<IElementProvider> GetElements(By by)
        {
            var elements = WebDriver.FindElements(by);
            var listElement = elements.Select(element => new ElementProvider((int)BrowserSettings.Settings.Timeout) {WebElement = element}).Cast<IElementProvider>().ToList();
            return listElement.AsReadOnly();
        }

        public void SwitchTo(int number)
        {
            _driver.Value.SwitchTo().Window(WebDriver.WindowHandles[number]);
        }

        public IAlertProvider GetAlert()
        {
            var alert = WebDriver.SwitchTo().Alert();
            return new AlertProvider()
            {
                Alert = alert
            };
        }

        public IDriverProvider GetDefaultFrame()
        {
            var driver = WebDriver.SwitchTo().DefaultContent();
            return new DriverProvider()
            {
                WebDriver = driver
            };
        }

        public IDriverProvider GetParentFrame()
        {
            var driver = WebDriver.SwitchTo().ParentFrame();
            return new DriverProvider()
            {
                WebDriver = driver
            };
        }

        public IDriverProvider GetFrame(int id)
        {
            var driver = WebDriver.SwitchTo().Frame(id);
            return new DriverProvider()
            {
                WebDriver = driver
            };
        }

        public IDriverProvider GetFrame(string name)
        {
            var driver = WebDriver.SwitchTo().Frame(name);
            return new DriverProvider()
            {
                WebDriver = driver
            };
        }

        public IDriverProvider GetFrame(By by)
        {
            var element = WebDriver.FindElement(by);
            var driver = WebDriver.SwitchTo().Frame(element);
            return new DriverProvider()
            {
                WebDriver = driver
            };
        }

        public bool GoToUrl(string url)
        {
            try
            {
                WebDriver.GoToUrl(url);
                return true;
            }
            catch (WebDriverException ex)
            {
                Log.Logger().LogError($"Page by url \"{url}\" is not correct. Exception is \"{ex.Message}\"");
                return false;
            }
        }

        public void Maximize()
        {
            WebDriver.Manage().Window.Maximize();
        }

        public bool Quit()
        {
            try
            {
                WebDriver.Quit();
                WebDriver = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Refresh()
        {
            try
            {
                WebDriver.Navigate().Refresh();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] Screenshot()
        {
            var scmkr = new ScreenshotMaker();
            scmkr.RemoveScrollBarsWhileShooting();
            return WebDriver.TakeScreenshot(scmkr);
        }

        public bool WindowSize(int width, int height)
        {
            try
            {
                WebDriver.Manage().Window.Size = new System.Drawing.Size(width, height);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}