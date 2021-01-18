using Molder.Controllers;
using Molder.Web.Exceptions;
using Molder.Web.Models.Factory.Browser.Interfaces;
using Molder.Web.Models.PageObject.Models.Alert;
using Molder.Web.Models.PageObject.Models.Page;
using Molder.Web.Models.PageObject.Models.Page.Interfaces;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Providers.Interfaces;
using Molder.Web.Models.Settings;
using Molder.Web.Models.Settings.Interfaces;
using OpenQA.Selenium.Remote;
using System;
using System.Linq;

namespace Molder.Web.Models.Factory.Browser
{
    public abstract class Browser : IBrowser
    {
        [ThreadStatic]
        private ISetting _settings;

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
        private Page _currentPage = null;

        [ThreadStatic]
        public IDriverProvider _provider = new DriverProvider();

        public string Url { get => _provider.Url; }
        public string Title { get => _provider.Title; }
        public int Tabs { get => _provider.Tabs; }

        public abstract SessionId SessionId { get; protected set; }

        public void SetCurrentPage(string name, bool loading = true)
        {
            var pages = PageCollection.GetPages();
            if (pages.Any())
            {
                if (pages.ContainsKey(name))
                {
                    _currentPage = (Page)Activator.CreateInstance(pages[name]);
                    _currentPage.SetProvider(_provider);

                    if (loading)
                    {
                        var gtp = _currentPage.GoToPage();
                        if(!gtp) throw new PageException($"Переход на страницу \"{name}\" по адресу \"{_currentPage.Url}\" выполнить не удалось.");
                    }
                }
                else
                {
                    throw new NullReferenceException($"Не найдена страница с названием \"{name}\". Убедитесь в наличии атрибута [Page] у классов страниц.");
                }
            }
            else
            {
                throw new NullReferenceException($"Не найдены страницы. Убедитесь в наличии атрибута [Page] у классов страниц и подключений их к проекту с тестами.");
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
            return _currentPage;
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

        public PageObject.Models.Alert.Interfaces.IAlert Alert()
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
