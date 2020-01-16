namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class InputElement : Element
    {
        public InputElement(string name, string xpath) : base(name, xpath) { }

        public virtual void SetText(string text)
        {
            if (IsEnabled() && IsVisible())
            {
                var element = this.GetWebElement();
                element.SendKeys(text);
            }
        }

        public virtual void Clear()
        {
            if (IsEnabled() && IsVisible())
            {
                var element = this.GetWebElement();
                element.Clear();
            }
        }
    }
}