using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public class WebPageWaitConditions : IWebPageWaitConditions
    {
        private readonly IWebDriver _webDriver;
        private readonly int _waitMs;

        public WebPageWaitConditions(IWebDriver webDriver, int waitMs)
        {
            _webDriver = webDriver;
            _waitMs = waitMs;
        }

        public void TitleToEqual(string title)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs))
                .Until(ExpectedConditions.TitleIs(title));
        }

        public void TitleToContain(string title)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs))
                .Until(ExpectedConditions.TitleContains(title));
        }

        public void UrlToEqual(string url)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs))
                .Until(ExpectedConditions.UrlToBe(url));
        }

        public void UrlToContain(string url)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs))
                .Until(ExpectedConditions.UrlContains(url));
        }

        public void UrlToMatch(string regex)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs))
                .Until(ExpectedConditions.UrlMatches(regex));
        }


        public void ReadyStateComplete()
        {
            WaitForJqueryAjax();
            new WebDriverWait(_webDriver, TimeSpan.FromMilliseconds(_waitMs))
                .Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public void WaitForJqueryAjax()
        {
            var delay = 10;
            while (delay > 0)
            {
                var jquery = (bool)((IJavaScriptExecutor) _webDriver)
                    .ExecuteScript("return window.jQuery == undefined");
                if (jquery)
                {
                    break;
                }
                var ajaxIsComplete = (bool)((IJavaScriptExecutor) _webDriver)
                    .ExecuteScript("return window.jQuery.active == 0");
                if (ajaxIsComplete)
                {
                    break;
                }
                Thread.Sleep(1000);
                delay--;
            }
        }
    }
}