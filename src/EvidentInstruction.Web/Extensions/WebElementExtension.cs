using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
using System;

namespace EvidentInstruction.Web.Extensions
{
    public static class WebElementExtension
    {
        public static bool ToBeVisible(this IWebElement webElement, int waitMs)
        {
            return Execute(() => webElement.Wait(waitMs).ForElement().ToBeVisible());
        }

        public static bool ToBeInvisible(this IWebElement webElement, int waitMs)
        {
            return Execute(() => webElement.Wait(waitMs).ForElement().ToBeInvisible());
        }

        public static bool ToBeDisabled(this IWebElement webElement, int waitMs)
        {
            return Execute(() => webElement.Wait(waitMs).ForElement().ToBeDisabled());
        }

        public static bool ToBeEnabled(this IWebElement webElement, int waitMs)
        {
            return Execute(() => webElement.Wait(waitMs).ForElement().ToBeEnabled());
        }

        public static bool ToBeSelected(this IWebElement webElement, int waitMs)
        {
            return Execute(() => webElement.Wait(waitMs).ForElement().ToBeSelected());
        }

        public static bool ToNotBeSelected(this IWebElement webElement, int waitMs)
        {
            return Execute(() => webElement.Wait(waitMs).ForElement().ToNotBeSelected());
        }

        private static bool Execute(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}