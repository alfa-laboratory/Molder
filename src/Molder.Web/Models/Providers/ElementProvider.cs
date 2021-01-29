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

        [ThreadStatic]
        public IWebElement Element = null;

        public bool Displayed => Element.ToBeVisible((int)_timeout);

        public bool Selected => Element.ToBeSelected((int)_timeout);

        public bool Enabled => Element.ToBeEnabled((int)_timeout);

        public bool Loaded => Element is null ? false : true;

        public bool Editabled => IsEditabled();

        public Point Location => Element.Location;

        public string Text => Element.Text;

        public string Tag => Element.TagName;

        public void Clear()
        {
            Element.Clear();
        }

        public void Click()
        {
            Element.Click();
        }

        public bool TextEqual(string text)
        {
            try
            {
                return Element.Wait((int)_timeout).ForText().ToEqual(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{Element.Text}\" is not equal \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextContain(string text)
        {
            try
            {
                return Element.Wait((int)_timeout).ForText().ToContain(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{Element.Text}\" is not contain \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextMatch(string text)
        {
            try
            {
                return Element.Wait((int)_timeout).ForText().ToMatch(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{Element.Text}\" is not match \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public IElementProvider FindElement(By by)
        {
            var element = Element.FindElement(by);
            return new ElementProvider(_timeout)
            {
                Element = element
            };
        }

        public ReadOnlyCollection<IElementProvider> FindElements(By by)
        {
            var elements = Element.FindElements(by);
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
            return Element.GetAttribute(name);
        }

        public string GetCss(string name)
        {
            return Element.GetCssValue(name);
        }

        public void SendKeys(string keys)
        {
            Element.SendKeys(keys);
        }

        private bool IsEditabled()
        {
            return Convert.ToBoolean(GetAttribute("readonly"));
        }
    }
}