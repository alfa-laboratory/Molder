using Molder.Helpers;
using Molder.Web.Extensions;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class ElementProvider : IElementProvider
    {
        private long? _timeout;

        #region WebElement

        private AsyncLocal<IWebElement> _element = new AsyncLocal<IWebElement>{ Value = null };

        public IWebElement WebElement
        {
            get => _element.Value;
            set => _element.Value = value;
        }

        #endregion
        
        public ElementProvider(long? timeout) => _timeout = timeout;
        public bool Displayed => WebElement.ToBeVisible((int)_timeout);
        public bool NotDisplayed => WebElement.ToBeInvisible((int)_timeout);

        public bool Selected => WebElement.ToBeSelected((int)_timeout);
        public bool NotSelected => WebElement.ToNotBeSelected((int)_timeout);

        public bool Enabled => WebElement.ToBeEnabled((int)_timeout);
        public bool Disabled => WebElement.ToBeDisabled((int)_timeout);
        
        public bool Loaded => !(WebElement is null);
        public bool NotLoaded => WebElement is null;

        public bool Editabled => IsEditabled();
        public bool NotEditabled => !IsEditabled();

        public Point Location => WebElement.Location;

        public string Text => WebElement.Text;

        public string Tag => WebElement.TagName;

        public void Clear()
        {
            WebElement.Clear();
        }

        public void Click()
        {
            WebElement.Click();
        }

        public bool TextEqual(string text)
        {
            try
            {
                return WebElement.Wait((int)_timeout).ForText().ToEqual(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{WebElement.Text}\" is not equal \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextContain(string text)
        {
            try
            {
                return WebElement.Wait((int)_timeout).ForText().ToContain(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{WebElement.Text}\" is not contain \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextMatch(string text)
        {
            try
            {
                return WebElement.Wait((int)_timeout).ForText().ToMatch(text);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{WebElement.Text}\" is not match \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public IElementProvider FindElement(By by)
        {
            var element = WebElement.FindElement(by);
            return new ElementProvider(_timeout)
            {
                WebElement = element
            };
        }

        public ReadOnlyCollection<IElementProvider> FindElements(By by)
        {
            var elements = WebElement.FindElements(by);
            var listElement = elements.Select(element => new ElementProvider(_timeout) {WebElement = element}).Cast<IElementProvider>().ToList();
            return listElement.AsReadOnly();
        }

        public string GetAttribute(string name)
        {
            return WebElement.GetAttribute(name);
        }

        public string GetCss(string name)
        {
            return WebElement.GetCssValue(name);
        }

        public void SendKeys(string keys)
        {
            WebElement.SendKeys(keys);
        }

        private bool IsEditabled()
        {
            return Convert.ToBoolean(GetAttribute("readonly"));
        }
    }
}