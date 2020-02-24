using OpenQA.Selenium.Interactions;

namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class ClickElement : Element
    {
        public ClickElement(string name, string xpath) : base(name, xpath) { }

        public virtual void Click()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();

                element.Click();
            }
        }

        public virtual void DoubleClick()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();

                var builder = new Actions(_driverSupport.WebDriver);
                builder.DoubleClick(element).Build().Perform();
            }
        }

        public virtual void ClickAndHold()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();

                var builder = new Actions(_driverSupport.WebDriver);
                builder.ClickAndHold(element).Build().Perform();
            }
        }
    }
}
