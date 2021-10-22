using OpenQA.Selenium;

namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public class ElementWaitConditions : WaitConditionsBase, IElementWaitConditions
    {
        private readonly IWebElement _webelement;

        public ElementWaitConditions(IWebElement webelement, int waitMs) : base(waitMs)
        {
            _webelement = webelement;
        }

        public void ToBeVisible()
        {
            WaitFor(() => _webelement.Displayed);
        }

        public void ToBeInvisible()
        {
            WaitFor(() => !_webelement.Displayed);
        }
        public void ToBeDisabled()
        {
            WaitFor(() => !_webelement.Enabled);
        }
        public void ToBeEnabled()
        {
            WaitFor(() => _webelement.Enabled);
        }
        public void ToBeSelected()
        {
            WaitFor(() => _webelement.Selected);
        }
        public void ToNotBeSelected()
        {
            WaitFor(() => !_webelement.Selected);
        }
    }
}