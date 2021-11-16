using Selenium.WebDriver.WaitExtensions.WaitConditions;

namespace Selenium.WebDriver.WaitExtensions.WaitTypeSelections
{
    public interface IElementWaitTypeSelection
    {
        ITextWaitConditions ForText();
        IClassWaitConditions ForClasses();
        IAttributeWaitConditions ForAttributes();
        IElementWaitConditions ForElement();
    }
}