using Molder.Web.Models.Providers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Molder.Web.Models.PageObjects.Elements
{
    public class Dropdown : Element
    {
        public Dropdown(string name, string locator, bool optional = false) : base(name, locator, optional)  { }

        public void SelectByValue(string value)
        {
            var select = new SelectElement((IWebElement)mediator.Execute(() => ((ElementProvider)_provider).WebElement));
            select.SelectByValue(value);
        }

        public void SelectByText(string text)
        {
            var select = new SelectElement((IWebElement)mediator.Execute(() => ((ElementProvider)_provider).WebElement));
            select.SelectByText(text);
        }

        public void SelectByIndex(int index)
        {
            var select = new SelectElement((IWebElement)mediator.Execute(() => ((ElementProvider)_provider).WebElement));
            select.SelectByIndex(index);
        }
    }
}