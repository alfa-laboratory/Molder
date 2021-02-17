using Molder.Web.Exceptions;
using Molder.Web.Extensions;
using Molder.Web.Models.PageObjects.Alerts;
using Molder.Web.Models.PageObjects.Pages;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Settings;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace Molder.Web.Models.Browser
{
    public abstract class Browser : IBrowser, IDisposable
    {
        protected AsyncLocal<ISetting> _settings = new AsyncLocal<ISetting>();
        protected AsyncLocal<Proxy.Proxy> _proxyServer = new AsyncLocal<Proxy.Proxy>();

        public AsyncLocal<Node> _currentPage = new AsyncLocal<Node> { Value = null };
        public ISetting Settings
        {
            get
            {
                return _settings.Value;
            }
            protected set
            {
                _settings.Value = value;
            }
        }

        public AsyncLocal<IDriverProvider> _provider = new AsyncLocal<IDriverProvider> { Value = new DriverProvider() };

        public string Url { get => _provider.Value.Url; }
        public string Title { get => _provider.Value.Title; }
        public int Tabs { get => _provider.Value.Tabs; }

        public abstract SessionId SessionId { get; protected set; }

        public void SetCurrentPage(string name, bool loading = true)
        {
            var page = TreePages.Get().SearchPageBy(name);
            _currentPage.Value = page;
            (_currentPage.Value.Object as Page).SetProvider(_provider.Value);
            (_currentPage.Value.Object as Page).Root = page;

            if (loading)
            {
                var gtp = (_currentPage.Value.Object as Page).GoToPage();
                if(!gtp) throw new PageException($"Переход на страницу \"{name}\" по адресу \"{(_currentPage.Value.Object as Page).Url}\" выполнить не удалось.");
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
            return _currentPage.Value.Object as IPage;
        }

        public bool Close()
        {
            return _provider.Value.Close();
        }

        public bool Quit()
        {
            return _provider.Value.Quit();
        }

        public bool WindowSize(int width, int height)
        {
            return _provider.Value.WindowSize(width, height);
        }

        public void Maximize()
        {
            _provider.Value.Maximize();
        }

        public void Back()
        {
            _provider.Value.Back();
        }

        public void Forward()
        {
            _provider.Value.Forward();
        }

        public bool GoToPage(string url)
        {
            return _provider.Value.GoToUrl(url);
        }

        public bool Refresh()
        {
            return _provider.Value.Refresh();
        }

        public IAlert Alert()
        {
            return new Alert(_provider.Value);
        }

        public void SwitchTo(int number)
        {
            _provider.Value.SwitchTo(number);
        }

        public byte[] Screenshot()
        {
            return _provider.Value.Screenshot();
        }

        public void Dispose()
        {
            _proxyServer.Value.Dispose();
            _proxyServer.Value = null;
        }
    }
}
