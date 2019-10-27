using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class ClickSupport
    {
        private readonly WebContext webContext;
        private readonly ElementSupport elementSupport;

        public ClickSupport(WebContext webContext, ElementSupport elementSupport)
        {
            this.webContext = webContext;
            this.elementSupport = elementSupport;
        }

        public void Click(By by)
        {
            this.elementSupport.BeEnabled(by);
            this.elementSupport.BeDisplayed(by);
            this.elementSupport.BeClickable(by);
        }

        public void DoubleClick(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            element.Wait(this.webContext.Timeout).ForElement().ToBeVisible();
            element.Wait(this.webContext.Timeout).ForElement().ToBeEnabled();
            var builder = new Actions(this.webContext.WebDriver);
            builder.DoubleClick(element).Build().Perform();
        }

        public void ClickAndHold(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            element.Wait(this.webContext.Timeout).ForElement().ToBeVisible();
            element.Wait(this.webContext.Timeout).ForElement().ToBeEnabled();
            var builder = new Actions(this.webContext.WebDriver);
            builder.ClickAndHold(element).Build().Perform();
        }
    }
}
