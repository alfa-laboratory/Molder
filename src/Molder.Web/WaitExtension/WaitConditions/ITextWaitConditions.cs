namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public interface ITextWaitConditions
    {
        bool ToEqual(string text);
        bool ToContain(string text);
        bool ToNotEqual(string text);
        bool ToNotContain(string text);
        bool ToMatch(string regex);
        bool ToNotMatch(string regex);
    }
}