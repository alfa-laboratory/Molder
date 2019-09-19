using System;
using AlfaBank.AFT.Core.Model.Context;
using Selenium.WebDriver.WaitExtensions;

namespace AlfaBank.AFT.Core.Model.Web.Support
{
    public class NavigationSupport
    {
        private readonly WebContext webContext;

        public NavigationSupport(WebContext webContext)
        {
            this.webContext = webContext;
        }

        public void NavigateTo(string url)
        {
            webContext.WebDriver.Navigate().GoToUrl(new Uri(url));
            webContext.WebDriver.Wait(this.webContext.Timeout).ForPage().ReadyStateComplete();
        }

        public void Refresh()
        {
            webContext.WebDriver.Navigate().Refresh();
            webContext.WebDriver.Wait(this.webContext.Timeout).ForPage().ReadyStateComplete();
        }
    }
}
