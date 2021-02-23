using Molder.Helpers;
using Molder.Web.Extensions;
using Molder.Web.Models.Settings;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using WDSE;
using WDSE.ScreenshotMaker;
using System.Threading;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class DriverProvider : IDriverProvider
    {
        private AsyncLocal<IWebDriver> _driver = new AsyncLocal<IWebDriver> { Value = null };
        public IWebDriver Driver
        {
            get => _driver.Value;
            set
            {
                _driver.Value = value;
            }
        }

        public string PageSource => _driver.Value.PageSource;
        public string Title => _driver.Value.Title;
        public string Url => _driver.Value.Url;
        public string CurrentWindowHandle => _driver.Value.CurrentWindowHandle;
        public int Tabs => _driver.Value.WindowHandles.Count; 
        public ReadOnlyCollection<string> WindowHandles => _driver.Value.WindowHandles;
        public ISetting Settings { get; set; }

        public void CreateDriver(Func<IWebDriver> action, ISetting settings)
        {
            _driver.Value = action();
            this.Settings = settings;

        }
        public IWebDriver GetDriver()
        {
            return _driver.Value;
        }

        public void Back()
        {
            _driver.Value.Navigate().Back();
        }

        public bool Close()
        {
            try
            {
                _driver.Value.Close();
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public void Forward()
        {
            _driver.Value.Navigate().Forward();
        }

        public IElementProvider GetActiveElement()
        {
            var element = _driver.Value.SwitchTo().ActiveElement();
            return new ElementProvider((Settings as BrowserSetting).ElementTimeout)
            {
                Element = element
            };
        }

        public IElementProvider GetElement(By by)
        {
            var element = _driver.Value.Wait((int)(Settings as BrowserSetting).ElementTimeout).ForElement(by).ToExist();
            return new ElementProvider((Settings as BrowserSetting).ElementTimeout)
            {
                Element = element
            };
        }

        public ReadOnlyCollection<IElementProvider> GetElements(By by)
        {
            var elements = _driver.Value.FindElements(by);
            var listElement = new List<IElementProvider>();
            foreach (var element in elements)
            {
                listElement.Add(new ElementProvider((Settings as BrowserSetting).ElementTimeout)
                {
                    Element = element
                });
            }
            return listElement.AsReadOnly();
        }

        public void SwitchTo(int number)
        {
            _driver.Value.SwitchTo().Window(_driver.Value.WindowHandles[number]);
        }

        public IAlertProvider GetAlert()
        {
            var alert = _driver.Value.SwitchTo().Alert();
            return new AlertProvider()
            {
                Alert = alert
            };
        }

        public IDriverProvider GetDefaultFrame()
        {
            var driver = _driver.Value.SwitchTo().DefaultContent();
            return new DriverProvider()
            {
                Driver = driver
            };
        }

        public IDriverProvider GetParentFrame()
        {
            var driver = _driver.Value.SwitchTo().ParentFrame();
            return new DriverProvider()
            {
                Driver = driver
            };
        }

        public IDriverProvider GetFrame(int id)
        {
            var driver = _driver.Value.SwitchTo().Frame(id);
            return new DriverProvider()
            {
                Driver = driver
            };
        }

        public IDriverProvider GetFrame(string name)
        {
            var driver = _driver.Value.SwitchTo().Frame(name);
            return new DriverProvider()
            {
                Driver = driver
            };
        }

        public IDriverProvider GetFrame(By by)
        {
            var element = _driver.Value.FindElement(by);
            var driver = _driver.Value.SwitchTo().Frame(element);
            return new DriverProvider()
            {
                Driver = driver
            };
        }

        public bool GoToUrl(string url)
        {
            try
            {
                _driver.Value.GoToUrl(Settings, url);
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
            _driver.Value.Manage().Window.Maximize();
        }

        public bool Quit()
        {
            try
            {
                _driver.Value.Quit();
                _driver.Value = null;
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
                _driver.Value.Navigate().Refresh();
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
            return _driver.Value.TakeScreenshot(scmkr);
        }

        public bool WindowSize(int width, int height)
        {
            try
            {
                _driver.Value.Manage().Window.Size = new System.Drawing.Size(width, height);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}