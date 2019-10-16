using AlfaBank.AFT.Core.Model.Context;
using FluentAssertions;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class TextBoxSupport
    {
        private readonly WebContext webContext;

        public TextBoxSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void SetText(By by, string value)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            element.Clear();
            element.SendKeys(value);
        }

        public void SetTextWithoutClear(By by, string value)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            element.SendKeys(value);
        }

        public void Clear(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            element.Clear();
        }

        public string GetValue(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            return element.GetAttribute("value");
        }

        public string GetText(By by)
        {
            var element = this.webContext.WebDriver.Wait(this.webContext.Timeout).ForElement(by).ToExist();
            element.Should().NotBeNull($"Элемент \"{by}\" не найден");
            return element.Text;
        }
    }
}
