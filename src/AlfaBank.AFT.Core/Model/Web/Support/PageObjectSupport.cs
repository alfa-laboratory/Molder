using AlfaBank.AFT.Core.Model.Context;
using OpenQA.Selenium;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class PageObjectSupport
    {
        private readonly WebContext webContext;

        public PageObjectSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public Element GetElementByName(string name)
        {
            return this.webContext.PageObject[name];
        }

        public By GetParameterByName(string name)
        {
            var element = this.webContext.PageObject[name];
            if (!string.IsNullOrEmpty(element.Id))
            {
                return By.Id(element.Id);
            }
            if(!string.IsNullOrEmpty(element.Xpath))
            {
                return By.XPath(element.Xpath);
            }
            if(!string.IsNullOrEmpty(element.Classname))
            {
                return By.ClassName(element.Classname);
            }
            return !string.IsNullOrEmpty(element.Tag) ? By.TagName(element.Tag) : null;
        }
    }
}
