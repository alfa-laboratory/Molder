using Molder.Web.Exceptions;
using Molder.Web.Extensions;
using Molder.Web.Models.PageObjects.Alerts;
using Molder.Web.Models.PageObjects.Pages;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Settings;
using OpenQA.Selenium.Remote;
using System;

namespace Molder.Web.Models.Browser
{
    public abstract class Browser : IBrowser
    {
        [ThreadStatic]
        private ISetting _settings;

        [ThreadStatic]
        protected Proxy.Proxy _proxyServer;

        [ThreadStatic]
        public Node _currentPage = null;

        public ISetting Settings
        {
            get
            {
                return _settings;
            }
            protected set
            {
                _settings = value;
            }
        }

        [ThreadStatic]
        public IDriverProvider _provider = new DriverProvider();

        public string Url { get => _provider.Url; }
        public string Title { get => _provider.Title; }
        public int Tabs { get => _provider.Tabs; }

        public abstract SessionId SessionId { get; protected set; }

        public void SetCurrentPage(string name, bool loading = true)
        {
            var page = TreePages.Get().SearchPageBy(name);
            _currentPage = page;
            (_currentPage.Object as Page).SetProvider(_provider);
            (_currentPage.Object as Page).Root = page;

            if (loading)
            {
                var gtp = (_currentPage.Object as Page).GoToPage();
                if(!gtp) throw new PageException($"Переход на страницу \"{name}\" по адресу \"{(_currentPage.Object as Page).Url}\" выполнить не удалось.");
            }
        }

        public void UpdateCurrentPage(string name)
        {
            this.SetCurrentPage(name, false);
        }

        public IPage GetCurrentPage()
        {
            if (_currentPage == null)
            {
                throw new NullReferenceException("Текущая страница не задана");
            }
            return _currentPage.Object as IPage;
        }

        public bool Close()
        {
            return _provider.Close();
        }

        public bool Quit()
        {
            return _provider.Quit();
        }

        public bool WindowSize(int width, int height)
        {
            return _provider.WindowSize(width, height);
        }

        public void Maximize()
        {
            _provider.Maximize();
        }

        public void Back()
        {
            _provider.Back();
        }

        public void Forward()
        {
            _provider.Forward();
        }

        public bool GoToPage(string url)
        {
            return _provider.GoToUrl(url);
        }

        public bool Refresh()
        {
            return _provider.Refresh();
        }

        public IAlert Alert()
        {
            return new Alert(_provider);
        }

        public void SwitchTo(int number)
        {
            _provider.SwitchTo(number);
        }

        public byte[] Screenshot()
        {
            return _provider.Screenshot();
        }
    }
}
