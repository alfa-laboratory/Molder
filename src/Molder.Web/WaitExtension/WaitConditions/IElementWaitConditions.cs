namespace Selenium.WebDriver.WaitExtensions.WaitConditions
{
    public interface IElementWaitConditions
    {
        void ToBeVisible();
        void ToBeInvisible();
        void ToBeDisabled();
        void ToBeEnabled();
        void ToBeSelected();
        void ToNotBeSelected();
    }
}