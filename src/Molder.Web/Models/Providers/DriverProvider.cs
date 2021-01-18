using Molder.Helpers;
using Molder.Web.Extensions;
using Molder.Web.Models.Providers.Interfaces;
using Molder.Web.Models.Settings;
using Molder.Web.Models.Settings.Interfaces;
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

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class DriverProvider : IDriverProvider
    {
        [ThreadStatic]
        private IWebDriver _driver = null;

        public string PageSource => _driver.PageSource;

        public string Title => _driver.Title;

        public string Url => _driver.Url;

        public string CurrentWindowHandle => _driver.CurrentWindowHandle;

        public int Tabs => _driver.WindowHandles.Count; 

        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public ISetting Settings { get; set; }

        public void CreateDriver(Func<IWebDriver> action, ISetting settings)
        {
            _driver = action();
            this.Settings = settings;

        }
        public IWebDriver GetDriver()
        {
            return _driver;
        }

        public void Back()
        {
            _driver.Navigate().Back();
        }

        public bool Close()
        {
            try
            {
                _driver.Close();
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public void Forward()
        {
            _driver.Navigate().Forward();
        }

        public IElementProvider GetActiveElement()
        {
            var element = _driver.SwitchTo().ActiveElement();
            return new ElementProvider((Settings as BrowserSetting).ElementTimeout)
            {
                Element = element
            };
        }

        public IElementProvider GetElement(By by)
        {
            var element = _driver.Wait((int)(Settings as BrowserSetting).ElementTimeout).ForElement(by).ToExist();
            return new ElementProvider((Settings as BrowserSetting).ElementTimeout)
            {
                Element = element
            };
        }

        public ReadOnlyCollection<IElementProvider> GetElements(By by)
        {
            var elements = _driver.FindElements(by);
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
            _driver.SwitchTo().Window(_driver.WindowHandles[number]);
        }

        public IAlertProvider GetAlert()
        {
            var alert = _driver.SwitchTo().Alert();
            return new AlertProvider()
            {
                Alert = alert
            };
        }

        public IDriverProvider GetDefaultFrame()
        {
            var driver = _driver.SwitchTo().DefaultContent();
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public IDriverProvider GetParentFrame()
        {
            var driver = _driver.SwitchTo().ParentFrame();
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public IDriverProvider GetFrame(int id)
        {
            var driver = _driver.SwitchTo().Frame(id);
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public IDriverProvider GetFrame(string name)
        {
            var driver = _driver.SwitchTo().Frame(name);
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public IDriverProvider GetFrame(By by)
        {
            var element = _driver.FindElement(by);
            var driver = _driver.SwitchTo().Frame(element);
            return new DriverProvider()
            {
                _driver = driver
            };
        }

        public bool GoToUrl(string url)
        {
            try
            {
                _driver.GoToUrl(Settings, url);
                _driver.Wait((int)(Settings as BrowserSetting).ElementTimeout).ForPage().ReadyStateComplete();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Page by url \"{url}\" is not correct. Exception is \"{ex.Message}\"");
                return false;
            }

        }

        public void Maximize()
        {
            _driver.Manage().Window.Maximize();
        }

        public bool Quit()
        {
            try
            {
                _driver.Quit();
                _driver = null;
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
                _driver.Navigate().Refresh();
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
            return _driver.TakeScreenshot(scmkr);
        }

        public bool WindowSize(int width, int height)
        {
            try
            {
                _driver.Manage().Window.Size = new System.Drawing.Size(width, height);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}