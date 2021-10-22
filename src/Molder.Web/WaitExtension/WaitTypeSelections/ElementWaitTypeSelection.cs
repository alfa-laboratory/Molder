using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions.WaitConditions;

namespace Selenium.WebDriver.WaitExtensions.WaitTypeSelections
{
    public class ElementWaitTypeSelection : IElementWaitTypeSelection
    {
        private readonly IWebElement _webelement;
        private readonly int _delayMs;

        public ElementWaitTypeSelection(IWebElement webelement, int delayMs)
        {
            _webelement = webelement;
            _delayMs = delayMs;
        }

        public ITextWaitConditions ForText()
        {
            return new TextWaitConditions(_webelement, _delayMs);
        }

        public IClassWaitConditions ForClasses()
        {
            return new ClassWaitConditions(_webelement, _delayMs);
        }

        public IAttributeWaitConditions ForAttributes()
        {
            return new AttributeWaitConditions(_webelement, _delayMs);
        }

        public IElementWaitConditions ForElement()
        {
            return new ElementWaitConditions(_webelement, _delayMs);
        }
    }
}