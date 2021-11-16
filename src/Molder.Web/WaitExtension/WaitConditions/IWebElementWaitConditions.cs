using OpenQA.Selenium;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public interface IWebElementWaitConditions
    {
        IWebElement ToExist();
        void ToNotExist();
    }
}