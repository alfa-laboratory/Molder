using Molder.Helpers;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class ElementProvider : IElementProvider
    {
        private int? _timeout;

        public ElementProvider(int? timeout)
        {
            _timeout = timeout ?? DefaultSetting.ELEMENT_TIMEOUT;
        }

        private AsyncLocal<IWebElement> _element = new AsyncLocal<IWebElement> { Value = null };

        public IWebElement Element
        {
            get => _element.Value;
            set
            {
                _element.Value = value;
            }
        }

        public bool Displayed => _element.Value.ToBeVisible((int)_timeout);

        public bool Selected => _element.Value.ToBeSelected((int)_timeout);

        public bool Enabled => _element.Value.ToBeEnabled((int)_timeout);

        public bool Loaded => _element.Value is null ? false : true;

        public bool Editabled => IsEditabled();

        public Point Location => _element.Value.Location;

        public string Text => _element.Value.Text;

        public string Tag => _element.Value.TagName;

        public void Clear()
        {
            _element.Value.Clear();
        }

        public void Click()
        {
            _element.Value.Click();
        }

        public bool TextEqual(string text)
        {
            try
            {
                return _element.Value.Wait((int)_timeout).ForText().ToEqual(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{_element.Value.Text}\" is not equal \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextContain(string text)
        {
            try
            {
                return _element.Value.Wait((int)_timeout).ForText().ToContain(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{_element.Value.Text}\" is not contain \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextMatch(string text)
        {
            try
            {
                return _element.Value.Wait((int)_timeout).ForText().ToMatch(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{_element.Value.Text}\" is not match \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public IElementProvider FindElement(By by)
        {
            var element = _element.Value.FindElement(by);
            return new ElementProvider(_timeout)
            {
                Element = element
            };
        }

        public ReadOnlyCollection<IElementProvider> FindElements(By by)
        {
            var elements = _element.Value.FindElements(by);
            var listElement = new List<IElementProvider>();
            foreach (var element in elements)
            {
                listElement.Add(new ElementProvider(_timeout)
                {
                    Element = element
                });
            }
            return listElement.AsReadOnly();
        }

        public string GetAttribute(string name)
        {
            return _element.Value.GetAttribute(name);
        }

        public string GetCss(string name)
        {
            return _element.Value.GetCssValue(name);
        }

        public void SendKeys(string keys)
        {
            _element.Value.SendKeys(keys);
        }

        private bool IsEditabled()
        {
            return Convert.ToBoolean(GetAttribute("readonly"));
        }
    }
}