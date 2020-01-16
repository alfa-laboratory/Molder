using AlfaBank.AFT.Core.Models.Web.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Diagnostics;
using System.Reflection;

namespace AlfaBank.AFT.Core.Models.Web
{
    public abstract class Element : IElement
    {
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(25);

        protected string _xpath;
        protected string _name;
        protected Driver _driverSupport;

        protected Element(string name, string xpath)
        {
            this._name = name;
            this._xpath = xpath;
        }
        public virtual void SetDriver(Driver driver)
        {
            _driverSupport = driver;
        }

        public virtual void MoveTo()
        {
            var action = new Actions(_driverSupport.WebDriver);
            var element = GetWebElement();
            action.MoveToElement(element).Build().Perform();
        }

        public virtual string GetText()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();
                return element.Text;
            }
            return null;
        }

        public virtual string GetValue()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();
                return element.GetAttribute("value");
            }
            return null;
        }

        public virtual string GetAttribute(string name)
        {
            var element = GetWebElement();
            return element.GetAttribute(name);
        }

        public virtual void PressKey(string key)
        {
            var field = typeof(Keys).GetField(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();
                element.SendKeys((string)field?.GetValue(null));
            }
        }

        public bool IsTextContains(string text)
        {
            try
            {
                var element = GetWebElement();
                return element.Wait(this._driverSupport.Timeout).ForText().ToContain(text);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public bool IsTextEquals(string text)
        {
            try
            {
                var element = GetWebElement();
                return element.Wait(this._driverSupport.Timeout).ForText().ToEqual(text);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public bool IsTextChange(string text)
        {
            try
            {
                var element = GetWebElement();
                return waitTextChange(() => element.Text, text);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public bool IsValueChange(string text)
        {
            try
            {
                var element = GetWebElement();
                return waitTextChange(() => element.GetAttribute("value"), text);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public bool IsLoad()
        {
            try
            {
                GetWebElement();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public bool IsDisabled()
        {
            var element = GetWebElement();
            try
            {
                element.Wait(this._driverSupport.Timeout).ForElement().ToBeDisabled();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public virtual bool IsEnabled()
        {
            var element = GetWebElement();
            try
            {
                element.Wait(this._driverSupport.Timeout).ForElement().ToBeEnabled();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsVisible()
        {
            var element = GetWebElement();
            try
            {
                element.Wait(this._driverSupport.Timeout).ForElement().ToBeVisible();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsInvisible()
        {
            var element = GetWebElement();
            try
            {
                element.Wait(this._driverSupport.Timeout).ForElement().ToBeInvisible();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public virtual bool IsSelected()
        {
            var element = GetWebElement();
            try
            {
                element.Wait(this._driverSupport.Timeout).ForElement().ToBeSelected();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsNotSelected()
        {
            var element = GetWebElement();
            try
            {
                element.Wait(this._driverSupport.Timeout).ForElement().ToNotBeSelected();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsEditable()
        {
            var element = GetWebElement();
            try
            {
                var onlyRead = element.GetAttribute("readonly");
                return Convert.ToBoolean(onlyRead);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch(FormatException)
            {
                return false;
            }
        }

        protected IWebElement GetWebElement(string xpath = null)
        {
            return xpath == null
                ? _driverSupport.WebDriver.Wait(_driverSupport.Timeout).ForElement(By.XPath(_xpath)).ToExist()
                : _driverSupport.WebDriver.Wait(_driverSupport.Timeout).ForElement(By.XPath(xpath)).ToExist();        
        }

        private bool waitTextChange(Func<string> test, string text)
        {
            var stopwatch = new Stopwatch();

            while (stopwatch.ElapsedMilliseconds <= _driverSupport.Timeout)
            {
                var str = test();
                if (!str.Equals(text))
                    return true;
                System.Threading.Thread.Sleep(_interval);
            }

            return false;
        }
    }
}
