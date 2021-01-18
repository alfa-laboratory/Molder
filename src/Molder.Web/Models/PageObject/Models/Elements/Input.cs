using System;

namespace Molder.Web.Models.PageObject.Models.Elements
{
    public class Input : Element
    {
        public Input(string name, string locator, bool optional) : base(name, locator, optional) { }

        public virtual void SetText(string text)
        {
            if (Enabled && Displayed)
            {
                _mediator.Execute(() => _provider.SendKeys(text));
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{_name}\" Enabled и Displayed");
            }
        }

        public void Clear()
        {
            if (Enabled && Displayed)
            {
                _mediator.Execute(() => _provider.Clear());
            }
            else
            {
                throw new ArgumentException($"Проверьте, что элемент \"{_name}\" Enabled и Displayed");
            }
        }
    }
}
