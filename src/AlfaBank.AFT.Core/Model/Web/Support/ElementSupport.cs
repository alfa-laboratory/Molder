using System;
using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class ElementSupport
    {
        private readonly WebContext webContext;

        public ElementSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void BeNull(By by)
        {
            var act = new Action(() => this.webContext.WebDriver.Wait().ForElement(by).ToExist());
            act.Should().Throw<WebDriverTimeoutException>();
        }

        public void NotBeNull(By by)
        {
            var act = new Action(() => this.webContext.WebDriver.Wait().ForElement(by).ToExist());
            act.Should().NotThrow<WebDriverTimeoutException>();
        }

        public void BeDisplayed(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");

            var act = new Action(() => element.Wait(this.webContext.Timeout).ForElement().ToBeVisible());
            act.Should().NotThrow<WebDriverTimeoutException>();
        }

        public void NotBeDisplayed(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");

            var act = new Action(() => element.Wait(this.webContext.Timeout).ForElement().ToBeInvisible());
            act.Should().NotThrow<WebDriverTimeoutException>();
        }

        public void BeDisabled(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");

            var act = new Action(() => element.Wait(this.webContext.Timeout).ForElement().ToBeDisabled());
            act.Should().NotThrow<WebDriverTimeoutException>();
        }

        public void BeEnabled(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");

            var act = new Action(() => element.Wait(this.webContext.Timeout).ForElement().ToBeEnabled());
            act.Should().NotThrow<WebDriverTimeoutException>();
        }

        public void BeSelected(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");

            var act = new Action(() => element.Wait(this.webContext.Timeout).ForElement().ToBeSelected());
            act.Should().NotThrow<WebDriverTimeoutException>();
        }

        public void NotBeSelected(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");

            var act = new Action(() => element.Wait(this.webContext.Timeout).ForElement().ToNotBeSelected());
            act.Should().NotThrow<WebDriverTimeoutException>();
        }
    }
}
