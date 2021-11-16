using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitTypeSelections;

namespace Selenium.WebDriver.WaitExtensions
{
    public static class WebElementExtensions
    {
        public static IElementWaitTypeSelection Wait(this IWebElement webelement , int ms = 500)
        {
            return new ElementWaitTypeSelection(webelement, ms);
        }

    }
}