using System;
using System.Collections.Generic;
using System.Reflection;
using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
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
            field.Should().NotBeNull("Клафиша не найдена");

            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент {by} не найден");

            element.SendKeys((string)field?.GetValue(null));
        }

        public void PressKeys(By by, List<string> keys)
        {
            new NotImplementedException();
        }

        public void PressKeysAndSymbols(By by, List<string> keys, List<string> symbols)
        {
            new NotImplementedException();
        }
    }
}
