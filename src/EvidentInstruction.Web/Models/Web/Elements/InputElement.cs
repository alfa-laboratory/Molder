using System;

namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class InputElement : Element
    {
        public InputElement(string name, string xpath) : base(name, xpath) { }

        public virtual void SetText(string text)
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();

                element.SendKeys(text);
            }
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{_name}\" Enabled и Visible");
            }
        }

        public virtual void Clear()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = GetWebElement();

                element.Clear();
            }
            else
            {
                throw new ArgumentNullException($"Проверьте, что элемент \"{_name}\" Enabled и Visible");
            }
        }
    }
}