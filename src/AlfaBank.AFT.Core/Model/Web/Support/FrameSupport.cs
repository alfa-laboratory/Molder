using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class FrameSupport
    {
        private readonly WebContext webContext;

        public FrameSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void SwitchFrameBy(int number)
        {
            this.webContext.WebDriver.SwitchTo().Frame(number);
        }

        public void SwitchFrameBy(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент {by} не найден");

            this.webContext.WebDriver.SwitchTo().Frame(element);
        }

        public void SwitchToDefaultContent()
        {
            this.webContext.WebDriver.SwitchTo().DefaultContent();
        }

        public void SwitchToParentFrame()
        {
            this.webContext.WebDriver.SwitchTo().ParentFrame();
        }
    }
}
