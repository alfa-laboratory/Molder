using System;

namespace Molder.Web.Models.PageObjects.Elements
{
    public class Input : Element
    {
        public Input(string name, string locator, bool optional) : base(name, locator, optional) { }

        public virtual void SetText(string text)
        {
            if (Enabled && Displayed)
            {
                mediator.Execute(() => ElementProvider.SendKeys(text));
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }

        public new void Clear()
        {
            if (Enabled && Displayed)
            {
                mediator.Execute(() => ElementProvider.Clear());
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{Name}\" Enabled и Displayed");
            }
        }
    }
}