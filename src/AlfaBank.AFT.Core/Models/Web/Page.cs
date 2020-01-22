using AlfaBank.AFT.Core.Models.Web.Attributes;
using AlfaBank.AFT.Core.Models.Web.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AlfaBank.AFT.Core.Models.Web
{
    public class Page : IPage
    {
        private readonly Dictionary<string, IElement> _allElements;
        private readonly List<IElement> _hiddenElements;
        private readonly List<IElement> _primaryElemets;

        private readonly Driver _driverSupport;

        public Page(Driver driverSupport)
        {
            _driverSupport = driverSupport;
            _allElements = new Dictionary<string, IElement>();
            _hiddenElements = new List<IElement>();
            _primaryElemets = new List<IElement>();
            Initialize();
        }

        public IElement GetElementByName(string name)
        {
            if (!_allElements.Any())
                throw new ArgumentException($"Элемент \"{name}\" не инициализирован на странице \"{GetName()}\"");
            if (!_allElements.ContainsKey(name))
                throw new ArgumentException($"Элемент \"{name}\" не инициализирован на странице \"{GetName()}\"");
            _allElements[name].SetDriver(_driverSupport);
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
            var attr = this.GetType()
                        .GetCustomAttribute<PageAttribute>();

            if (attr != null)
            {
                if (attr.Url != null)
                {
                    GoToUrl(attr.Url);
                }
                else
                {
                    throw new ArgumentException($"Атрибут \"Url\" не задан для страницы \"{GetName()}\"");
                }
                }
                else
                {
                    throw new ArgumentException($"Атрибуты не заданы для страницы \"{GetName()}\"");
                }
            }

        public void Close()
        {
            this._driverSupport.WebDriver.Close();
        }

        public string GetName()
        {
            var attr = this.GetType().GetCustomAttribute<PageAttribute>();

            return attr?.Name;
        }

        public string GetUrl()
        {
            return this._driverSupport.WebDriver.Url;
        }

        public string GetAttrUrl()
        {
            var attr = this.GetType()
            .GetCustomAttribute<PageAttribute>();

            return attr?.Url;
        }

        public string GetTitle()
        {
            return this._driverSupport.WebDriver.Title;
        }

        public void Refresh()
        {
            this._driverSupport.WebDriver.Navigate().Refresh();
        }

        public void Maximize()
        {
            this._driverSupport.WebDriver.Manage().Window.Maximize();
        }

        public void PageTop()
        {
            var action = new Actions(this._driverSupport.WebDriver);
            action.SendKeys(Keys.Control).SendKeys(Keys.Home).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        public void PageDown()
        {
            var action = new Actions(this._driverSupport.WebDriver);
            action.SendKeys(Keys.Control).SendKeys(Keys.End).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        public void IsPageLoad()
        {
            var elements = new List<string>();
            foreach (var element in _primaryElemets)
            {
                element.SetDriver(_driverSupport);
                if (!element.IsLoad())
                {
                    elements.Add(element.GetName());
                }
            }

            if (elements.Any())
            {
                var aggregate = string.Join(", ", elements);
                throw new ArgumentException($"элемент/ы \"{aggregate}\" не инициализированы на странице \"{GetName()}\"");
            }
        }

        public bool IsAppeared()
        {
            var countElement = 0;
            foreach (var element in _primaryElemets)
            {
                element.SetDriver(_driverSupport);
                if (element.IsLoad())
                {
                    countElement++;
                }
            }

            return countElement == _primaryElemets.Count;
        }

        public bool IsDisappeared()
        {
            throw new NotImplementedException();
        }

        private void Initialize()
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
                var instance = (IElement)Activator.CreateInstance(field.FieldType, attr.Name, attr.Locator);

                var newFields = instance.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<ElementAttribute>() != null)
                        .ToArray();

                if (!_allElements.ContainsKey(attr.Name))
                {
                    _allElements.Add(attr.Name, instance);
                }
                if (attr.Hidden)
                {
                    _hiddenElements.Add(instance);
                }
                if (!attr.Optional)
                {
                    _primaryElemets.Add(instance);
                }

                if (newFields.Length > 0)
                {
                    CollectAllPageElement(newFields);
                }
            }
        }

        private void GoToUrl(string url)
        {
            _driverSupport.WebDriver.Navigate().GoToUrl(new Uri(url));
            _driverSupport.WebDriver.Wait(_driverSupport.Timeout).ForPage().ReadyStateComplete();
        }
    }
}
