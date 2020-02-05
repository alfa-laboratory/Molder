using AlfaBank.AFT.Core.Models.Context;
using AlfaBank.AFT.Core.Models.Web.Attributes;
using AlfaBank.AFT.Core.Models.Web.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace AlfaBank.AFT.Core.Models.Web
{
    public class Page : IPage
    {
        private ThreadLocal<IElement> _webElement { get; set; } = new ThreadLocal<IElement>();
        public IElement WebElement
        {
            get
            {
                if (!_webElement.IsValueCreated)
                {
                    throw new NullReferenceException(
                        "Элемент не создан");
                }

                return _webElement.Value;
            }
            set { _webElement.Value = value; }
        }

        private readonly Dictionary<string, IElement> _allElements;
        private readonly List<IElement> _hiddenElements;
        private readonly List<IElement> _primaryElemets;

        private readonly Driver _driver;
        private readonly VariableContext _context;

        public Page(Driver driver, VariableContext context)
        {
            _driver = driver;
            _context = context;
            _allElements = new Dictionary<string, IElement>();
            _hiddenElements = new List<IElement>();
            _primaryElemets = new List<IElement>();
            InitializeElements();
        }

        public string Name => this.GetType().GetCustomAttribute<PageAttribute>()?.Name;
        public string Url => this._driver.WebDriver.Url;
        public string AttrUrl => _context.ReplaceVariablesInXmlBody(this.GetType().GetCustomAttribute<PageAttribute>()?.Url);
        public string Title => this._driver.WebDriver.Title;

        public IElement GetElementByName(string name)
        {
            if (!_allElements.Any())
                throw new ArgumentException($"Элемент \"{name}\" не инициализирован на странице \"{Name}\"");
            if (!_allElements.ContainsKey(name))
                throw new ArgumentException($"Элемент \"{name}\" не инициализирован на странице \"{Name}\"");
            _allElements[name].SetDriver(_driver);
            return _allElements[name];
        }

        public List<IElement> GetHiddenElements()
        {
            if (_hiddenElements.Any())
            {
                return _hiddenElements;
            }
            return null;
        }

        public List<IElement> GetPrimaryElements()
        {
            if (_primaryElemets.Any())
            {
                return _primaryElemets;
            }
            return null;
        }

        public void GoToPage()
        {
            try
            {
                GoToUrl(AttrUrl);
            }
            catch (UriFormatException)
            {
                throw new UriFormatException($"Url страницы \"{Name}\" пустой или содержит ошибки: url => \"{AttrUrl}\"");
            }
        }

        public void Close()
        {
            this._driver.WebDriver.Close();
        }

        public void Refresh()
        {
            this._driver.WebDriver.Navigate().Refresh();
        }

        public void Maximize()
        {
            this._driver.WebDriver.Manage().Window.Maximize();
        }

        public void PageTop()
        {
            var action = new Actions(this._driver.WebDriver);
            action.SendKeys(Keys.Control).SendKeys(Keys.Home).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        public void PageDown()
        {
            var action = new Actions(this._driver.WebDriver);
            action.SendKeys(Keys.Control).SendKeys(Keys.End).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        public void IsPageLoad()
        {
            var elements = new List<string>();

            _primaryElemets.ForEach(element =>
            {
                element.SetDriver(_driver);
                if (!element.IsLoad())
                {
                    elements.Add(element.Name);
                }
            });

            if (elements.Any())
            {
                var aggregate = string.Join(", ", elements);
                throw new ArgumentException($"элемент/ы \"{aggregate}\" не инициализированы на странице \"{Name}\"");
            }
        }

        public bool IsAppeared()
        {
            var countElement = 0;

            _primaryElemets.ForEach(element =>
            {
                element.SetDriver(_driver);
                if (!element.IsLoad())
                {
                    countElement++;
                }
            });

            return countElement == _primaryElemets.Count;
        }

        public bool IsDisappeared()
        {
            throw new NotImplementedException();
        }

        private void InitializeElements()
        {
            var fields = this.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<ElementAttribute>() != null)
                        .ToArray();

            CollectAllPageElement(fields);
        }

        private void CollectAllPageElement(FieldInfo[] fields)
        {
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<ElementAttribute>();
                var locator = _context.ReplaceVariablesInXmlBody(attr.Locator);
                WebElement = (IElement)Activator.CreateInstance(field.FieldType, attr.Name, locator);

                var newFields = WebElement.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<ElementAttribute>() != null)
                        .ToArray();

                if (!_allElements.ContainsKey(attr.Name))
                {
                    _allElements.Add(attr.Name, WebElement);
                }
                if (attr.Hidden)
                {
                    _hiddenElements.Add(WebElement);
                }
                if (!attr.Optional)
                {
                    _primaryElemets.Add(WebElement);
                }

                if (newFields.Length > 0)
                {
                    WebElement = null;
                    CollectAllPageElement(newFields);
                }
                WebElement = null;
            }
        }

        private void GoToUrl(string url)
        {
            _driver.WebDriver.Navigate().GoToUrl(new Uri(url));
            _driver.WebDriver.Wait(_driver.Timeout).ForPage().ReadyStateComplete();
        }
    }
}
