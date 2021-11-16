using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitConditions;

namespace Selenium.WebDriver.WaitExtensions.WaitTypeSelections
{
    public interface IWaitTypeSelection
    {
        IWebElementWaitConditions ForElement(By @by);
        IWebPageWaitConditions ForPage();
    }
}