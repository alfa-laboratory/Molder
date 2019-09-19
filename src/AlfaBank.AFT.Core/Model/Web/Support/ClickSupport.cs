using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class ClickSupport
    {
        private readonly WebContext webContext;

        public ClickSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void Click(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            element.Click();
        }

        public void DoubleClick(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            var builder = new Actions(this.webContext.WebDriver);
            builder.DoubleClick(element).Build().Perform();
        }

        public void ClickAndHold(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");

            var builder = new Actions(this.webContext.WebDriver);
            builder.ClickAndHold(element).Build().Perform();
        }
    }
}
