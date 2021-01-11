using EvidentInstruction.Helpers;
using EvidentInstruction.Web.Models.WaitTypeSelections.Interfaces;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitConditions;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Web.Models.WaitTypeSelections
{
    [ExcludeFromCodeCoverage]
    public class ElementWaitType : WaitConditionsBase, IElementWait
    {
        private readonly IWebElement _webelement;

        public ElementWaitType(IWebElement webelement, int waitMs) : base(waitMs)
        {
            _webelement = webelement;
        }

        public bool ToBeDisabled()
        {
            try
            {
                return WaitFor(() => _webelement.Displayed);
            }
            catch(WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"{_webelement} is not displayed. Exception is {ex.Message}");
                return false;
            }
        }

        public bool ToBeEnabled()
        {
            try
            {
                return WaitFor(() => _webelement.Enabled);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"{_webelement} is not enabled. Exception is {ex.Message}");
                return false;
            }
        }

        public bool ToBeInvisible()
        {
            try
            {
                return WaitFor(() => !_webelement.Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"{_webelement} is not invisible. Exception is {ex.Message}");
                return false;
            }
        }

        public bool ToBeSelected()
        {
            try
            {
                return WaitFor(() => _webelement.Selected);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"{_webelement} is not selected. Exception is {ex.Message}");
                return false;
            }
        }

        public bool ToBeVisible()
        {
            try
            {
                return WaitFor(() => _webelement.Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"{_webelement} is not visible. Exception is {ex.Message}");
                return false;
            }
        }

        public bool ToNotBeSelected()
        {
            try
            {
                return WaitFor(() => !_webelement.Selected);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"{_webelement} is not be selected. Exception is {ex.Message}");
                return false;
            }
        }
    }
}
