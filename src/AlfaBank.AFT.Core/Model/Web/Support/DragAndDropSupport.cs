using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class DragAndDropSupport
    {
        private readonly WebContext webContext;

        public DragAndDropSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void DragAndDrop(By source, By target)
        {
            var action = new Actions(this.webContext.WebDriver);
            var elementSource = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(source).ToExist();
            elementSource.Should().NotBeNull($"Элемент \"{source}\" не найден");
            var elementTarget = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(target).ToExist();
            elementTarget.Should().NotBeNull($"Элемент \"{target}\" не найден");
            action.DragAndDrop(elementSource, elementTarget).Build().Perform();
        }
    }
}
