using Molder.Helpers;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Threading;
using Molder.Web.Exceptions;
using Molder.Web.Extensions;
using Molder.Web.Models.Settings;
using OpenQA.Selenium.Support.UI;

namespace Molder.Web.Models.Providers
{
    [ExcludeFromCodeCoverage]
    public class ElementProvider : IElementProvider
    {
        private long? _timeout;
        private By _locator;

        #region WebElement

        private AsyncLocal<IWebElement> _element = new() { Value = null };

        public IWebElement WebElement
        {
            get
            {
                if (_element.Value is not null) return _element.Value;
                
                _element.Value = WebDriver.Wait((int)BrowserSettings.Settings.Timeout).ForElement(_locator).ToExist();
                return _element.Value;
            }
            set => _element.Value = value;
        }

        #endregion
        
        #region  WebDriver

        private AsyncLocal<IWebDriver> _driver = new() { Value = null };
        public IWebDriver WebDriver
        {
            get => _driver.Value;
            set => _driver.Value = value;
        }

        #endregion
        
        public ElementProvider(long? timeout, By locator)
        {
            _timeout = timeout;
            _locator = locator;
        }
        
        public bool Displayed => WebElement.Displayed;
        public bool NotDisplayed => !WebElement.Displayed;

        public bool Selected => WebElement.Selected;
        public bool NotSelected => !WebElement.Selected;

        public bool Enabled => WebElement.Enabled;
        public bool Disabled => !WebElement.Enabled;
        
        public bool Loaded => WebElement is not null;
        public bool NotLoaded => WebElement is null;

        public bool Editabled => IsEditabled();
        public bool NotEditabled => !IsEditabled();

        public Point Location => WebElement.Location;

        public string Text => WebElement.Text;

        public string Tag => WebElement.TagName;

        public void Clear()
        {
            try
            {
                WebElement.Clear();
            }
            catch (Exception ex)
            {
                throw new ElementException($"Clear element is return error with message {ex.Message}");
            }
        }

        public void Click()
        {
            try
            {
                WebElement.Click();
            }
            catch (Exception ex)
            {
                throw new ElementException($"Click element is return error with message {ex.Message}");
            }
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
            var element = WebElement.FindBy(by, WebDriver, (int) BrowserSettings.Settings.Timeout);
            return new ElementProvider(_timeout, by)
            {
                WebElement = element
            };
        }

        public ReadOnlyCollection<IElementProvider> FindElements(By by)
        {
            var elements = WebElement.FindAllBy(by, WebDriver, (int) BrowserSettings.Settings.Timeout);
            var listElement = elements.Select(element => new ElementProvider(_timeout, by) {WebElement = element}).Cast<IElementProvider>().ToList();
            return listElement.AsReadOnly();
        }

        public string GetAttribute(string name)
        {
            try
            {
                Log.Logger().LogDebug($"Get attribute by name \"{name}\"");
                return WebElement.GetAttribute(name);
            }
            catch (Exception ex)
            {
                throw new ElementException($"Get attribute by name \"{name}\" is return error with message {ex.Message}");
            }
        }

        public string GetCss(string name)
        {
            try
            {
                return WebElement.GetCssValue(name);
            }
            catch (Exception ex)
            {
                throw new ElementException($"GetCssValue by name \"{name}\" is return error with message {ex.Message}");
            }
        }

        public void SendKeys(string keys)
        {
            try
            {
                WebElement.SendKeys(keys);
            }
            catch (Exception ex)
            {
                throw new ElementException($"SendKeys \"{keys}\" in element is return error with message {ex.Message}");
            }
        }

        private bool IsEditabled()
        {
            return Convert.ToBoolean(GetAttribute("readonly"));
        }
        
        public void WaitUntilAttributeValueEquals(string attributeName, string attributeValue)
        {      
            var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds((long)BrowserSettings.Settings.Timeout));
            WebElement = wait.Until(_ => WebElement.GetAttribute(attributeName) == attributeValue ? WebElement : throw new ElementException($"Waiting until attribute \"{attributeName}\" becomes value \"{attributeValue ?? "null"}\" is failed"));
        }
    }
}