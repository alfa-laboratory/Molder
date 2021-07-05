using Molder.Web.Exceptions;
using Molder.Web.Extensions;
using Molder.Web.Models.PageObjects.Alerts;
using Molder.Web.Models.PageObjects.Pages;
using Molder.Web.Models.Providers;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Molder.Helpers;

namespace Molder.Web.Models.Browser
{
    public abstract class Browser : IBrowser, IDisposable
    {
        #region Node for current *
        
        private AsyncLocal<Node> _currentPage = new AsyncLocal<Node> { Value = null };
        
        #endregion

        private AsyncLocal<IDriverProvider> _provider = new AsyncLocal<IDriverProvider> { Value = new DriverProvider() };

        protected IDriverProvider WebDriver
        {
            get => _provider.Value;
            set => _provider.Value = value;
        }

        public string Url => WebDriver.Url;
        public string Title => WebDriver.Title;
        public int Tabs => WebDriver.Tabs;

        public abstract SessionId SessionId { get; protected set; }

        public void SetCurrentPage(string name, bool loading = true)
        {
            Log.Logger().LogInformation($"SetCurrentPage is {name} {(loading ? "with load primary element" : "without load element")}");
            var page = TreePages.Get().SearchPageBy(name);
            _currentPage.Value = page;
            (_currentPage.Value.Object as Page)?.SetProvider(_provider.Value);
            ((Page) _currentPage.Value.Object).Root = page;
            ((Page) _currentPage.Value.Object).Local = null;
            
            if (!loading) return;
            
            var gtp = ((Page) _currentPage.Value.Object).GoToPage();
            if(!gtp) throw new PageException($"Going to page \"{name}\" at \"{(_currentPage.Value.Object as Page)?.Url}\" failed");
        }

        public void UpdateCurrentPage(string name)
        {
            this.SetCurrentPage(name, false);
        }

        public IPage GetCurrentPage()
        {
            if (_currentPage == null)
            {
                throw new NullReferenceException("Current page is null.");
            }
            return _currentPage.Value.Object as IPage;
        }

        public bool Close()
        {
            return WebDriver.Close();
        }

        public bool Quit()
        {
            return WebDriver.Quit();
        }

        public bool WindowSize(int width, int height)
        {
            Log.Logger().LogInformation($"Browser size is ({width},{height})");
            return WebDriver.WindowSize(width, height);
        }

        public void Maximize()
        {
            Log.Logger().LogInformation($"Browser size is maximize");
            WebDriver.Maximize();
        }

        public void Back()
        {
            Log.Logger().LogInformation($"Go a back page");
            WebDriver.Back();
        }

        public void Forward()
        {
            Log.Logger().LogInformation($"Go a forvard page");
            WebDriver.Forward();
        }

        public bool GoToPage(string url)
        {
            Log.Logger().LogInformation($"GoToUrl {url}");
            return WebDriver.GoToUrl(url);
        }

        public bool Refresh()
        {
            Log.Logger().LogInformation($"Refresh page");
            return WebDriver.Refresh();
        }

        public IAlert Alert()
        {
            return new Alert(WebDriver);
        }

        public void SwitchTo(int number)
        {
            Log.Logger().LogInformation($"Switch to {number} page");
            WebDriver.SwitchTo(number);
        }

        public byte[] Screenshot()
        {
            return WebDriver.Screenshot();
        }

        public void Dispose() {  }
    }
}
