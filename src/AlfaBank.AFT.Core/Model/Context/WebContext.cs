using System;
using System.Collections.Generic;
using System.Diagnostics;
using AlfaBank.AFT.Core.Infrastructure.Web;
using AlfaBank.AFT.Core.Model.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Context
{
    public class WebContext : IDisposable
    {
        public IWebDriver WebDriver { get; private set; }
        public Dictionary<string, Element> PageObject { get; set; }
        public int Timeout { get; set; } = 500;
        public void Start(BrowserType browser, bool remote = false, DriverOptions options = null, string version = null, string url = null, PlatformType platform = PlatformType.Any)
        {
            if (!(WebDriver is null))
            {
                return;
            }

            if (remote)
            {
                if((version is null) || (url is null))
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

                        WebDriver = new RemoteWebDriver(new Uri(url), capabilities);
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
                    WebDriver = options != null ? new ChromeDriver((ChromeOptions) options) : new ChromeDriver(); 
                    break;
                }
                case BrowserType.Mozila:
                {
                    WebDriver = options != null ? new FirefoxDriver((FirefoxOptions)options) : new FirefoxDriver();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(browser), browser, null);
            }
        }
        public void Stop()
        {
            if (WebDriver is null)
            {
                return;
            }

            try
            {
                WebDriver.Quit();
                WebDriver.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e, "WebDriver stop error");
            }

            WebDriver = null;
        }
        public void Dispose()
        {
            if (WebDriver is null)
            {
                return;
            }

            try
            {
               WebDriver.Quit();
               WebDriver.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e, "WebDriver stop error");
            }

            WebDriver = null;
        }
    }
}
