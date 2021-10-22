using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitTypeSelections;

namespace Selenium.WebDriver.WaitExtensions
{
    public static class WebDriverExtensions
    {
        public static IWaitTypeSelection Wait(this IWebDriver webDriver, int ms = 500)
        {
            return new WaitTypeSelection(webDriver, ms);
        }
    }
}