using System;
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
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{_name}\" Enabled и Visible");
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
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{_name}\" Enabled и Visible");
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
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{_name}\" Enabled и Visible");
            }
        }
    }
}
