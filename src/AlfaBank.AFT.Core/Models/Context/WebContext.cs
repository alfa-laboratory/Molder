using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using AlfaBank.AFT.Core.Helpers;
using AlfaBank.AFT.Core.Infrastructures.Web;
using AlfaBank.AFT.Core.Models.Web;
using AlfaBank.AFT.Core.Models.Web.Attributes;
using AlfaBank.AFT.Core.Models.Web.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Models.Context
{
    public class WebContext
    {
        private readonly Dictionary<string, Type> _allPages = null;
        private IPage _currentPage = null;
        private readonly Driver _driverSupport;

        public WebContext(Driver driverSupport)
        {
            this._driverSupport = driverSupport;
            _allPages = InitializePages();
        }
        public void Start(BrowserType browser, bool remote = false, DriverOptions options = null, string version = null, string url = null, PlatformType platform = PlatformType.Any)
        {
            if (!(_driverSupport.WebDriver is null))
            {
                return;
            }

            if (remote)
            {
                if ((version is null) || (url is null))
                {
                    return;
                }

                switch (browser)
                {
                    case BrowserType.Chrome:
                    case BrowserType.Mozila:
                    {
#pragma warning disable 618
                        var capabilities = new DesiredCapabilities(browser.ToString().ToLower(), version, new Platform(platform));
                        capabilities?.SetCapability("enableVNC", true);
#pragma warning restore 618

                        _driverSupport.WebDriver = new RemoteWebDriver(new Uri(url), capabilities);
                        return;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(browser), browser, null);
                }
            }

            switch (browser)
            {
                case BrowserType.Chrome:
                case BrowserType.Mozila:
                    Start(browser, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(browser), browser, null);
            }
        }
        public void Start(BrowserType browser, DriverOptions options = null)
        {
            switch (browser)
            {
                case BrowserType.Chrome:
                {
                    _driverSupport.WebDriver = options != null ? new ChromeDriver((ChromeOptions)options) : new ChromeDriver();
                    break;
                }
                case BrowserType.Mozila:
                {
                    _driverSupport.WebDriver = options != null ? new FirefoxDriver((FirefoxOptions)options) : new FirefoxDriver();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(browser), browser, null);
            }
        }
        public void Stop()
        {
            if (_driverSupport.WebDriver is null)
            {
                return;
            }

            try
            {
                _driverSupport.WebDriver.Quit();
                _driverSupport.WebDriver.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e, "WebDriver stop error");
            }

            _driverSupport.WebDriver = null;
        }
        public void Dispose()
        {
            DisposeDriverService.FinishHim(_driverSupport.WebDriver);
        }
        public void SetCurrentPageBy(string name, bool withLoad = false)
        {
            if (_allPages.Any())
            {
                if (_allPages.ContainsKey(name))
                {
                    _currentPage = (IPage)Activator.CreateInstance(_allPages[name], _driverSupport);

                    if(withLoad)
                    {
                        _currentPage.GoToPage();
                    }

                    _currentPage.IsPageLoad();
                }
            }
        }
        public IPage GetCurrentPage()
        {
            if (_currentPage == null) throw new InvalidOperationException("Текущая страница не задана");
            return _currentPage;
        }
        public void SetTimeout(int sec)
        {
            this._driverSupport.Timeout = sec;
        }
        public int GetTimeout() => _driverSupport.Timeout;
        public void SetSizeBrowser(int width, int height)
        {
            if (this._driverSupport.WebDriver != null)
                this._driverSupport.WebDriver.Manage().Window.Size = new Size(width, height);
        }
        public Size GetSizeBrowser()
        {
            if (this._driverSupport.WebDriver != null)
                return this._driverSupport.WebDriver.Manage().Window.Size;

            return new Size(0, 0);
        }
        public void GoToUrl(string url)
        {
            this._driverSupport.WebDriver.Navigate().GoToUrl(new Uri(url));
            this._driverSupport.WebDriver.Wait(this._driverSupport.Timeout).ForPage().ReadyStateComplete();
        }
        public void Maximize()
        {
            if (this._driverSupport.WebDriver != null)
                this._driverSupport.WebDriver.Manage().Window.Maximize();
        }
        public void GoToTab(int number)
        {
             _driverSupport.WebDriver.SwitchTo().Window(_driverSupport.WebDriver.WindowHandles[number]);
        }
        public int GetCountTabs() => this._driverSupport.WebDriver.WindowHandles.Count();
        private Dictionary<string, Type> InitializePages()
        {
            var projects = AppDomain.CurrentDomain.GetAssemblies();
            Dictionary<string, Type> allClasses = new Dictionary<string, Type>();

            foreach (var project in projects)
            {
                var classes = project.GetTypes().Where(t => t.IsClass).Where(t => t.GetCustomAttribute(typeof(PageAttribute), true) != null);

                foreach (var cl in classes)
                {
                    allClasses.Add(cl.GetCustomAttribute<PageAttribute>().Name, cl);
                }

            }

            return allClasses;
        }
    }
}
