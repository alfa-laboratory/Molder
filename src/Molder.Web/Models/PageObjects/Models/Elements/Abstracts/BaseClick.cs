using Molder.Web.Models.Providers;
using OpenQA.Selenium.Interactions;
using System;

namespace Molder.Web.Models.PageObjects.Elements
{
    public abstract class BaseClick : Element
    {
        public BaseClick(string name, string locator, bool optional = false) : base(name, locator, optional) { }

        public virtual void Click()
        {
            if (Enabled && Displayed)
            {
                _mediator.Execute(() => _provider.Click());
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
                var action = new Actions(_driverProvider.GetDriver());
                _mediator.Execute(() => action.DoubleClick(((ElementProvider)_provider).Element).Build().Perform());
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
                var action = new Actions(_driverProvider.GetDriver());
                _mediator.Execute(() => action.ClickAndHold(((ElementProvider)_provider).Element).Build().Perform());
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }
    }
}