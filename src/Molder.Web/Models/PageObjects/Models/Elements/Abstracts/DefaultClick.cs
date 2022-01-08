using Molder.Web.Models.Providers;
using OpenQA.Selenium.Interactions;
using System;

namespace Molder.Web.Models.PageObjects.Elements
{
    public abstract class DefaultClick : Element
    {
        public DefaultClick(string name, string locator, bool optional = false) : base(name, locator, optional) { }

        public virtual void Click()
        {
            if (Enabled && Displayed)
            {
                mediator.Execute(() => ElementProvider.Click());
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }

        public virtual void DoubleClick()
        {
            if (Enabled && Displayed)
            {
                var action = new Actions(Driver.GetDriver());
                mediator.Execute(() => action.DoubleClick(((ElementProvider)ElementProvider).WebElement).Build().Perform());
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }

        public virtual void ClickAndHold()
        {
            if (Enabled && Displayed)
            {
                var action = new Actions(Driver.GetDriver());
                mediator.Execute(() => action.ClickAndHold(((ElementProvider)ElementProvider).WebElement).Build().Perform());
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }
    }
}