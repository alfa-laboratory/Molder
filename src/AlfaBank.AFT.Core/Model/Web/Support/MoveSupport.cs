using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class MoveSupport
    {
        private readonly WebContext webContext;

        public MoveSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void MoveToElement(By by)
        {
            var action = new Actions(this.webContext.WebDriver);
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            action.MoveToElement(element).Build().Perform();
        }

        public void PageTop()
        {
            var action = new Actions(this.webContext.WebDriver);
            action.SendKeys(Keys.Home).Build().Perform();
        }

        public void PageDown()
        {
            var action = new Actions(this.webContext.WebDriver);
            action.SendKeys(Keys.Control).SendKeys(Keys.End).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }
    }
}
