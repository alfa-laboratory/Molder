using EvidentInstruction.Web.Models.Factory.Browser.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Page;
using EvidentInstruction.Web.Models.PageObject.Models.Page.Interfaces;
using EvidentInstruction.Web.Models.Providers;
using EvidentInstruction.Web.Models.Providers.Interfaces;
using EvidentInstruction.Web.Models.Settings;
using EvidentInstruction.Web.Models.Settings.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EvidentInstruction.Web.Models.Factory.Browser
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

        public IDriverProvider _provider = new DriverProvider();

        public string Url { get => _provider.Url; }
        public string Title { get => _provider.Title; }

        public void SetCurrentPage(string name)
        {
            var pages = PageCollection.GetPages();
            if (pages.Any())
            {
                if (pages.ContainsKey(name))
                {
                    _currentPage = (Page)Activator.CreateInstance(pages[name]);
                    _currentPage.SetProvider(_provider);
                    _currentPage.GoToPage();
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

        public IElementProvider GetElement(By by)
        {
            return _provider.GetElement(by);
        }

        public ReadOnlyCollection<IElementProvider> GetElements(By by)
        {
            return _provider.GetElements(by);
        }

        public IElementProvider GetActiveElement()
        {
            return _provider.GetActiveElement();
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

        public IAlertProvider GetAlert()
        {
            return _provider.GetAlert();
        }

        #region Работа с фреймами

        public IDriverProvider GetDefaultFrame()
        {
            return _provider.GetDefaultFrame();
        }

        public IDriverProvider GetFrame(int id)
        {
            return _provider.GetFrame(id);
        }

        public IDriverProvider GetFrame(string name)
        {
            return _provider.GetFrame(name);
        }
        public IDriverProvider GetFrame(By by)
        {
            return _provider.GetFrame(by);
        }
        
        #endregion

        public Screenshot Screenshot()
        {
            return _provider.Screenshot();
        }
    }
}
