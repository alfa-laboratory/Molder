using System.Linq;
using System.Reflection;
using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class KeySupport
    {
        private readonly WebContext webContext;

        public KeySupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void PressKey(By by, string key)
        {
            var field = typeof(Keys).GetField(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            field.Should().NotBeNull("Клавиша не найдена");

            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент {by} не найден");
            element.Wait(this.webContext.Timeout).ForElement().ToBeVisible();
            element.Wait(this.webContext.Timeout).ForElement().ToBeEnabled();

            element.SendKeys((string)field?.GetValue(null));
        }

        public void PressKeysBy(By by, string keys)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            element.Wait(this.webContext.Timeout).ForElement().ToBeVisible();
            element.Wait(this.webContext.Timeout).ForElement().ToBeEnabled();
            Press(keys, element);
        }

        public void PressKeys(string keys)
        {
            Press(keys);
        }

        private void Press(string keys, IWebElement webElement = null)
        {
            var lk = keys.Split('+').ToList();

            if(!lk.Any())
                return;
            var actions = new Actions(this.webContext.WebDriver);
            foreach(var key in lk)
            {
                var field = typeof(Keys).GetField(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
                if(field != null)
                {
                    if (webElement is null)
                    {
                        actions.KeyDown((string)field.GetValue(null));
                    }
                    else
                    {
                        actions.KeyDown(webElement, (string)field.GetValue(null));
                    }
                }
                else
                {
                    if(webElement is null)
                    {
                        actions.SendKeys(key);
                    }
                    else
                    {
                        actions.SendKeys(webElement, key);
                    }
                }
            }

            actions.Build().Perform();
        }
    }
}
